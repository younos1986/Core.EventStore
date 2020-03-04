using Core.EventStore.Autofac;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventStore.Builders
{
    public class EventStoreConnectionBuilder
    {

        public static IEventStoreConnection Build(InitializationConfiguration initializationConfiguration)
        {
            //return EventStoreConnection.Create("connectto=tcp://admin:changeit@eventstore:1113");
            StringBuilder connectionString = new StringBuilder("connectto=tcp://");

            if (!string.IsNullOrWhiteSpace(initializationConfiguration.Username) || string.IsNullOrWhiteSpace(initializationConfiguration.Password))
                connectionString.Append($"{initializationConfiguration.Username}:{initializationConfiguration.Password}@");

            if (initializationConfiguration.IsDockerized)
                connectionString.Append(initializationConfiguration.DockerContainerName);
            else
                connectionString.Append(initializationConfiguration.ConnectionUri);

            connectionString.Append(":" + initializationConfiguration.DefaultPort);
            initializationConfiguration.ReadonlyConnectionString = connectionString.ToString();
            
            var con= EventStoreConnection.Create(initializationConfiguration.ReadonlyConnectionString);
            con.ConnectAsync().Wait();

            return con;
        }
    }
}
