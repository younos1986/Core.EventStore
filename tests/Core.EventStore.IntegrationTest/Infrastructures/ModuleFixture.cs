using Autofac;
using Core.EventStore.IntegrationTest.DockerFramework.Containers;

namespace Core.EventStore.IntegrationTest.Infrastructures
{
    public class ModuleFixture
    {
        private static IContainer Container { get; set; }
        private static object LockObject { get; } = new object();

        public ModuleFixture()
        {
            lock (LockObject)
            {
                
                if (Container != null)
                    return;
                
                var builder = new ContainerBuilder();

                //PerformPostgreSqlContainer(builder);

                PerformMySqlContainer(builder);

                PerformEventStoreContainer(builder);

                PerformSqlServerContainer(builder);

                PerformMongoDbContainer(builder);

                Container = builder.Build();
            }
        }

        private void PerformMongoDbContainer(ContainerBuilder builder)
        {
            var mongoDbContainer = new MongoDbContainer();
            mongoDbContainer.InitializeAsync().GetAwaiter().GetResult();
            builder.RegisterInstance(mongoDbContainer).As<MongoDbContainer>().IfNotRegistered(typeof(MongoDbContainer)).SingleInstance();
        }

        private void PerformSqlServerContainer(ContainerBuilder builder)
        {
            //configuration.ConnectionString = "Data Source=localhost,1433;Initial Catalog=EventStoreDb;Persist Security Info=True;User ID=sa;Password=TTTttt456!@#;Max Pool Size=80;";
            var sqlContainer = new SqlServerContainer("EventStoreDb", "TTTttt456!@#", 1433);
            sqlContainer.InitializeAsync().GetAwaiter().GetResult();
            sqlContainer.CreateDatabaseAsync().GetAwaiter().GetResult();
            builder.RegisterInstance(sqlContainer).As<SqlServerContainer>().IfNotRegistered(typeof(SqlServerContainer)).SingleInstance();
        }

        private void PerformEventStoreContainer(ContainerBuilder builder)
        {
            var eventStoreContainer = new EventStoreContainer();
            eventStoreContainer.InitializeAsync().GetAwaiter().GetResult();
            builder.RegisterInstance(eventStoreContainer).As<EventStoreContainer>().IfNotRegistered(typeof(EventStoreContainer)).SingleInstance();
        }

        private void PerformMySqlContainer(ContainerBuilder builder)
        {
            //configuration.ConnectionString = "Data Source=localhost,1433;Initial Catalog=EventStoreDb;Persist Security Info=True;User ID=sa;Password=TTTttt456!@#;Max Pool Size=80;";
            var mySqlContainer = new MySqlContainer("localhost", "EventStoreDb", "TTTttt456", 3306);
            mySqlContainer.InitializeAsync().GetAwaiter().GetResult();
            mySqlContainer.CreateDatabaseAsync().GetAwaiter().GetResult();
            builder.RegisterInstance(mySqlContainer).As<MySqlContainer>().IfNotRegistered(typeof(MySqlContainer)).SingleInstance();
        }

        private void PerformPostgreSqlContainer(ContainerBuilder builder)
        {
            //configuration.ConnectionString = "Data Source=localhost,1433;Initial Catalog=EventStoreDb;Persist Security Info=True;User ID=sa;Password=TTTttt456!@#;Max Pool Size=80;";
            var postgreSqlContainer = new PostgreSqlContainer("localhost", "EventStoreDb", "postgres", "TTTttt456", 5432);
            postgreSqlContainer.InitializeAsync().GetAwaiter().GetResult();
            postgreSqlContainer.CreateDatabaseAsync().GetAwaiter().GetResult();
            builder.RegisterInstance(postgreSqlContainer).As<PostgreSqlContainer>().IfNotRegistered(typeof(PostgreSqlContainer)).SingleInstance();
        }
    }
}