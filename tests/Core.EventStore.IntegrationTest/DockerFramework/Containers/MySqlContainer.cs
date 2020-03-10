using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Core.EventStore.IntegrationTest.DockerFramework.Containers
{
    public class MySqlContainer : DockerContainer
    {
        //private const string LocalConnectionString = "server=localhost;uid=root;pwd=Password123;sslmode=none;";
        
        public string Server { get; private set; }
        public string DatabaseName { get; private set; }
        public string Password { get; private set; }

        public int HostPort { get; private set; }


        public MySqlContainer(string server,string databaseName, string password, int hostPort)
        {
            Server = server;
            DatabaseName = databaseName;
            Password = password;
            HostPort = hostPort;

            Configuration =
                new MySqlServerContainerConfiguration(CreateMasterConnectionStringBuilder(), Password, HostPort);
        }

        private MySqlConnectionStringBuilder CreateMasterConnectionStringBuilder() =>
            CreateConnectionStringBuilder("master");

        private MySqlConnectionStringBuilder CreateConnectionStringBuilder(string database) =>
            new MySqlConnectionStringBuilder
            {
                Server = $@"{Server}",
                UserID = "root",
                Password = Password,
                SslMode = MySqlSslMode.None,
            };

        public async Task<MySqlConnectionStringBuilder> CreateDatabaseAsync()
        {
            var database = $"{DatabaseName}";
            var dbText = $@"

CREATE DATABASE IF NOT EXISTS EventStoreDb;

use EventStoreDb;

create table if not exists Idempotences
(
    Id        char(38) not null
        primary key,
    CreatedOn datetime not null
);

create table if not exists Positions
(
    Id              char(38) not null
        primary key,
    CommitPosition  bigint   not null,
    PreparePosition bigint   not null,
    CreatedOn       datetime not null
);

";
            using (var connection = new MySqlConnection(CreateMasterConnectionStringBuilder().ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(dbText, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                connection.Close();
            }

            return CreateConnectionStringBuilder(database);
        }

        private class MySqlServerContainerConfiguration : DockerContainerConfiguration
        {
            public MySqlServerContainerConfiguration(MySqlConnectionStringBuilder builder, string password, int hostPort)
            {
                Image = new ImageSettings
                {
                    Registry = "",
                    Name = "mysql",
                    Tag = "5.7"
                };

                Container = new ContainerSettings
                {
                    Name = "mysql-integrationtest",
                    PortBindings = new[]
                    {
                        new PortBinding
                        {
                            HostPort = hostPort,
                            GuestPort = 3306
                        }
                    },
                    EnvironmentVariables = new[]
                    {
                        $"MYSQL_ROOT_PASSWORD={password}",
                    }
                };

                WaitUntilAvailable = async attempt =>
                {
                    if (attempt <= 30)
                    {
                        try
                        {
                            //await Task.Delay(3000).ConfigureAwait(false);
                            using (var connection = new MySqlConnection(builder.ConnectionString))
                            {
                                await connection.OpenAsync();
                                connection.Close();
                            }

                            return TimeSpan.Zero;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        return TimeSpan.FromSeconds(1);
                    }

                    throw new TimeoutException(
                        $"The container {Container.Name} did not become available in a timely fashion.");
                };
            }
        }
    }
}