using Core.EventStore.Configurations;
using Core.EventStore.EFCore.PostgreSQL.Autofac;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.EFCore.PostgreSQL.DbContexts
{
    public class EventStorePostgresDbContext: DbContext
    {
        public virtual DbSet<EventStoreIdempotence> EventStoreIdempotences { get; set; }
        
        public virtual DbSet<EventStorePosition> EventStorePositions { get; set; }
        
        
        private readonly IPostgreSqlConfiguration _efCoreConfiguration;
        public EventStorePostgresDbContext(DbContextOptions<EventStorePostgresDbContext> options
            , IPostgreSqlConfiguration efCoreConfiguration
            ) : base(options)
        {
            _efCoreConfiguration = efCoreConfiguration;
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Host=my_host;Database=my_db;Username=my_user;Password=my_pw
                //optionsBuilder.UseNpgsql(_efCoreConfiguration.ConnectionString);
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