using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.EventStore.IntegrationTest.Infrastructures;
using FluentAssertions;
using IntegrationEvents;
using Newtonsoft.Json;
using Xunit; //using Microsoft.AspNetCore.TestHost;

namespace Core.EventStore.IntegrationTest.MySqlTests
{
   
    public class EventStoreMySqlTest :IClassFixture<ModuleFixture>
    {
        private readonly HttpClient _queryApi;
        private readonly HttpClient _commandApi;
        public EventStoreMySqlTest(ModuleFixture fixture)
        {
            _commandApi = new Server().GetClient<MySqlCommandService.Startup>().GetAwaiter().GetResult();
            _queryApi = new Server().GetClient<MySqlQueryService.Startup>().GetAwaiter().GetResult();
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