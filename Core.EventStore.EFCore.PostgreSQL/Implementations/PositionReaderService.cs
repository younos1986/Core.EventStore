using System;
using System.Linq;
using System.Threading.Tasks;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.PostgreSQL.Autofac;
using Core.EventStore.EFCore.PostgreSQL.DbContexts;
using Core.EventStore.IdGeneration;
using EventStore.ClientAPI;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.EFCore.PostgreSQL.Implementations
{
    public class PositionReaderService: IPositionReaderService
    {
        private readonly IPostgreSqlConfiguration _mongoConfiguration; 
        private readonly EventStorePostgresDbContext _dbContext;
        public PositionReaderService(IPostgreSqlConfiguration mongoConfiguration, EventStorePostgresDbContext  dbContext)
        {
            _mongoConfiguration = mongoConfiguration;
            _dbContext = dbContext;
        }
        
        public async Task<EventStorePosition> GetCurrentPosition()
        {
            var position =await _dbContext.EventStorePositions.OrderByDescending(q => q.CreatedOn).FirstOrDefaultAsync();

            if (position is null)
            {
                var defaultPosition = await CreateDefaultPosition();
                return defaultPosition;
            }

            return position;
        }
        
        private async Task<EventStorePosition> CreateDefaultPosition()
        {
            var defaultPosition =new EventStorePosition()
            {
                Id = CombGuid.Generate(),
                CommitPosition = Position.Start.CommitPosition,
                PreparePosition =Position.Start.PreparePosition, 
                CreatedOn = DateTime.UtcNow,
            }; 
            
            await _dbContext.EventStorePositions.AddAsync(defaultPosition);
            await _dbContext.SaveChangesAsync();

            var createdPosition =await _dbContext.EventStorePositions.FirstOrDefaultAsync();

            return createdPosition;
        }
    }
}