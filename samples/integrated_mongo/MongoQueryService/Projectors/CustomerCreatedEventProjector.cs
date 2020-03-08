using Core.EventStore.Contracts;
using IntegrationEvents;
using System.Threading.Tasks;
using MongoQueryService.MongoDbConfigs;

namespace MongoQueryService.Projectors
{
    public class CustomerCreatedEventProjector : IProjector<CustomerCreated>
    {
        private IMongoDb _mongoDb; 
        public CustomerCreatedEventProjector(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }
        
        public async Task HandleAsync(CustomerCreated integrationEvent)
        {
            await _mongoDb.InsertOneAsync(integrationEvent);
        }
    }
}
