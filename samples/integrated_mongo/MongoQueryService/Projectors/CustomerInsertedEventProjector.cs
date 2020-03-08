using Core.EventStore.Contracts;
using IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoQueryService.MongoDbConfigs;

namespace MongoQueryService.Projectors
{
    public class CustomerInsertedEventProjector : IProjector<CustomerCreated>
    {
        private IMongoDb _mongoDb; 
        public CustomerInsertedEventProjector(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }
        
        public async Task HandleAsync(CustomerCreated integrationEvent)
        {
            await _mongoDb.InsertOneAsync(integrationEvent);
        }
    }
}
