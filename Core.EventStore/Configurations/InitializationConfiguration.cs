

// [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Core.EventStore.IntegrationTest")]

namespace Core.EventStore.Configurations
{
    public class InitializationConfiguration
    {
        public string EventStoreConnectionString { get; set; }
        
        // public string Username { get; set; }
        // public string Password { get; set; }
        // public int DefaultPort { get; set; } = 1113;
        // public bool IsDockerized { get; set; }
        // public string DockerContainerName { get; set; }
        // public string ConnectionUri { get; set; }
        // public string ReadonlyConnectionString { get; set; }
    }
}
