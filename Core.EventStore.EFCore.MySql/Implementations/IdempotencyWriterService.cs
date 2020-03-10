using System.Threading.Tasks;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.MySql.EFCore.Autofac;
using Core.EventStore.MySql.EFCore.DbContexts;

namespace Core.EventStore.MySql.EFCore.Implementations
{
    public class IdempotenceWriterService: IIdempotenceWriterService
    {
        private readonly IMySqlConfiguration _mongoConfiguration;
        private readonly EventStoreMySqlDbContext _dbContext;
        public IdempotenceWriterService(IMySqlConfiguration mongoConfiguration, EventStoreMySqlDbContext  dbContext)
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