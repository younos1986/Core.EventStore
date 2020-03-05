using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventStore.Dependencies
{
    public  interface IEventStoreReader
    {
        Task PerformAllRegisteredEvents(Action<Guid> actionToNotifyEventIsDone = null);
        Task PerformAll(Action<Guid> actionToNotifyEventIsDone = null);
        
    }
}
