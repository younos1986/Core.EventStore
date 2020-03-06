using System.Threading.Tasks;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.PostgreSQL.Autofac;
using Core.EventStore.EFCore.PostgreSQL.DbContexts;

namespace Core.EventStore.EFCore.PostgreSQL.Implementations
{
    public class PositionWriteService: IPositionWriteService
    {
        private readonly IEfCoreConfiguration _mongoConfiguration;
        private readonly EventStoreEfCoreDbContext _dbContext;
        public PositionWriteService(IEfCoreConfiguration mongoConfiguration, EventStoreEfCoreDbContext  dbContext)
        {
            _mongoConfiguration = mongoConfiguration;
            _dbContext = dbContext;
        }
        
        public async Task InsertOneAsync(EventStorePosition entity)
        {
            await _dbContext.EventStorePositions.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}