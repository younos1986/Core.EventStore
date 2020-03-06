using System.Threading.Tasks;
using Core.EventStore.Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.Mongo.Autofac;
using MongoDB.Driver;

namespace Core.EventStore.Mongo.Implementations
{
    public class PositionWriteService: IPositionWriteService
    {
        private readonly IMongoConfiguration _mongoConfiguration; 
        public PositionWriteService(IMongoConfiguration mongoConfiguration)
        {
            _mongoConfiguration = mongoConfiguration;
        }
        
        public async Task InsertOneAsync(EventStorePosition entity)
        {
            await _mongoConfiguration.GetPositionCollection.InsertOneAsync(entity);
        }
        
    }
}