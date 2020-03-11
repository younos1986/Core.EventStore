using System;
using System.Threading.Tasks;
using Core.EventStore.Contracts;
using Core.EventStore.EFCore.PostgreSQL.Autofac;
using Core.EventStore.EFCore.PostgreSQL.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.EFCore.PostgreSQL.Implementations
{
    public class IdempotenceReaderService: IIdempotenceReaderService
    {
        private readonly IPostgreSqlConfiguration _mongoConfiguration;
        private readonly EventStorePostgresDbContext _dbContext;
        public IdempotenceReaderService(IPostgreSqlConfiguration mongoConfiguration , EventStorePostgresDbContext  dbContext)
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