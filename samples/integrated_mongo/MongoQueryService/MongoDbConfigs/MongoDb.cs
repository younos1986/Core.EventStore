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
            
            public async Task<CustomerCreated> GetOneAsync(Guid id)
            {
                var queryResult = await GetCollection<CustomerCreated>().FindAsync(x => x.Id == id);
                var doc = await queryResult.SingleOrDefaultAsync();

                return doc;
            }

        }

        public interface IMongoDb
        {
            Task InsertOneAsync<T>(T entity);

            Task<CustomerCreated> GetOneAsync(Guid id);
        }
}
