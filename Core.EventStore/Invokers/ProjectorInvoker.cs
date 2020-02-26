using Core.EventStore.Autofac;
using Core.EventStore.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.EventStore.Invokers
{
    public abstract class ProjectorInvoker
    {


        public abstract void BeforeInvoke(EventContext eventContext);

        public bool Invoke(EventContext eventContext) // Guid eventId,string eventName, byte[] jsonBytes)
        {
            var jsonData = Encoding.ASCII.GetString(eventContext.ResolvedEvent.Event.Data);
            
            BeforeInvoke(eventContext);

            InvokeEvent(eventContext.EventId, eventContext.EventName, jsonData);

            AfterInvoke(eventContext);

            return true;
        }


        private void InvokeEvent(Guid eventId,string eventName, string jsonData)
        {
            var events = Autofac.SubscriptionConfiguration.Events;
            var eventType = events.FirstOrDefault(q => q.Key == eventName);
            if (eventType.Equals(default(KeyValuePair<string, object>)))
                return;

            var obj = eventType.Value;

            var allProjectors = GetAllTypesImplementingOpenGenericType(typeof(IProjector<>));
            var objName = ((Type)obj).Name;
            var handlerObject = allProjectors.FirstOrDefault(q => q.Name == objName + "EventProjector");

            object projectorInstance = Activator.CreateInstance(handlerObject);
            var handlerMethodInfo = projectorInstance.GetType().GetMethod("HandleAsync");

            var parameterInfo = handlerMethodInfo.GetParameters().FirstOrDefault();

            object deserializedJsonObject = DeserializeObject(jsonData, parameterInfo);

            handlerMethodInfo.Invoke(projectorInstance, new object[] { deserializedJsonObject });
        }

        private static object DeserializeObject(string data, ParameterInfo parameterInfo)
        {
            return JsonConvert.DeserializeObject(data, parameterInfo.ParameterType);
        }

        public abstract void AfterInvoke(EventContext eventContext);


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
    }
}