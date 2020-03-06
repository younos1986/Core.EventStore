using System;
using System.Collections.Generic;
using Autofac;
using EventStore.ClientAPI;

namespace Core.EventStore.Autofac
{
    public class EventStoreContext
    {
        public EventStoreContext(Guid eventId,string eventName, ResolvedEvent resolvedEvent, string jsonData, Dictionary<string, object> subscribedEvents, ILifetimeScope  container = null )
        {
            EventName = eventName;
            EventId = eventId;
            ResolvedEvent = resolvedEvent;
            JsonData = jsonData;
            SubscribedEvents = subscribedEvents;
            Container = container;
        }

        public Guid EventId { get; private  set; }
        public string EventName { get; private set; }
        public ResolvedEvent ResolvedEvent { get; private set; }
        public string JsonData { get; private set; }
        public Dictionary<string, object> SubscribedEvents { get; }
        public ILifetimeScope Container { get; }
        
    }
}
