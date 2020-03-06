using System;
using System.Threading.Tasks;

namespace Core.EventStore.Contracts
{
    public interface IIdempotenceReaderService
    {
        Task<bool> IsProcessedBefore(Guid streamId);
    }
}