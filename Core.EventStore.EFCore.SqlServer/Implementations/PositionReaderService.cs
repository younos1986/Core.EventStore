using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.SqlServer.Autofac;
using Core.EventStore.EFCore.SqlServer.DbContexts;
using Core.EventStore.IdGeneration;
using EventStore.ClientAPI;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Core.EventStore.EFCore.SqlServer.Implementations
{
    public class PositionReaderService: IPositionReaderService
    {
        private readonly EventStoreEfCoreDbContext _dbContext;
        public PositionReaderService(ILifetimeScope container)
        {
            var _configuration = container.Resolve<IEfCoreConfiguration>();
            _dbContext = container.Resolve<EventStoreEfCoreDbContext>();
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