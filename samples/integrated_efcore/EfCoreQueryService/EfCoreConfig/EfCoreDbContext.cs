using Core.EventStore.Configurations;
using IntegrationEvents;
using Microsoft.EntityFrameworkCore;

namespace EfCoreQueryService.EfCoreConfig
{
    public class EfCoreDbContext : DbContext
    {
        public virtual DbSet<CustomerCreatedForEfCore> Customers { get; set; }



        public EfCoreDbContext(DbContextOptions<EfCoreDbContext> options) : base(options)
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

            modelBuilder.Entity<CustomerCreatedForEfCore>().ToTable("Customers");
            modelBuilder.Entity<CustomerCreatedForEfCore>().HasKey(q => q.Id);

        }
    }
}