using Core.EventStore.Autofac;
using Core.EventStore.Invokers;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.EventStore.Registration
{
    public class PersistentSubscriptionClient
    {
        private readonly IEventStoreConnection eventStoreConnection;
        private readonly ProjectorInvoker projectorInvoker;
        public PersistentSubscriptionClient(IEventStoreConnection _eventStoreConnection, ProjectorInvoker _projectorInvoker)
        {
            eventStoreConnection = _eventStoreConnection;
            projectorInvoker = _projectorInvoker;
        }

        List<string> streams = new List<string>()
            {
            "CustomerCreatedEvent","UserCreatedEvent"
            };


        private static readonly UserCredentials User = new UserCredentials("admin", "changeit");
        private EventStoreSubscription _subscription;

        public void Start()
        {
            using (eventStoreConnection)
            {
                //eventStoreConnection.ConnectAsync().Wait();
                CreateSubscription();

                Console.WriteLine("waiting for events. press enter to exit");
                Console.ReadLine();
            }
        }


        private void CreateSubscription()
        {
            PersistentSubscriptionSettings settings = PersistentSubscriptionSettings.Create()
                .DoNotResolveLinkTos()
                .StartFromCurrent();

            try
            {
                eventStoreConnection.SubscribeToAllAsync(false, EventAppeared, SubscriptionDropped, User).Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException.GetType() != typeof(InvalidOperationException)
                    && ex.InnerException?.Message != $"Subscription group on stream already exists")
                {
                    throw;
                }
            }
        }

        private async void ConnectToSubscription()
        {
            try
            {
                _subscription = await eventStoreConnection.SubscribeToAllAsync(false, EventAppeared, SubscriptionDropped, User);
            }
            catch
            {
                eventStoreConnection.ConnectAsync().Wait();
                _subscription = await eventStoreConnection.SubscribeToAllAsync(false, EventAppeared, SubscriptionDropped, User);
            }
        }

        private void SubscriptionDropped(EventStoreSubscription eventStorePersistentSubscriptionBase,
           SubscriptionDropReason subscriptionDropReason, Exception ex)
        {
            ConnectToSubscription();
        }

        private Task EventAppeared(EventStoreSubscription eventStorePersistentSubscriptionBase,
            ResolvedEvent resolvedEvent)
        {
            var eventName = resolvedEvent.Event.EventStreamId;
            var jsonBytes= resolvedEvent.Event.Data;
            var eventId= resolvedEvent.Event.EventId;


            var eventContext = new EventStoreContext()
            {
                EventId = eventId,
                ResolvedEvent= resolvedEvent,
                EventName = eventName
            };

            projectorInvoker.Invoke(eventContext);

            return Task.FromResult(0);
        }

    }
}
