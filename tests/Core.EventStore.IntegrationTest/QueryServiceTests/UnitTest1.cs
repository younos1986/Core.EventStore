using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommandService.Commands;
using Core.EventStore.IntegrationTest.Extensions;
using Core.EventStore.IntegrationTest.Infrastructures;
using Newtonsoft.Json;
using Xunit;

namespace Core.EventStore.IntegrationTest
{
    public class UnitTest1 : QueryServiceServerBase
    {
        [Fact]
        public async Task  Test1()
        {
            using (var accountServer = new QueryServiceServerBase().CreateServer())
            {

                var accountClient = accountServer.CreateIdempotentClient(new Uri("https://localhost:5000"));


                var createCustomer = new CreateCustomerCommand()
                {
                    FirstName = "Younes",
                    LastName = "Baghaei Moghaddam"
                };
                
                // create a customer
                var customer =  JsonConvert.SerializeObject(createCustomer);
                var customerData = new StringContent(customer , UTF8Encoding.UTF8, "application/json");
                var addCustomerResponse = await accountClient.PostAsync(QueryServiceServerBase.Post.AddCustomer, customerData);

                
                
            }

        }
    }
}