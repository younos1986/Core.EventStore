using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Core.EventStore.Test
{
    public class MongoCommandServiceApi
    {
        public Microsoft.AspNetCore.TestHost.TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(MongoCommandServiceApi))
                .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json", optional: false)
                        .AddEnvironmentVariables();
                }).UseStartup<MongoCommandService.Startup>();

            Microsoft.AspNetCore.TestHost.TestServer testServer = null;
            try
            {
                testServer = new TestServer(hostBuilder);

            }
            catch (System.Exception ex)
            {

            }




            return testServer;
        }
    }
}