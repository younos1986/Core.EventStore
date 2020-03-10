using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;

namespace Core.EventStore.IntegrationTest.DockerFramework.Containers
{
    public class MongoDbContainer : DockerContainer
    {

        public MongoDbContainer()
        {
            Configuration = new MongoDbContainerConfiguration();
        }


        private class MongoDbContainerConfiguration : DockerContainerConfiguration
        {
            public MongoDbContainerConfiguration()
            {
                Image = new ImageSettings
                {
                    Registry = "", // "mcr.microsoft.com",
                    Name = "mongo"
                };

                Container = new ContainerSettings
                {
                    Name = "mongo-integrationtest",
                    PortBindings = new[]
                    {
                        new PortBinding
                        {
                            HostPort = 27014,
                            GuestPort = 27017
                        }
                    },
                    EnvironmentVariables = new List<string>()
                    {
                        "MONGO_INITDB_ROOT_USERNAME=test",
                        "MONGO_INITDB_ROOT_PASSWORD=test"
                    }.ToArray()
                };

                WaitUntilAvailable = async attempt =>
                {
                    if (attempt <= 30)
                    {
                        try
                        {
                            var _connectionString = "mongodb://127.0.0.1";
                            MongoClient dbClient = new MongoClient(_connectionString);

                            return TimeSpan.Zero;
                        }
                        catch
                        {
                        }

                        await Task.CompletedTask;
                        return TimeSpan.FromSeconds(1);
                    }

                    throw new TimeoutException(
                        $"The container {Container.Name} did not become available in a timely fashion.");
                };
            }
        }
    }
}