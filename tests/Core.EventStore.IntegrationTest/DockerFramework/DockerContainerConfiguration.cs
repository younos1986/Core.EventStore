﻿using System;
using System.Threading.Tasks;

namespace Core.EventStore.IntegrationTest.DockerFramework
{
    public class DockerContainerConfiguration
    {
        public ImageSettings Image { get; set; }

        public ContainerSettings Container { get; set; }

        public Func<int, Task<TimeSpan>> WaitUntilAvailable { get; set; } = attempts => Task.FromResult(TimeSpan.Zero);
    }
}