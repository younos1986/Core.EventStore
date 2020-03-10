using Core.EventStore.Configurations;
using Core.EventStore.MySql.EFCore.Autofac;
using Core.EventStore.MySql.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.MySql.EFCore.Autofac
{
    public class DbContextOptionsFactory
    {
        public static DbContextOptions<EventStoreMySqlDbContext> Get(string stringConnection)
        {
            var builder = new DbContextOptionsBuilder<EventStoreMySqlDbContext>();
            DbContextConfigurer.Configure(builder, stringConnection);
    
            return builder.Options;
        }
    }
    
    public class DbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<EventStoreMySqlDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }
    }
}