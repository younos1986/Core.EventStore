using System.Threading.Tasks;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.MySql.EFCore.Autofac;
using Core.EventStore.MySql.EFCore.DbContexts;

namespace Core.EventStore.MySql.EFCore.Implementations
{
    public class PositionWriteService: IPositionWriteService
    {
        private readonly IMySqlConfiguration _mongoConfiguration;
        private readonly EventStoreMySqlDbContext _dbContext;
        public PositionWriteService(IMySqlConfiguration mongoConfiguration, EventStoreMySqlDbContext  dbContext)
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