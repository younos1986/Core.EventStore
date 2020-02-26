﻿using Core.EventStore.Autofac;
using Core.EventStore.Invokers;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventStore.Dependencies
{
    public class EventStoreReader : IEventStoreReader
    {

        readonly IEventStoreConnection _eventStoreConnection;
        readonly ProjectorInvoker _projectorInvoker;
        public EventStoreReader(IEventStoreConnection eventStoreConnection, ProjectorInvoker projectorInvoker)
        {
            _eventStoreConnection = eventStoreConnection;
            _projectorInvoker = projectorInvoker;
        }

        public Task PerformReadStreamEventsForwardAsync(string stream, long start, int count, bool resolveLinkTos = false, Action<Guid> actionToNotifyEventIsDone = null)
        {
            var events = _eventStoreConnection.ReadStreamEventsForwardAsync(stream, start, count, resolveLinkTos).GetAwaiter().GetResult();
            foreach (var resolvedEvent in events.Events)
            {
                Encoding.UTF8.GetString(resolvedEvent.Event.Data);

                var eventName = resolvedEvent.Event.EventStreamId;
                var jsonBytes = resolvedEvent.Event.Data;
                var eventId = resolvedEvent.Event.EventId;

                var eventContext = new EventContext()
                {
                    EventId = eventId,
                    ResolvedEvent = resolvedEvent,
                    EventName = eventName
                };

                _projectorInvoker.Invoke(eventContext);

                if (actionToNotifyEventIsDone != null)
                    actionToNotifyEventIsDone(resolvedEvent.Event.EventId);
            }

            return Task.FromResult(0);
        }

        public Task PerformAll(Action<Guid> actionToNotifyEventIsDone = null)
        {
            var events = _eventStoreConnection.ReadStreamEventsForwardAsync("", 0, int.MaxValue, false).GetAwaiter().GetResult();
            foreach (var resolvedEvent in events.Events)
            {
                Encoding.UTF8.GetString(resolvedEvent.Event.Data);

                var eventName = resolvedEvent.Event.EventStreamId;
                var jsonBytes = resolvedEvent.Event.Data;
                var eventId = resolvedEvent.Event.EventId;

                var eventContext = new EventContext()
                {
                    EventId = eventId,
                    ResolvedEvent = resolvedEvent,
                    EventName = eventName
                };

                _projectorInvoker.Invoke(eventContext);

                if (actionToNotifyEventIsDone != null)
                    actionToNotifyEventIsDone(resolvedEvent.Event.EventId);
            }

            return Task.FromResult(0);
        }

    }
}