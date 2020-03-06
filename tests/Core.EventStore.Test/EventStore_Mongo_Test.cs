using System;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Xunit;

namespace Core.EventStore.Test
{
    
    //[Collection(nameof(EventStoreCollection))]
    [Collection(nameof(MongoDbCollection))]
    public class EventStore_Mongo_Test
    {
        //private IEventStoreConnection Connection { get; }

        public EventStore_Mongo_Test(MongoDbFixture mongoDbFixture)
        {
            //this.Connection = fixture.Connection;
        }

        [Fact]
        public async Task Show()
        {
            // await this.Connection.AppendToStreamAsync("test", ExpectedVersion.Any, new UserCredentials("admin", "changeit"), new EventData[]
            // {
            //     new EventData(Guid.NewGuid(), "MyMessage", true, Encoding.UTF8.GetBytes("{}"), null)
            // });
            //
            
            
            
            
        }
    }
}