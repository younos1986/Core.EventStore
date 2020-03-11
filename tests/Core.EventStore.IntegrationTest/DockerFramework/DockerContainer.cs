using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace Core.EventStore.IntegrationTest.DockerFramework
{
   public abstract class DockerContainer
    {
        private const string UnixPipe = "unix:///var/run/docker.sock";
        private const string WindowsPipe = "npipe://./pipe/docker_engine";

        private static readonly Uri DockerUri =
            new Uri(
                Environment.GetEnvironmentVariable("DOCKER_HOST") ??
                (
                    Environment.OSVersion.Platform.Equals(PlatformID.Unix)
                        ? UnixPipe
                        : WindowsPipe
                )
            );

        private static readonly DockerClientConfiguration DockerClientConfiguration =
            new DockerClientConfiguration(DockerUri);

        private readonly DockerClient _client;

        protected DockerContainer()
        {
            _client = DockerClientConfiguration.CreateClient();
        }

        public DockerContainerConfiguration Configuration { get; protected set; }

        public async Task InitializeAsync()
        {
            var found = await TryFindContainer();
            if (found == null)
            {
                await CreateImageIfNotExists();

                var created = await CreateContainer();
                if (created != null)
                {
                    await StartContainer(created);
                }
            }
            else
            {
                //await StartContainer(found);
                
                await StopContainer(found);
                await RemoveContainer(found);
                
                var created = await CreateContainer();
                if (created != null)
                {
                    await StartContainer(created);
                }
            }
        }

        private async Task<string> TryFindContainer()
        {
            IList<ContainerListResponse> containers = null;
            try
            {
                // containers = await _client.Containers.ListContainersAsync(
                //     new ContainersListParameters
                //     {
                //         All = true,
                //         Filters = new Dictionary<string, IDictionary<string, bool>>
                //         {
                //             ["name"] = new Dictionary<string, bool>
                //             {
                //                 [Configuration.Container.Name] = true
                //             }
                //         }
                //     }
                //     ).ConfigureAwait(false);
                //
                containers = await _client.Containers.ListContainersAsync(new ContainersListParameters
                {
                    All = true
                    }
                 ).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var formattedName = "/" + Configuration.Container.Name;
            
            var id = containers
                ?.FirstOrDefault(container => container.Names.Contains(formattedName))?.ID;
            
            return id;
            //return containers.FirstOrDefault(container => container.State != "exited")?.ID;
        }

        private async Task<string> CreateContainer()
        {
            var container = await _client
                .Containers
                .CreateContainerAsync(new CreateContainerParameters
                {
                    Image = Configuration.Image.FullyQualifiedName,
                    Name = Configuration.Container.Name,
                    Tty = true,
                    Env = Configuration.Container.EnvironmentVariables,
                    HostConfig = new HostConfig
                    {
                        PortBindings = Configuration.Container.PortBindings.ToDictionary(
                            binding => $"{binding.GuestPort}/tcp",
                            binding => (IList<Docker.DotNet.Models.PortBinding>)new List<Docker.DotNet.Models.PortBinding>
                            {
                                new Docker.DotNet.Models.PortBinding
                                {
                                    HostPort = binding.HostPort.ToString(CultureInfo.InvariantCulture)
                                }
                            })
                    }
                })
                .ConfigureAwait(false);

            return container.ID;
        }

        private async Task StartContainer(string id)
        {
            var started = await _client
                .Containers
                .StartContainerAsync(id, new ContainerStartParameters())
                .ConfigureAwait(false);

            if (started)
            {
                var attempt = 0;
                var result = await Configuration.WaitUntilAvailable(attempt++);
                while (result > TimeSpan.Zero)
                {
                    await Task.Delay(result);
                    result = await Configuration.WaitUntilAvailable(attempt++);
                }
            }

            if (Configuration.PreTestIfAvailable != null)
            {
                var attempt = 0;
                var result = await Configuration.PreTestIfAvailable(attempt++);
                while (result > TimeSpan.Zero)
                {
                    await Task.Delay(result);
                    result = await Configuration.PreTestIfAvailable(attempt++);
                }
            }
            
        }

        private async Task StopContainer(string id)
        {
            await _client
                .Containers
                .StopContainerAsync(id, new ContainerStopParameters { WaitBeforeKillSeconds = 10 })
                .ConfigureAwait(false);
        }

        private async Task RemoveContainer(string id, bool force= true)
        {
            await _client
                .Containers
                .RemoveContainerAsync(id, new ContainerRemoveParameters { Force = force })
                .ConfigureAwait(false);
        }

        private async Task CreateImageIfNotExists()
        {
            if (!await ImageExists().ConfigureAwait(false))
            {
                await _client
                    .Images
                    .CreateImageAsync(
                        new ImagesCreateParameters
                        {
                            FromImage = Configuration.Image.RegistryQualifiedName,
                            Tag = Configuration.Image.Tag
                        },
                        null,
                        Progress.IsBeingIgnored)
                    .ConfigureAwait(false);
            }
        }

        private async Task<bool> ImageExists()
        {
            var images = await _client
                .Images
                .ListImagesAsync(new ImagesListParameters { MatchName = Configuration.Image.RegistryQualifiedName })
                .ConfigureAwait(false);
            return images.Count != 0;
        }

        public async Task DisposeAsync()
        {
            if (Configuration.Container.StopContainer)
            {
                var found = await TryFindContainer();
                if (found != null)
                {
                    await StopContainer(found);
                    if (Configuration.Container.RemoveContainer)
                    {
                        await RemoveContainer(found);
                    }
                }
            }
            _client.Dispose();
        }

        private class Progress : IProgress<JSONMessage>
        {
            public static readonly IProgress<JSONMessage> IsBeingIgnored = new Progress();

            public void Report(JSONMessage value)
            {
            }
        }
    }
}