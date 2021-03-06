﻿namespace Core.EventStore.MySql.EFCore.Autofac
{
    public interface IMySqlConfiguration
    {
        string ConnectionString { get; set; }
        string PositionTableName { get; set; }
        string IdempotenceTableName { get; set; }
        string DefaultSchema { get; set; }
    }

    public class MySqlConfiguration : IMySqlConfiguration
    {
        public string ConnectionString { get; set; }
        
        public string PositionTableName { get; set; } = "Positions";
        
        public string IdempotenceTableName { get; set; } = "Idempotences";

        public string DefaultSchema { get; set; } = null;


    }
}