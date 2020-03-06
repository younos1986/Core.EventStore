using System;
using System.Linq;
using System.Threading.Tasks;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.IdGeneration;
using Core.EventStore.MySql.EFCore.Autofac;
using Core.EventStore.MySql.EFCore.DbContexts;
using EventStore.ClientAPI;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.MySql.EFCore.Implementations
{
    public class PositionReaderService: IPositionReaderService
    {
        private readonly IEfCoreConfiguration _mongoConfiguration; 
        private readonly EventStoreEfCoreDbContext _dbContext;
        public PositionReaderService(IEfCoreConfiguration mongoConfiguration, EventStoreEfCoreDbContext  dbContext)
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