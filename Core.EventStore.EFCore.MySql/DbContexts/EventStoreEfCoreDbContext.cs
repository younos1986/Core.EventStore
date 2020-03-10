﻿using Core.EventStore.Configurations;
using Core.EventStore.MySql.EFCore.Autofac;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.MySql.EFCore.DbContexts
{
    public class EventStoreMySqlDbContext: DbContext
    {
        public virtual DbSet<EventStoreIdempotence> EventStoreIdempotences { get; set; }
        
        public virtual DbSet<EventStorePosition> EventStorePositions { get; set; }
        
        
        private readonly IMySqlConfiguration _efCoreConfiguration;
        public EventStoreMySqlDbContext(DbContextOptions<EventStoreMySqlDbContext> options, IMySqlConfiguration efCoreConfiguration) : base(options)
        {
            _efCoreConfiguration = efCoreConfiguration;
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(_efCoreConfiguration.ConnectionString);
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