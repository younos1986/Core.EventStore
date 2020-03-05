using System.Threading.Tasks;
using MongoDB.Driver;

namespace QueryService.MongoDbConfigs
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

        }

        public interface IMongoDb
        {
            Task InsertOneAsync<T>(T entity);
        }
}
