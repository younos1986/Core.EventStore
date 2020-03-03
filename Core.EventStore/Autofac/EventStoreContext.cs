using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventStore.Autofac
{
    public class EventStoreContext
    {
        public EventStoreContext(Guid eventId,string eventName, ResolvedEvent resolvedEvent, string jsonData, Dictionary<string, object> subscribedEvents )
        {
            EventName = eventName;
            EventId = eventId;
            ResolvedEvent = resolvedEvent;
            JsonData = jsonData;
            SubscribedEvents = subscribedEvents;
        }

        public Guid EventId { get; private  set; }
        public string EventName { get; private set; }
        public ResolvedEvent ResolvedEvent { get; private set; }
        public string JsonData { get; private set; }
        public Dictionary<string, object> SubscribedEvents { get; }
    }
}
