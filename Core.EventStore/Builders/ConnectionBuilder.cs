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
            var connection = new StringBuilder("connectto=tcp://");

            if (!string.IsNullOrWhiteSpace(initializationConfiguration.Username) || string.IsNullOrWhiteSpace(initializationConfiguration.Password))
                connection.Append($"{initializationConfiguration.Username}:{initializationConfiguration.Password}@");

            if (initializationConfiguration.IsDockerized)
                connection.Append(initializationConfiguration.DockerContainerName);
            else
                connection.Append(initializationConfiguration.ConnectionUri);

            connection.Append(":" + initializationConfiguration.DefaultPort.ToString());
            var _con= EventStoreConnection.Create(connection.ToString());

            _con.ConnectAsync().Wait();

            return _con;

        }
    }
}
