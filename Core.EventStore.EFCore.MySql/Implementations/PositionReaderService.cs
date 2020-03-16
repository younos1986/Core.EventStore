using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
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
        private readonly EventStoreMySqlDbContext _dbContext;
        public PositionReaderService(ILifetimeScope container)
        {
            var _configuration = container.Resolve<IMySqlConfiguration>();
            _dbContext = container.Resolve<EventStoreMySqlDbContext>();
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