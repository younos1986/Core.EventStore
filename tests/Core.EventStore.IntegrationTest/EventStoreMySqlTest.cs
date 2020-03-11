using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Dependencies;
using Core.EventStore.IntegrationTest.EfCoreSqlServers;
using Core.EventStore.Mongo.Autofac;
using Core.EventStore.IntegrationTest.DockerFramework.Containers;
using Core.EventStore.IntegrationTest.MongoServers;
using Core.EventStore.IntegrationTest.MySqlServers;
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
   
    public class EventStoreMySqlTest :IClassFixture<ModuleFixture>
    {
        private readonly HttpClient _queryApi;
        private readonly HttpClient _commandApi;
        public EventStoreMySqlTest(ModuleFixture fixture)
        {
            // var eventStoreContainer =  fixture.Container.Resolve<EventStoreContainer>();
            // var sqlContainer =  fixture.Container.Resolve<SqlServerContainer>();

            _commandApi = new MySqlCommandServiceApi().GetClient().GetAwaiter().GetResult();
            _queryApi =  new MySqlQueryServiceApi().GetClient().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Raised_Event_Should_Be_In_MySql()
        {
            var command = new CustomerCreatedForMySql(Guid.NewGuid(),"Younes" , "Baghaie Moghaddam", DateTime.Now);
            var jsonCustomer = JsonConvert.SerializeObject(command);
            
            // create a customer
            var customerData = new StringContent(jsonCustomer, UTF8Encoding.UTF8, "application/json");
            var createCustomerResponse = await _commandApi.PostAsync("api/Customers/CreateCustomer", customerData);
            createCustomerResponse.EnsureSuccessStatusCode();

            var createCustomerString = await createCustomerResponse.Content.ReadAsStringAsync();
            var createdCustomer = JsonConvert.DeserializeObject<CustomerCreatedForMySql>(createCustomerString);

            var getCustomerResponse = await _queryApi.TryGetAsync($@"api/Customers/GetCustomer?id={ createdCustomer.Id }");
            var getCustomerString = await getCustomerResponse.Content.ReadAsStringAsync();
            var retrievedCustomer = JsonConvert.DeserializeObject<CustomerCreatedForMySql>(getCustomerString);

            createdCustomer.Id.Should().Be(retrievedCustomer.Id);
        }
    }
}