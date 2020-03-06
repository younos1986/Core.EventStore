using System.Threading.Tasks;
using Core.EventStore.Configurations;

namespace Core.EventStore.Contracts
{
    public interface IPositionReaderService
    {
        Task<EventStorePosition> GetCurrentPosition();
    }
    
}