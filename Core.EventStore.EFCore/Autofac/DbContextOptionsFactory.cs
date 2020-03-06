using Core.EventStore.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.EFCore.Autofac
{
    public class DbContextOptionsFactory
    {
        public static DbContextOptions<EventStoreEfCoreDbContext> Get(string stringConnection)
        {
            var builder = new DbContextOptionsBuilder<EventStoreEfCoreDbContext>();
            DbContextConfigurer.Configure(builder, stringConnection);

            return builder.Options;
        }
    }
    
    public class DbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<EventStoreEfCoreDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }
    }
}