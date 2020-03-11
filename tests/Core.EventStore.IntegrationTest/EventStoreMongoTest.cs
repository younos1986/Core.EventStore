using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Dependencies;
using Core.EventStore.IntegrationTest.MongoServers;
using Core.EventStore.Mongo.Autofac;
using Core.EventStore.IntegrationTest.DockerFramework.Containers;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using FluentAssertions;
using IntegrationEvents;
//using Microsoft.AspNetCore.TestHost;
using MongoDB.Driver;
using Newtonsoft.Json;
using Xunit;

namespace Core.EventStore.IntegrationTest
{
   
    public class EventStoreMongoTest  :IClassFixture<ModuleFixture>
    {
      
        private HttpClient _queryApi;
        private HttpClient _commandApi;
        
        
        public EventStoreMongoTest(ModuleFixture fixture)
        {
            // var eventStoreContainer =  fixture.Container.Resolve<EventStoreContainer>();
            // var mongoDbContainer =  fixture.Container.Resolve<MongoDbContainer>();

            _commandApi = new MongoCommandServiceApi().GetClient().GetAwaiter().GetResult();
            _queryApi =  new MongoQueryServiceApi().GetClient().GetAwaiter().GetResult();
        }
       

        [Fact]
        public async Task Raised_Event_Should_Be_In_Mongo()
        {
            
            var command = new CustomerCreated(Guid.NewGuid(),"Younes" , "Baghaie Moghaddam", DateTime.Now);
            var jsonCustomer = JsonConvert.SerializeObject(command);
            
            // create a customer
            var customerData = new StringContent(jsonCustomer, UTF8Encoding.UTF8, "application/json");
            var createCustomerResponse = await _commandApi.PostAsync("api/Customers/CreateCustomer", customerData);
            createCustomerResponse.EnsureSuccessStatusCode();

            var createCustomerString = await createCustomerResponse.Content.ReadAsStringAsync();
            var createdCustomer = JsonConvert.DeserializeObject<CustomerCreated>(createCustomerString);

            var getCustomerResponse = await _queryApi.TryGetAsync($@"api/Customers/GetCustomer?id={ createdCustomer.Id }");
            var getCustomerString = await getCustomerResponse.Content.ReadAsStringAsync();
            var retrievedCustomer = JsonConvert.DeserializeObject<CustomerCreated>(getCustomerString);

            createdCustomer.Id.Should().Be(retrievedCustomer.Id);

        }
    }
}