using System.Threading.Tasks;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.PostgreSQL.Autofac;
using Core.EventStore.EFCore.PostgreSQL.DbContexts;

namespace Core.EventStore.EFCore.PostgreSQL.Implementations
{
    public class IdempotenceWriterService: IIdempotenceWriterService
    {
        private readonly IEfCoreConfiguration _mongoConfiguration;
        private readonly EventStoreEfCoreDbContext _dbContext;
        public IdempotenceWriterService(IEfCoreConfiguration mongoConfiguration, EventStoreEfCoreDbContext  dbContext)
        {
            _mongoConfiguration = mongoConfiguration;
            _dbContext = dbContext;
        }
        
        public async Task PersistIdempotenceAsync(EventStoreIdempotence entity)
        {
            await _dbContext.EventStoreIdempotences.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}