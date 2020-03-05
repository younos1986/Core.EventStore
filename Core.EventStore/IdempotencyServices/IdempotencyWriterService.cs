using System.Threading.Tasks;
using Core.EventStore.Autofac;

namespace Core.EventStore.IdempotencyServices
{
    public interface IIdempotencyWriterService
    {
        Task PersistIdempotencyAsync(EventStoreIdempotency entity);
    }

    public class IdempotencyWriterService: IIdempotencyWriterService
    {
        private readonly IMongoConfiguration _mongoConfiguration; 
        public IdempotencyWriterService(IMongoConfiguration mongoConfiguration)
        {
            _mongoConfiguration = mongoConfiguration;
        }
        
        public async Task PersistIdempotencyAsync(EventStoreIdempotency entity)
        {
            await _mongoConfiguration.GetIdempotencyCollection.InsertOneAsync(entity);
        }
    }
}