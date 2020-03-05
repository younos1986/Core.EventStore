using System;
using System.Threading.Tasks;
using Core.EventStore.Autofac;
using MongoDB.Driver;

namespace Core.EventStore.IdempotencyServices
{
    public interface IIdempotencyReaderService
    {
        Task<bool> IsProcessedBefore(Guid streamId);
    }
    
    public class IdempotencyReaderService: IIdempotencyReaderService
    {
        private readonly IMongoConfiguration _mongoConfiguration; 
        public IdempotencyReaderService(IMongoConfiguration mongoConfiguration)
        {
            _mongoConfiguration = mongoConfiguration;
        }
        
        public async Task<bool> IsProcessedBefore(Guid streamId)
        {
            var nameFilter = MongoDB.Driver.Builders<EventStoreIdempotency>.Filter.Eq(x => x.Id, streamId);
            
            var isProcessedBefore = await  _mongoConfiguration.GetIdempotencyCollection
                .Find(nameFilter)
                .AnyAsync();

            return isProcessedBefore;
        }
    }
}