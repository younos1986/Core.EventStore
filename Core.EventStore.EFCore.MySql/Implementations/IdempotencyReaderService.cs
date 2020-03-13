using System;
using System.Threading.Tasks;
using Autofac;
using Core.EventStore.Contracts;
using Core.EventStore.MySql.EFCore.Autofac;
using Core.EventStore.MySql.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.MySql.EFCore.Implementations
{
    public class IdempotenceReaderService: IIdempotenceReaderService
    {
        private readonly EventStoreMySqlDbContext _dbContext;
        public IdempotenceReaderService(ILifetimeScope container)
        {
            _dbContext = container.Resolve<EventStoreMySqlDbContext>();
        }
        
        public async Task<bool> IsProcessedBefore(Guid streamId)
        {
            var isProcessedBefore = await _dbContext.EventStoreIdempotences.AnyAsync(q => q.Id == streamId);
            return isProcessedBefore;
        }
    }
}