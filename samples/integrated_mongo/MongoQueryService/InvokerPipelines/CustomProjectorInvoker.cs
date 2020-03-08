using Core.EventStore.Autofac;
using Core.EventStore.Invokers;
using System;

namespace MongoQueryService.InvokerPipelines
{
    public class CustomProjectorInvoker : ProjectorInvoker
    {
        public override void AfterInvoke(EventStoreContext eventContext)
        {

        }

        public override void BeforeInvoke(EventStoreContext eventContext)
        {
                Console.WriteLine("********************");
                Console.WriteLine(eventContext.EventName);
        }

        
    }
}