using System.Threading.Tasks;
using Core.EventStore.Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.Mongo.Autofac;

namespace Core.EventStore.Mongo.Implementations
{
    public class IdempotenceWriterService: IIdempotenceWriterService
    {
        private readonly IMongoConfiguration _mongoConfiguration; 
        public IdempotenceWriterService(IMongoConfiguration mongoConfiguration)
        {
            _mongoConfiguration = mongoConfiguration;
        }
        
        public async Task PersistIdempotenceAsync(EventStoreIdempotence entity)
        {
            await _mongoConfiguration.GetIdempotenceCollection.InsertOneAsync(entity);
        }
    }
}