using System;
using System.Threading.Tasks;
using Core.EventStore.Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.Mongo.Autofac;
using MongoDB.Driver;

namespace Core.EventStore.Mongo.Implementations
{
    public class IdempotenceReaderService: IIdempotenceReaderService
    {
        private readonly IMongoConfiguration _mongoConfiguration; 
        public IdempotenceReaderService(IMongoConfiguration mongoConfiguration)
        {
            _mongoConfiguration = mongoConfiguration;
        }
        
        public async Task<bool> IsProcessedBefore(Guid streamId)
        {
            var nameFilter = MongoDB.Driver.Builders<EventStoreIdempotence>.Filter.Eq(x => x.Id, streamId);
            
            var isProcessedBefore = await  _mongoConfiguration.GetIdempotenceCollection
                .Find(nameFilter)
                .AnyAsync();

            return isProcessedBefore;
        }
    }
}