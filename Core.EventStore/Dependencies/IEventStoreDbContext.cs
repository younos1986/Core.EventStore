using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventStore.Dependencies
{
    public interface IEventStoreDbContext
    {
         Task AppendToStreamAsync<T>(T command,Guid? eventId=null);
    }
}
