using System;
using Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.EFCore.SqlServer.Autofac;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.EFCore.SqlServer.DbContexts
{
    public class EventStoreEfCoreDbContext: DbContext
    {
        public virtual DbSet<EventStoreIdempotence> EventStoreIdempotences { get; set; }
        
        public virtual DbSet<EventStorePosition> EventStorePositions { get; set; }
        
        
        private readonly IEfCoreConfiguration _efCoreConfiguration;
        public EventStoreEfCoreDbContext(DbContextOptions<EventStoreEfCoreDbContext> options) : base(options)
        {
            //_efCoreConfiguration = container.Resolve<IEfCoreConfiguration>();
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(_efCoreConfiguration.ConnectionString);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventStoreIdempotence>().ToTable("Idempotences");
            modelBuilder.Entity<EventStoreIdempotence>().HasKey(q => q.Id);
            
            modelBuilder.Entity<EventStorePosition>().ToTable("Positions");
            modelBuilder.Entity<EventStorePosition>().HasKey(q => q.Id);

        }
    }
}