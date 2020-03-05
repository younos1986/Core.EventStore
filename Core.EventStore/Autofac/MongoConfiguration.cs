using System;
using Core.EventStore.IdempotencyServices;
using Core.EventStore.Positions;
using Core.EventStore.Services;
using MongoDB.Driver;

namespace Core.EventStore.Autofac
{
    public interface IMongoConfiguration
    {
        IMongoCollection<EventStorePosition> GetPositionCollection { get; }
        IMongoCollection<EventStoreIdempotency> GetIdempotencyCollection { get; }
        

        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string PositionCollectionName { get; set; }
    }

    public class MongoConfiguration : IMongoConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        
        public string PositionCollectionName { get; set; } = "Positions";
        
        public string IdempotencyCollectionName { get; set; } = "Idempotencies";


        public IMongoCollection<EventStorePosition> GetPositionCollection
        {
            get
            {
                var collection = MongoDatabase.GetCollection<EventStorePosition>(PositionCollectionName);
                return collection;
            }
        }
        
        public IMongoCollection<EventStoreIdempotency> GetIdempotencyCollection
        {
            get
            {
                var collection = MongoDatabase.GetCollection<EventStoreIdempotency>(IdempotencyCollectionName);
                return collection;
            }
        }

        private IMongoDatabase MongoDatabase
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConnectionString))
                    throw new Exception("At least in KeepIdempotencyInMongo or KeepPositionInMongo you should provide MongoConfiguration");
                
                MongoClient dbClient = new MongoClient(ConnectionString);
                var database = dbClient.GetDatabase(DatabaseName);
                return database;
            }
        }
    }
}