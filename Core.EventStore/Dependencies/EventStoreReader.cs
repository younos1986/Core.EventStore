using Core.EventStore.Autofac;
using Core.EventStore.Invokers;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core.Lifetime;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;

namespace Core.EventStore.Dependencies
{
    public class EventStoreReader : IEventStoreReader
    {
        readonly IEventStoreConnection _eventStoreConnection;
        readonly ProjectorInvoker _projectorInvoker;
        readonly ISubscriptionConfiguration _subscriptionConfiguration;
        private IPositionReaderService _positionReaderService;
        private ILifetimeScope _container;
        public EventStoreReader(IEventStoreConnection eventStoreConnection, ProjectorInvoker projectorInvoker,
            ISubscriptionConfiguration subscriptionConfiguration,
            IPositionReaderService positionReaderService,
            ILifetimeScope container)
        {
            _eventStoreConnection = eventStoreConnection;
            _projectorInvoker = projectorInvoker;
            _subscriptionConfiguration = subscriptionConfiguration;
            _positionReaderService = positionReaderService;
            _container = container;
        }

        public async Task PerformAllRegisteredEvents(Action<Guid> actionToNotifyEventIsDone = null)
        {
            var subscribedEvents = _subscriptionConfiguration.SubscribedEvents;
            int count = 4096;
            bool resolveLinkTos = false;
            foreach (var registeredEvent in subscribedEvents.Keys)
            {
                var currentposition = await _positionReaderService.GetCurrentPosition();
                var retrievedEvents = await _eventStoreConnection.ReadStreamEventsForwardAsync(registeredEvent, currentposition.CommitPosition, count, resolveLinkTos);
                PerformEventHandlerInvoke(actionToNotifyEventIsDone, retrievedEvents.Events);
            }
        }
        
        public async Task PerformAll(Action<Guid> actionToNotifyEventIsDone = null)
        {
            var currentposition = await _positionReaderService.GetCurrentPosition();
            var position = new Position(currentposition.CommitPosition, currentposition.PreparePosition);
            var maxCount = 4096;

            var retrievedEvents = await _eventStoreConnection.ReadAllEventsForwardAsync(position, maxCount, false);
            
            PerformEventHandlerInvoke(actionToNotifyEventIsDone, retrievedEvents.Events);
        }

        private void PerformEventHandlerInvoke(Action<Guid> actionToNotifyEventIsDone, ResolvedEvent[] events)
        {
            foreach (var resolvedEvent in events)
            {
                var eventName = resolvedEvent.Event.EventStreamId;
                var jsonBytes = resolvedEvent.Event.Data;
                var eventId = resolvedEvent.Event.EventId;
                
                var eventContext = new EventStoreContext(eventId, eventName, resolvedEvent, string.Empty, _subscriptionConfiguration.SubscribedEvents, _container);

                _projectorInvoker.Invoke(eventContext);

                if (actionToNotifyEventIsDone != null)
                    actionToNotifyEventIsDone(resolvedEvent.Event.EventId);
            }
        }
    }
}