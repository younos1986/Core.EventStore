using System;
using System.Linq;
using System.Threading.Tasks;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.Autofac;
using Core.EventStore.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Core.EventStore.EFCore.Implementations
{
    public class IdempotenceReaderService: IIdempotenceReaderService
    {
        private readonly IEfCoreConfiguration _mongoConfiguration;
        private readonly EventStoreEfCoreDbContext _dbContext;
        public IdempotenceReaderService(IEfCoreConfiguration mongoConfiguration , EventStoreEfCoreDbContext  dbContext)
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