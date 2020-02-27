using System.Threading.Tasks;

namespace Core.EventStore.Managers
{
    public interface IEventStoreConnectionManager
    {
         Task Start();
         void Stop();
    }
}