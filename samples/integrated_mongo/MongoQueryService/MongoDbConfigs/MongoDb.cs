using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IntegrationEvents;
using MongoDB.Driver;

namespace MongoQueryService.MongoDbConfigs
{
        public class MongoDb : IMongoDb
        {
            private IMongoCollection<T> GetCollection<T>()
            {
                var _connectionString = "mongodb://127.0.0.1";
                MongoClient dbClient = new MongoClient(_connectionString);

                var database = dbClient.GetDatabase("TestDB");
                var collection = database.GetCollection<T>(typeof(T).Name);
                return collection;
            }
            
            public async Task InsertOneAsync<T>(T entity)
            {
                await GetCollection<T>().InsertOneAsync(entity);
            }
            
            public async Task<T> GetOneAsync<T>(Guid id)
            {
                //var nameFilter = MongoDB.Driver.Builders<CustomerCreated>.Filter.Eq(x => x., id);
            
                var doc = await  GetCollection<T>()
                    .Find(x => true)
                    .FirstOrDefaultAsync();

                return doc;
            }

        }

        public interface IMongoDb
        {
            Task InsertOneAsync<T>(T entity);

            Task<T> GetOneAsync<T>(Guid id);
        }
}
