using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.EventStore.Test.MongoServers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Core.EventStore.Test.EfCoreSqlServers
{
    public class EfCoreSqlQueryServiceApi
    {
        
        public async Task<HttpClient> GetClient()
        {
            var path = Assembly.GetAssembly(typeof(MongoCommandServiceApi))
                .Location;

            
            IHostBuilder hostBuilder =
                Host.CreateDefaultBuilder()
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //<--NOTE THIS
                    .ConfigureContainer<ContainerBuilder>(builder => { })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        // Add TestServer
                        webBuilder.UseTestServer();
                        webBuilder.UseStartup<EfCoreQueryService.Startup>();
                        webBuilder.ConfigureAppConfiguration(cb =>
                        {
                            cb.AddJsonFile("appsettings.json", optional: false)
                                .AddEnvironmentVariables();
                        });
                    });

            // Build and start the IHost
            var host = await hostBuilder.StartAsync();

            // Create an HttpClient to send requests to the TestServer
            var client = host.GetTestClient();

            return client;

        }
        
       
    }
}