using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Dependencies;
using Core.EventStore.Mongo.Autofac;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using IntegrationEvents;
using Microsoft.AspNetCore.TestHost;
using MongoDB.Driver;
using Newtonsoft.Json;
using Xunit;

namespace Core.EventStore.Test
{
   
    public class EventStoreMongoTest //: IClassFixture<EventStoreFixture> , IClassFixture<MongoDbFixture> 
    {
        private IEventStoreConnection Connection { get; }
        private HttpClient queryApi;
        private HttpClient commandApi;
        public EventStoreMongoTest()//EventStoreFixture eventStoreFixture , MongoDbFixture mongoDbFixture)
        {
            //this.Connection = eventStoreFixture.Connection;
            
            var commandServer = new MongoCommandServiceApi().CreateServer();
            var queryServer = new MongoQueryServiceApi().CreateServer();
            

            commandServer.BaseAddress = new Uri("https://localhost:5000"); 
            commandApi = commandServer.CreateClient();

            queryServer.BaseAddress = new Uri("https://localhost:5001"); 
            queryApi = queryServer.CreateClient();
            
            
        }

        
        
       
        
        [Fact]
        public async Task Show()
        {
            
            var command = new CustomerCreated("Younes" , "Baghaie Moghaddam", DateTime.Now);
            var jsonCustomer = JsonConvert.SerializeObject(command);
            
            // create a customer
            var customerData = new StringContent(jsonCustomer, UTF8Encoding.UTF8, "application/json");
            var addCustomerResponse = await commandApi.PostAsync("api/Customers/CreateCustomer", customerData);
            
            addCustomerResponse.EnsureSuccessStatusCode();
            

            var customerResponse = await queryApi.GetAsync("api/Customers/GetCustomer");
            var customerString = await addCustomerResponse.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<CustomerCreated>(customerString);
            

            // await this.Connection.AppendToStreamAsync("test", ExpectedVersion.Any, new UserCredentials("admin", "changeit"), new EventData[]
            // {
            //     new EventData(Guid.NewGuid(), "MyMessage", true, Encoding.UTF8.GetBytes("{}"), null)
            // });
            //




        }
    }
}