using System.Threading.Tasks;
using Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.SqlServer.Autofac;
using Core.EventStore.EFCore.SqlServer.DbContexts;

namespace Core.EventStore.EFCore.SqlServer.Implementations
{
    public class IdempotenceWriterService: IIdempotenceWriterService
    {
        private readonly EventStoreEfCoreDbContext _dbContext;
        public IdempotenceWriterService(ILifetimeScope container)
        {
            var _configuration = container.Resolve<IEfCoreConfiguration>();
            _dbContext = container.Resolve<EventStoreEfCoreDbContext>();
        }
        
        public async Task PersistIdempotenceAsync(EventStoreIdempotence entity)
        {
            await _dbContext.EventStoreIdempotences.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}