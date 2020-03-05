using Core.EventStore.Contracts;
using IntegrationEvents;
using System.Threading.Tasks;
using QueryService.MongoDbConfigs;

namespace QueryService.Projectors
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
