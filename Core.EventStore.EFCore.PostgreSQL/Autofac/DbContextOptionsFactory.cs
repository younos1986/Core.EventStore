using Core.EventStore.EFCore.PostgreSQL.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.EFCore.PostgreSQL.Autofac
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
            builder.UseNpgsql(connectionString);
        }
    }
}