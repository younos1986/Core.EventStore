using System;
using System.Threading.Tasks;
using Core.EventStore.Contracts;
using Core.EventStore.MySql.EFCore.Autofac;
using Core.EventStore.MySql.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.MySql.EFCore.Implementations
{
    public class IdempotenceReaderService: IIdempotenceReaderService
    {
        private readonly IMySqlConfiguration _mongoConfiguration;
        private readonly EventStoreMySqlDbContext _dbContext;
        public IdempotenceReaderService(IMySqlConfiguration mongoConfiguration , EventStoreMySqlDbContext  dbContext)
        {
            _mongoConfiguration = mongoConfiguration;
            _dbContext = dbContext;
        }
        
        public async Task<bool> IsProcessedBefore(Guid streamId)
        {
            var isProcessedBefore = await _dbContext.EventStoreIdempotences.AnyAsync(q => q.Id == streamId);
            return isProcessedBefore;
        }
    }
}