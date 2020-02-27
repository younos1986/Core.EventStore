using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace Core.EventStore.Managers
{
    public class EventStoreConnectionManager: IEventStoreConnectionManager
    {

        private readonly IEventStoreConnection _eventStoreConnection;

        public EventStoreConnectionManager(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

        public async Task Start()
        {
            await _eventStoreConnection.ConnectAsync();
        }

        public void Stop()
        {
            _eventStoreConnection.Close();
        }
        
    }
}