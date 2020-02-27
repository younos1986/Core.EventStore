using Core.EventStore.Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventStore.Invokers
{
    public class EventInvoker : ProjectorInvoker
    {
        public override void AfterInvoke(EventStoreContext eventContext)
        {
        }

        public override void BeforeInvoke(EventStoreContext eventContext)
        {
        }
    }
}
