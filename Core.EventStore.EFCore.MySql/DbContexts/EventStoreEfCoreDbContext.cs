using Core.EventStore.Configurations;
using Core.EventStore.MySql.EFCore.Autofac;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.MySql.EFCore.DbContexts
{
    public class EventStoreMySqlDbContext: DbContext
    {
        public virtual DbSet<EventStoreIdempotence> EventStoreIdempotences { get; set; }
        
        public virtual DbSet<EventStorePosition> EventStorePositions { get; set; }
        
        
        private readonly IMySqlConfiguration _efCoreConfiguration;
        public EventStoreMySqlDbContext(DbContextOptions<EventStoreMySqlDbContext> options) : base(options)
        {
            //_efCoreConfiguration = efCoreConfiguration;
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseMySql(_efCoreConfiguration.ConnectionString);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventStoreIdempotence>().ToTable(_efCoreConfiguration.IdempotenceTableName);
            modelBuilder.Entity<EventStoreIdempotence>().HasKey(q => q.Id);
            
            modelBuilder.Entity<EventStorePosition>().ToTable(_efCoreConfiguration.PositionTableName);
            modelBuilder.Entity<EventStorePosition>().HasKey(q => q.Id);

        }
    }
}