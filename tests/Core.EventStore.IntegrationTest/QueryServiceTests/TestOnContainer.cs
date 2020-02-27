using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CommandService.Commands;
using Core.EventStore.Autofac;
using Core.EventStore.IntegrationTest.Extensions;
using Core.EventStore.IntegrationTest.Infrastructures;
using IntegrationEvents;
using Newtonsoft.Json;
using QueryService.InvokerPipelines;
using Xunit;

namespace Core.EventStore.IntegrationTest
{
    public class TestOnContainer : QueryServiceServerBase
    {
        [Fact]
        public async Task  Test1()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterEventStore(initializationConfiguration =>
                {
                    initializationConfiguration.Username = "admin";
                    initializationConfiguration.Password = "changeit";
                    initializationConfiguration.DefaultPort = 1113;

                    //initializationConfiguration.IsDockerized = true;
                    //initializationConfiguration.DockerContainerName = "eventstore";

                    initializationConfiguration.IsDockerized = false;
                    initializationConfiguration.ConnectionUri = "127.0.0.1";
                }, new CustomProjectorInvoker())
                .SubscribeRead(subscriptionConfiguration =>
                {
                    subscriptionConfiguration.AddEvent<CustomerCreated>(nameof(CustomerCreated));
                    subscriptionConfiguration.AddEvent<CustomerModified>(nameof(CustomerModified));
                });
            
            
            
            

        }
    }
}