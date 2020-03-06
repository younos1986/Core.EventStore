using System.Threading.Tasks;
using Core.EventStore.Configurations;

namespace Core.EventStore.Contracts
{
    public interface IPositionWriteService
    {
        Task InsertOneAsync(EventStorePosition entity);
    }
}