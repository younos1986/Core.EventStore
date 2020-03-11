using System.Threading.Tasks;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.PostgreSQL.Autofac;
using Core.EventStore.EFCore.PostgreSQL.DbContexts;

namespace Core.EventStore.EFCore.PostgreSQL.Implementations
{
    public class IdempotenceWriterService: IIdempotenceWriterService
    {
        private readonly IPostgreSqlConfiguration _mongoConfiguration;
        private readonly EventStorePostgresDbContext _dbContext;
        public IdempotenceWriterService(IPostgreSqlConfiguration mongoConfiguration, EventStorePostgresDbContext  dbContext)
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