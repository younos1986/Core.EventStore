using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventStore.Autofac
{
    public class EventStoreContext
    {
        
        public string EventName { get; set; }
        public Guid EventId { get; set; }

        public ResolvedEvent ResolvedEvent { get; set; }

        public string JsonData { get; set; }
    }
}
