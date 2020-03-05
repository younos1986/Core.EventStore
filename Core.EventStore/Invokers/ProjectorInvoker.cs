using Core.EventStore.Autofac;
using Core.EventStore.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Core.EventStore.IdempotencyServices;
using Core.EventStore.Positions;
using Core.EventStore.Services;

namespace Core.EventStore.Invokers
{
    public abstract class ProjectorInvoker
    {
        public abstract void BeforeInvoke(EventStoreContext eventContext);

        public bool Invoke(EventStoreContext eventContext)
        {
            if (eventContext.SubscribedEvents.All(q => q.Key != eventContext.EventName))
                return false;
            
            if (IsProcessedBefore(eventContext)) return false;

            BeforeInvoke(eventContext);
            InvokeEvent(eventContext);

            PersistPositionAsync(eventContext).GetAwaiter();
            PersistIdempotencyAsync(eventContext).GetAwaiter();

            AfterInvoke(eventContext);

            return true;
        }

        private static bool IsProcessedBefore(EventStoreContext eventContext)
        {
            var idempotencyReaderService = eventContext.Container.ResolveOptional<IIdempotencyReaderService>();
            if (idempotencyReaderService == null)
                return false;

            var isProcessedBefore = idempotencyReaderService.IsProcessedBefore(eventContext.EventId).GetAwaiter()
                .GetResult();
            return isProcessedBefore;
        }

        private void InvokeEvent(EventStoreContext eventContext)
        {
            var jsonData = Encoding.ASCII.GetString(eventContext.ResolvedEvent.Event.Data);

            var eventType = eventContext.SubscribedEvents.FirstOrDefault(q => q.Key == eventContext.EventName);
            if (eventType.Equals(default(KeyValuePair<string, object>)))
                return;

            Type enumerableType = typeof(IEnumerable<>);
            Type projectorType = typeof(IProjector<>);
            Type type = ((Type) eventType.Value);
            Type genericType = projectorType.MakeGenericType(type);
            var enumerableGenericType = enumerableType.MakeGenericType(genericType);

            var resolvedType = eventContext.Container.Resolve(enumerableGenericType);

            var enumerator = ((IEnumerable) resolvedType).GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentProjectorClass = enumerator.Current;
                if (currentProjectorClass == null)
                    continue;

                var handlerMethodInfo = currentProjectorClass.GetType().GetMethod("HandleAsync");
                if (handlerMethodInfo == null)
                    throw new Exception("The Projector class doesn't have a HandleAsync method");

                var parameterInfo = handlerMethodInfo.GetParameters().FirstOrDefault();
                object deserializedJsonObject = DeserializeObject(jsonData, parameterInfo);

                handlerMethodInfo.Invoke(currentProjectorClass, new object[] {deserializedJsonObject});
            }
        }

        private static object DeserializeObject(string data, ParameterInfo parameterInfo)
        {
            return JsonConvert.DeserializeObject(data, parameterInfo.ParameterType);
        }

        public abstract void AfterInvoke(EventStoreContext eventContext);


        static List<Type> types = new List<Type>();
        private static object lockObject = new object();

        private List<Type> GetAllTypesImplementingOpenGenericType(Type openGenericType)
        {
            if (types.Any())
                return types;

            lock (lockObject)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                List<Type> tempTypes = new List<Type>();
                foreach (var assm in assemblies)
                {
                    var query = from x in assm.GetTypes()
                        from z in x.GetInterfaces()
                        let y = x.BaseType
                        where
                            (y != null && y.IsGenericType &&
                             openGenericType.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                            (z.IsGenericType &&
                             openGenericType.IsAssignableFrom(z.GetGenericTypeDefinition()))
                        select x;

                    var res = query.ToList();
                    tempTypes.AddRange(res);
                }

                types = tempTypes;
            }

            return types;
        }

        private async Task PersistPositionAsync(EventStoreContext container)
        {
            try
            {
                var positionWriteService = container.Container.ResolveOptional<IPositionWriteService>();
                if (positionWriteService == null)
                    return;

                var position = new EventStorePosition()
                {
                    Id = container.ResolvedEvent.Event.EventId,
                    CommitPosition = container.ResolvedEvent.OriginalPosition.Value.CommitPosition,
                    PreparePosition = container.ResolvedEvent.OriginalPosition.Value.PreparePosition,
                    CreatedOn = DateTime.UtcNow,
                };
                await positionWriteService.InsertOneAsync(position);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task PersistIdempotencyAsync(EventStoreContext container)
        {
            try
            {
                var idempotencyWriterService = container.Container.ResolveOptional<IIdempotencyWriterService>();
                if (idempotencyWriterService == null)
                    return;

                var eventStoreIdempotency = new EventStoreIdempotency()
                {
                    Id = container.ResolvedEvent.Event.EventId,
                    CreatedOn = DateTime.UtcNow,
                };
                await idempotencyWriterService.PersistIdempotencyAsync(eventStoreIdempotency);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}