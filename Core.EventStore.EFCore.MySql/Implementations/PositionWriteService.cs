using System.Threading.Tasks;
using Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.MySql.EFCore.Autofac;
using Core.EventStore.MySql.EFCore.DbContexts;

namespace Core.EventStore.MySql.EFCore.Implementations
{
    public class PositionWriteService: IPositionWriteService
    {
        private readonly EventStoreMySqlDbContext _dbContext;
        public PositionWriteService(ILifetimeScope container)
        {
            _dbContext = container.Resolve<EventStoreMySqlDbContext>();
        }
        
        public async Task InsertOneAsync(EventStorePosition entity)
        {
            await _dbContext.EventStorePositions.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}