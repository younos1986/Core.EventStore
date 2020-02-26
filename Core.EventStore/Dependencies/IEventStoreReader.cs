using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventStore.Dependencies
{
    public  interface IEventStoreReader
    {
        Task PerformReadStreamEventsForwardAsync(string stream, long start, int count, bool resolveLinkTos = false, Action<Guid> actionToNotifyEventIsDone = null);

        Task PerformAll(Action<Guid> actionToNotifyEventIsDone = null);
    }
}
