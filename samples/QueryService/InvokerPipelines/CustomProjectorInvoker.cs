using Core.EventStore.Autofac;
using Core.EventStore.Invokers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryService.InvokerPipelines
{
    public class CustomProjectorInvoker : ProjectorInvoker
    {
        public override void AfterInvoke(EventContext eventContext)
        {
        }

        public override void BeforeInvoke(EventContext eventContext)
        {
        }
    }
}
