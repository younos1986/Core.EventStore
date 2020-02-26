using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventStore.Autofac
{
    public class InitializationConfiguration
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public int DefaultPort { get; set; } = 1113;


        public bool IsDockerized { get; set; }
        public string DockerContainerName { get; set; }


        public string ConnectionUri { get; set; }
    }
}
