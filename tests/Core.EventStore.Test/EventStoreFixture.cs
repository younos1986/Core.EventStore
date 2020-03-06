using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Core.EventStore.Builders;
using Core.EventStore.Configurations;
using Core.EventStore.Test.Infrastructures;
using Docker.DotNet;
using Docker.DotNet.Models;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using FluentAssertions.Common;
using Xunit;

namespace Core.EventStore.Test
{
    public class EventStoreFixture  : IAsyncLifetime
    {
        public string EventStoreContainer { get; private set; } = $"es-{Guid.NewGuid().ToString("N")}";
        const string EventStoreImage = "eventstore/eventstore";
      
        
        public async Task InitializeAsync()
        {
            //DockerClient client = new DockerClientConfiguration(new Uri("http://localhost:2375")).CreateClient();
            
            Client = new DockerClientConfiguration(
                    new Uri("npipe://./pipe/docker_engine"))
                .CreateClient();
            
            
            
             // I'm running docker on Ubuntu, you may have to connect to Docker Machine on Windows.
            var config = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"));
            this.Client = config.CreateClient();
            var images = await this.Client.Images.ListImagesAsync(new ImagesListParameters { MatchName = EventStoreImage });
            if (images.Count == 0)
            {
                // No image found. Pulling latest ..
                await this.Client.Images.CreateImageAsync(new ImagesCreateParameters { FromImage = EventStoreImage, Tag = "latest" }, null, IgnoreProgress.Forever);
            }
            var containers = await this.Client.Containers.ListContainersAsync(new ContainersListParameters { All = true });
            
            
            
            await this.Client.Containers.CreateContainerAsync(
                new CreateContainerParameters
                { 
                    Image = EventStoreImage, 
                    Name = EventStoreContainer, 
                    Tty = true,
                    HostConfig = new HostConfig
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                        {
                            { 
                                "2113/tcp", 
                                new List<PortBinding> { 
                                    new PortBinding
                                    {
                                        HostPort = "2113"
                                    } 
                                }
                            },
                            { 
                                "1113/tcp", 
                                new List<PortBinding> { 
                                    new PortBinding
                                    {
                                        HostPort = "1113"
                                    } 
                                }
                            }
                        }
                    }
                });
            // Starting the container ...
            await this.Client.Containers.StartContainerAsync(EventStoreContainer, new ContainerStartParameters { });

            var endpoint = new Uri("tcp://127.0.0.1:1113");
            var settings = ConnectionSettings
                .Create()
                .KeepReconnecting()
                .KeepRetrying()
                .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"));
            var connectionName = $"M={Environment.MachineName},P={Process.GetCurrentProcess().Id},T={DateTimeOffset.UtcNow.Ticks}";
            this.Connection = EventStoreConnection.Create(settings, endpoint, connectionName);
            await this.Connection.ConnectAsync();
            
        }

        private DockerClient Client { get; set; }

        public IEventStoreConnection Connection { get; private set; }

        private class IgnoreProgress : IProgress<JSONMessage>
        {
            public static readonly IProgress<JSONMessage> Forever = new IgnoreProgress();

            public void Report(JSONMessage value) { }
        }

        public async Task DisposeAsync()
        {
            if(this.Client != null)
            {
                this.Connection?.Dispose();
                await this.Client.Containers.StopContainerAsync(EventStoreContainer, new ContainerStopParameters { });
                await this.Client.Containers.RemoveContainerAsync(EventStoreContainer, new ContainerRemoveParameters { Force = true });
                this.Client.Dispose();
            }
        }
    }
}