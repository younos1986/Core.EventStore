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
    public class MongoDbFixture  : IAsyncLifetime
    {
        public string MongoContainer { get; private set; } = $"es-{Guid.NewGuid().ToString("N")}";
        const string MongoImage = "mongo";
      
        
        public async Task InitializeAsync()
        {
            //DockerClient client = new DockerClientConfiguration(new Uri("http://localhost:2375")).CreateClient();
            
            Client = new DockerClientConfiguration(
                    new Uri("unix:///var/run/docker.sock"))
                .CreateClient();
            
            // I'm running docker on Ubuntu, you may have to connect to Docker Machine on Windows.
            //var config = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"));
            var config = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock"));
            
            
            // Client = new DockerClientConfiguration(
            //         new Uri("npipe://./pipe/docker_engine"))
            //     .CreateClient();
            //
            //
            //
            //  // I'm running docker on Ubuntu, you may have to connect to Docker Machine on Windows.
            // var config = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"));
            this.Client = config.CreateClient();
            var images = await this.Client.Images.ListImagesAsync(new ImagesListParameters { MatchName = MongoImage });
            if (images.Count == 0)
            {
                // No image found. Pulling latest ..
                await this.Client.Images.CreateImageAsync(new ImagesCreateParameters { FromImage = MongoImage, Tag = "latest" }, null, IgnoreProgress.Forever);
            }
            var containers = await this.Client.Containers.ListContainersAsync(new ContainersListParameters { All = true });
            await this.Client.Containers.CreateContainerAsync(
                new CreateContainerParameters
                { 
                    Image = MongoImage, 
                    Name = MongoContainer, 
                    Tty = true,
                    Env = new List<string>()
                    {
                        "MONGO_INITDB_ROOT_USERNAME=test",
                        "MONGO_INITDB_ROOT_PASSWORD=test"
                    },
                    HostConfig = new HostConfig
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                        {
                            { 
                                "27017/tcp", 
                                new List<PortBinding> { 
                                    new PortBinding
                                    {
                                        HostPort = "27014"
                                    } 
                                }
                            }
                        }
                    }
                });
            // Starting the container ...
            await this.Client.Containers.StartContainerAsync(MongoContainer, new ContainerStartParameters { });
           
        }

        private DockerClient Client { get; set; }

        private class IgnoreProgress : IProgress<JSONMessage>
        {
            public static readonly IProgress<JSONMessage> Forever = new IgnoreProgress();

            public void Report(JSONMessage value) { }
        }

        public async Task DisposeAsync()
        {
            if(this.Client != null)
            {
                await this.Client.Containers.StopContainerAsync(MongoContainer, new ContainerStopParameters { });
                await this.Client.Containers.RemoveContainerAsync(MongoContainer, new ContainerRemoveParameters { Force = true });
                this.Client.Dispose();
            }
        }
    }
}