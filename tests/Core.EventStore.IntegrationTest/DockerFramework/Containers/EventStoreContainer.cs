﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace Core.EventStore.IntegrationTest.DockerFramework.Containers
{
    public class EventStoreContainer : DockerContainer
    {
        
        private const int HostPort = 21433;
        private const string Username = "admin";
        private const string Password = "changeit";
        
        public EventStoreContainer()
        {
            Configuration = new EventStoreContainerConfiguration();
        }
        
        private class EventStoreContainerConfiguration : DockerContainerConfiguration
        {
            public EventStoreContainerConfiguration()
            {
                Image = new ImageSettings
                {
                    Registry = "", // "mcr.microsoft.com",
                    Name = "eventstore/eventstore"
                };

                Container = new ContainerSettings
                {
                    Name = "es-integrationtest",
                    PortBindings = new[]
                    {
                        new PortBinding
                        {
                            HostPort = 2113,
                            GuestPort = 2113
                        },
                        new PortBinding
                        {
                            HostPort = 1113,
                            GuestPort = 1113
                        }
                    }
                };

                WaitUntilAvailable = async attempt =>
                {
                    if (attempt <= 30)
                    {
                        try
                        {
                            var endpoint = new Uri("tcp://127.0.0.1:1113");
                            var settings = ConnectionSettings
                                .Create()
                                .KeepReconnecting()
                                .KeepRetrying()
                                .SetDefaultUserCredentials(new UserCredentials(Username, Password));
                            var connectionName = $"M={Environment.MachineName},P={Process.GetCurrentProcess().Id},T={DateTimeOffset.UtcNow.Ticks}";
                            var connection = EventStoreConnection.Create(settings, endpoint, connectionName);
                            await connection.ConnectAsync();
                            connection.Close();
                            return TimeSpan.Zero;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        //await Task.CompletedTask;
                        return TimeSpan.FromSeconds(1);
                    }

                    throw new TimeoutException(
                        $"The container {Container.Name} did not become available in a timely fashion.");
                };
            }
        }
    }
}