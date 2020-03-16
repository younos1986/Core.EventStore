using System.Threading.Tasks;
using Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.SqlServer.Autofac;
using Core.EventStore.EFCore.SqlServer.DbContexts;

namespace Core.EventStore.EFCore.SqlServer.Implementations
{
    public class PositionWriteService: IPositionWriteService
    {
        private readonly EventStoreEfCoreDbContext _dbContext;
        public PositionWriteService(ILifetimeScope container)
        {
            var _configuration = container.Resolve<IEfCoreConfiguration>();
            _dbContext = container.Resolve<EventStoreEfCoreDbContext>();
        }
        
        public async Task InsertOneAsync(EventStorePosition entity)
        {
            await _dbContext.EventStorePositions.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}