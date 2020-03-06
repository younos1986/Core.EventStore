using Core.EventStore.MySql.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.MySql.EFCore.Autofac
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
            builder.UseMySQL(connectionString);
        }
    }
}