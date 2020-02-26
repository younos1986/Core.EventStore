using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventStore.Autofac
{
    public class SubscriptionConfiguration
    {
        public static Dictionary<string, object> Events { get; set; }

        public SubscriptionConfiguration()
        {
            Events = new Dictionary<string, object>();
        }

        public void AddEvent<T>(string aliasName = null)
        {
            if (string.IsNullOrWhiteSpace(aliasName))
                Events.Add(typeof(T).Name, typeof(T));
            else
                Events.Add(aliasName, typeof(T));
        }

    }
}
