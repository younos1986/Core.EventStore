using System.Threading.Tasks;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.SqlServer.Autofac;
using Core.EventStore.EFCore.SqlServer.DbContexts;

namespace Core.EventStore.EFCore.SqlServer.Implementations
{
    public class IdempotenceWriterService: IIdempotenceWriterService
    {
        private readonly EventStoreEfCoreDbContext _dbContext;
        public IdempotenceWriterService(EventStoreEfCoreDbContext  dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task PersistIdempotenceAsync(EventStoreIdempotence entity)
        {
            await _dbContext.EventStoreIdempotences.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}