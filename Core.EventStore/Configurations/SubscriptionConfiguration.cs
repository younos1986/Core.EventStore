﻿using System.Collections.Generic;

namespace Core.EventStore.Configurations
{

    public interface ISubscriptionConfiguration
    {
        Dictionary<string, object> SubscribedEvents { get;  }
        void AddEvent<T>(string aliasName = null);
    }

    public class SubscriptionConfiguration : ISubscriptionConfiguration
    {
        //public static Dictionary<string, object> SubscribedEvents { get; private set; }
        public Dictionary<string, object> SubscribedEvents { get; private set; }
        public  SubscriptionConfiguration()
        {
            if (SubscribedEvents == null)
                SubscribedEvents = new Dictionary<string, object>();
        }

        public void AddEvent<T>(string aliasName = null)
        {
            if (string.IsNullOrWhiteSpace(aliasName))
                SubscribedEvents.Add(typeof(T).Name, typeof(T));
            else
                SubscribedEvents.Add(aliasName, typeof(T));
        }
    }
}
