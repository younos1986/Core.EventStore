using Core.EventStore.Configurations;
using Core.EventStore.MySql.EFCore.Autofac;
using Core.EventStore.MySql.EFCore.DbContexts;
using IntegrationEvents;
using Microsoft.EntityFrameworkCore;

namespace MySqlQueryService.MySqlConfig
{
    public class MySqlDbContext : DbContext
    {
        public virtual DbSet<CustomerCreatedForMySql> Customers { get; set; }



        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // optionsBuilder.UseMySql(_efCoreConfiguration.ConnectionString);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerCreatedForMySql>().ToTable("Customers");
            modelBuilder.Entity<CustomerCreatedForMySql>().HasKey(q => q.Id);

        }
    }
}