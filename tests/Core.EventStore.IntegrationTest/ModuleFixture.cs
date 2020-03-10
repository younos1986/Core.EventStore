using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Core.EventStore.IntegrationTest.DockerFramework.Containers;
using Core.EventStore.Mongo.Autofac;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Core.EventStore.IntegrationTest
{
    public class ModuleFixture
    {
        public static IContainer _Container { get; private set; }

        public IContainer Container
        {
            get { return _Container; }
        }

        private static object _lockObject = new object();


        public ModuleFixture()
        {
            lock (_lockObject)
            {
                
                if (Container != null)
                    return;
                
                var builder = new ContainerBuilder();

                //configuration.ConnectionString = "Data Source=localhost,1433;Initial Catalog=EventStoreDb;Persist Security Info=True;User ID=sa;Password=TTTttt456!@#;Max Pool Size=80;";
                var mySqlContainer = new MySqlContainer("localhost", "EventStoreDb", "TTTttt456", 3306);
                mySqlContainer.InitializeAsync().GetAwaiter().GetResult();
                mySqlContainer.CreateDatabaseAsync().GetAwaiter().GetResult();
                builder.RegisterInstance(mySqlContainer).As<MySqlContainer>().IfNotRegistered(typeof(MySqlContainer))
                    .SingleInstance();

                // var mySqlContainerInstance = builder.Build().Resolve<MySqlContainer>();
                // mySqlContainerInstance.InitializeAsync().GetAwaiter().GetResult();
                // mySqlContainerInstance.CreateDatabaseAsync().GetAwaiter().GetResult();


                var eventStoreContainer = new EventStoreContainer();
                eventStoreContainer.InitializeAsync().GetAwaiter().GetResult();
                builder.RegisterInstance(eventStoreContainer).As<EventStoreContainer>()
                    .IfNotRegistered(typeof(EventStoreContainer)).SingleInstance();

                //configuration.ConnectionString = "Data Source=localhost,1433;Initial Catalog=EventStoreDb;Persist Security Info=True;User ID=sa;Password=TTTttt456!@#;Max Pool Size=80;";
                var sqlContainer = new SqlServerContainer("EventStoreDb", "TTTttt456!@#", 1433);
                sqlContainer.InitializeAsync().GetAwaiter().GetResult();
                sqlContainer.CreateDatabaseAsync().GetAwaiter().GetResult();
                builder.RegisterInstance(sqlContainer).As<SqlServerContainer>()
                    .IfNotRegistered(typeof(SqlServerContainer)).SingleInstance();

                var mongoDbContainer = new MongoDbContainer();
                mongoDbContainer.InitializeAsync().GetAwaiter().GetResult();
                builder.RegisterInstance(mongoDbContainer).As<MongoDbContainer>()
                    .IfNotRegistered(typeof(MongoDbContainer)).SingleInstance();


                _Container = builder.Build();
            }
        }
    }
}