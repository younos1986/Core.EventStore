using System;
using Core.EventStore.Configurations;
using MongoDB.Driver;

namespace Core.EventStore.Mongo.Autofac
{
    public interface IMongoConfiguration
    {
        IMongoCollection<EventStorePosition> GetPositionCollection { get; }
        IMongoCollection<EventStoreIdempotence> GetIdempotenceCollection { get; }
        

        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string PositionCollectionName { get; set; }
    }

    public class MongoConfiguration : IMongoConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        
        public string PositionCollectionName { get; set; } = "Positions";
        
        public string IdempotenceCollectionName { get; set; } = "Idempotencies";


        public IMongoCollection<EventStorePosition> GetPositionCollection
        {
            get
            {
                var collection = MongoDatabase.GetCollection<EventStorePosition>(PositionCollectionName);
                return collection;
            }
        }
        
        public IMongoCollection<EventStoreIdempotence> GetIdempotenceCollection
        {
            get
            {
                var collection = MongoDatabase.GetCollection<EventStoreIdempotence>(IdempotenceCollectionName);
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