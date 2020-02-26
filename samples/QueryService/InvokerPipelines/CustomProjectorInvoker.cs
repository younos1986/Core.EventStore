using Core.EventStore.Autofac;
using Core.EventStore.Invokers;
using System;

namespace QueryService.InvokerPipelines
{
    public class CustomProjectorInvoker : ProjectorInvoker
    {
        public override void AfterInvoke(EventContext eventContext)
        {

        }

        public override void BeforeInvoke(EventContext eventContext)
        {
                Console.WriteLine("********************");
                Console.WriteLine(eventContext.EventName);
        }
    }
}