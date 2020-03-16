using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.SqlServer.Autofac;
using Core.EventStore.EFCore.SqlServer.DbContexts;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Core.EventStore.EFCore.SqlServer.Implementations
{
    public class IdempotenceReaderService: IIdempotenceReaderService
    {
        
        private readonly EventStoreEfCoreDbContext _dbContext;
        public IdempotenceReaderService(ILifetimeScope container)
        {
            var _configuration = container.Resolve<IEfCoreConfiguration>();
            _dbContext = container.Resolve<EventStoreEfCoreDbContext>();
        }
        
        public async Task<bool> IsProcessedBefore(Guid streamId)
        {
            var isProcessedBefore = await _dbContext.EventStoreIdempotences.AnyAsync(q => q.Id == streamId);
            return isProcessedBefore;
        }
    }
}