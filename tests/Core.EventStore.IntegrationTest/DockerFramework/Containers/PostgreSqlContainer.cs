using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Npgsql;

namespace Core.EventStore.IntegrationTest.DockerFramework.Containers
{
    public class PostgreSqlContainer : DockerContainer
    {
        //private const string LocalConnectionString = "server=localhost;uid=root;pwd=Password123;sslmode=none;";

        public string Server { get; private set; }
        public string DatabaseName { get; private set; }
        public string Password { get; private set; }
        public string Username { get; private set; }
        public int HostPort { get; private set; }
        
        readonly string ConnectionString = string.Empty;

        public PostgreSqlContainer(string server, string databaseName, string username, string password, int hostPort)
        {
            Server = server;
            DatabaseName = databaseName;
            Password = password;
            HostPort = hostPort;
            Username = username;
            
            //$"Host={server};Port={hostPort};Database={databaseName};Username={username};password={password}";
            ConnectionString =  $"Host={server};Port={hostPort};Username={username};password={password}";
            
            Configuration = new PostgreSqlContainerConfiguration(ConnectionString,username, password, hostPort);
        }

        public async Task CreateDatabaseAsync()
        {

            var dbText = @"CREATE DATABASE EventStoreDb
            WITH OWNER = postgres
            ENCODING = 'UTF8'
            CONNECTION LIMIT = -1;";
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(dbText, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                connection.Close();
            }
            
            
            var dbtablePosition = @"create table if not exists idempotences
(
    id        char(38) not null,
    createdon date     not null,
    constraint idempotences_pk
        primary key (id)
);";
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(dbtablePosition, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                connection.Close();
            }
            
            
            
            var dbTableIdempotence = @"create table if not exists positions
(
    id              char(38) not null,
    commitposition  bigint,
    prepareposition bigint,
    createdon       date,
    constraint positions_pk
        primary key (id)
);";
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(dbTableIdempotence, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                connection.Close();
            }
        }
 
        private class PostgreSqlContainerConfiguration : DockerContainerConfiguration
        {
           

            public PostgreSqlContainerConfiguration(string connectionString,string username, string password, int hostPort)
            {
                

                Image = new ImageSettings
                {
                    Registry = "",
                    Name = "postgres",
                    //Tag = "11-alpine"
                };

                Container = new ContainerSettings
                {
                    Name = "postgres-integrationtest",
                    PortBindings = new[]
                    {
                        new PortBinding
                        {
                            HostPort = hostPort,
                            GuestPort = 5432
                        }
                    },
                    EnvironmentVariables = new[]
                    {
                        $"POSTGRES_USER={username}",
                        $"POSTGRES_PASSWORD={password}",
                    }
                };

                WaitUntilAvailable = async attempt =>
                {
                    if (attempt <= 30)
                    {
                        try
                        {
                            using (var conn = new NpgsqlConnection(connectionString))
                            {
                                await conn.OpenAsync();
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

/*
 
 DROP DATABASE IF EXISTS EventStoreDb;
CREATE DATABASE EventStoreDb;


create table if not exists idempotences
(
    id        char(38) not null,
    createdon date     not null,
    constraint idempotences_pk
        primary key (id)
);

alter table idempotences
    owner to postgres;

create table if not exists positions
(
    id              char(38) not null,
    commitposition  bigint,
    prepareposition bigint,
    createdon       date,
    constraint positions_pk
        primary key (id)
);

alter table positions
    owner to postgres;

  
 */