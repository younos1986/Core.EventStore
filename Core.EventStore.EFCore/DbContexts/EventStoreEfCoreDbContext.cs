using System;
using Core.EventStore.Configurations;
using Core.EventStore.EFCore.Autofac;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.EFCore.DbContexts
{
    public class EventStoreEfCoreDbContext: DbContext
    {
        public virtual DbSet<EventStoreIdempotence> EventStoreIdempotences { get; set; }
        
        public virtual DbSet<EventStorePosition> EventStorePositions { get; set; }
        
        
        private readonly IEfCoreConfiguration _efCoreConfiguration;
        public EventStoreEfCoreDbContext(DbContextOptions<EventStoreEfCoreDbContext> options, IEfCoreConfiguration efCoreConfiguration) : base(options)
        {
            _efCoreConfiguration = efCoreConfiguration;
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_efCoreConfiguration.ConnectionString);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventStoreIdempotence>().ToTable(_efCoreConfiguration.IdempotenceTableName , _efCoreConfiguration.DefaultSchema);
            modelBuilder.Entity<EventStoreIdempotence>().HasKey(q => q.Id);
            
            modelBuilder.Entity<EventStorePosition>().ToTable(_efCoreConfiguration.PositionTableName,_efCoreConfiguration.DefaultSchema);
            modelBuilder.Entity<EventStorePosition>().HasKey(q => q.Id);

        }
    }
}