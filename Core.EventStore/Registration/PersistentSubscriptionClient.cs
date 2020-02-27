using Core.EventStore.Autofac;
using Core.EventStore.Invokers;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.EventStore.Managers;

namespace Core.EventStore.Registration
{
    public class PersistentSubscriptionClient : IPersistentSubscriptionClient
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly IEventStoreConnectionManager _eventStoreConnectionManager; 
        private readonly ProjectorInvoker _projectorInvoker;
        public PersistentSubscriptionClient(IEventStoreConnection eventStoreConnection, ProjectorInvoker projectorInvoker, IEventStoreConnectionManager eventStoreConnectionManager)
        {
            _eventStoreConnection = eventStoreConnection;
            _projectorInvoker = projectorInvoker;
            _eventStoreConnectionManager = eventStoreConnectionManager;
        }

        private static readonly UserCredentials User = new UserCredentials("admin", "changeit");
        private EventStoreSubscription _subscription;

        public void Start()
        {
            //using (_eventStoreConnection)
            {
                _eventStoreConnectionManager.Start();
                //eventStoreConnection.ConnectAsync().Wait();
                CreateSubscription();

                Console.WriteLine("waiting for events. press enter to exit");
                //Console.ReadLine();
                //Thread.Sleep()
            }
        }


        private void CreateSubscription()
        {
            var settings = ConnectionSettings
                .Create()
                .KeepReconnecting()
                .KeepRetrying()
                //.EnableVerboseLogging()
                .UseConsoleLogger();

            // PersistentSubscriptionSettings settings = PersistentSubscriptionSettings.Create()
            //     .DoNotResolveLinkTos()
            //     .StartFromCurrent();

            try
            {
                _eventStoreConnection.SubscribeToAllAsync(false, EventAppeared, SubscriptionDropped, User).Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException != null && (ex.InnerException.GetType() != typeof(InvalidOperationException)
                                                  && ex.InnerException?.Message != $"Subscription group on stream already exists"))
                {
                    throw;
                }
            }
        }

        private async void ConnectToSubscription()
        {
            try
            {
                _subscription = await _eventStoreConnection.SubscribeToAllAsync(false, EventAppeared, SubscriptionDropped, User);
            }
            catch
            {
                _eventStoreConnection.ConnectAsync().Wait();
                _subscription = await _eventStoreConnection.SubscribeToAllAsync(false, EventAppeared, SubscriptionDropped, User);
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
            var @event = resolvedEvent.Event;
            if (@event == null || !@event.IsJson)
                return Task.FromResult(0);

            var eventName = resolvedEvent.Event.EventStreamId;
            var jsonBytes= resolvedEvent.Event.Data;
            var eventId= resolvedEvent.Event.EventId;

            var events = Autofac.SubscriptionConfiguration.Events;
            if (events.All(q => q.Key != eventName))
                return Task.FromResult(0);

            var eventContext = new EventStoreContext()
            {
                EventId = eventId,
                ResolvedEvent= resolvedEvent,
                EventName = eventName
            };

            _projectorInvoker.Invoke(eventContext);

            return Task.FromResult(0);
        }

    }
}
