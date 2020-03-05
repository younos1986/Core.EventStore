using System.Threading.Tasks;
using Core.EventStore.Autofac;
using Core.EventStore.Positions;
using MongoDB.Driver;

namespace Core.EventStore.Services
{
    public interface IPositionWriteService
    {
        Task InsertOneAsync(EventStorePosition entity);
    }

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