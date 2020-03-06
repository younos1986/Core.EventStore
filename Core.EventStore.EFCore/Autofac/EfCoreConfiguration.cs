using System;
using Core.EventStore.Configurations;
using MongoDB.Driver;

namespace Core.EventStore.EFCore.Autofac
{
    public interface IEfCoreConfiguration
    {
        string ConnectionString { get; set; }
        string PositionTableName { get; set; }
        string IdempotenceTableName { get; set; }
        string DefaultSchema { get; set; }
    }

    public class EfCoreConfiguration : IEfCoreConfiguration
    {
        public string ConnectionString { get; set; }
        
        public string PositionTableName { get; set; } = "Positions";
        
        public string IdempotenceTableName { get; set; } = "Idempotences";

        public string DefaultSchema { get; set; } = "es";

        
    }
}