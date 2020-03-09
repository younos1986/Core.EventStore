﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Core.EventStore.Test.DockerFramework.Containers
{
    public class SqlServerContainer : DockerContainer
    {
        public string DatabaseName { get; private set; }
        public string Password { get; private set; }

        public int HostPort { get; private set; }


        public SqlServerContainer(string databaseName, string password, int hostPort)
        {
            DatabaseName = databaseName;
            Password = password;
            HostPort = hostPort;

            Configuration =
                new SqlServerContainerConfiguration(CreateMasterConnectionStringBuilder(), Password, HostPort);
        }

        private SqlConnectionStringBuilder CreateMasterConnectionStringBuilder() =>
            CreateConnectionStringBuilder("master");

        private SqlConnectionStringBuilder CreateConnectionStringBuilder(string database) =>
            new SqlConnectionStringBuilder
            {
                DataSource = $@"localhost,{HostPort}",
                InitialCatalog = database,
                UserID = "sa",
                Password = Password,
                PersistSecurityInfo = true,
                MaxPoolSize = 80,
                //Encrypt = false,
                //Enlist = false,
                //IntegratedSecurity = false
            };

        public async Task<SqlConnectionStringBuilder> CreateDatabaseAsync()
        {
            var database = $"{DatabaseName}";

            var dbText = $@"
IF DB_ID('{database}') IS NULL
   Create database ['{database}']";

            var tableText = $@"
USE [EventStoreDb]
/****** Object:  Table [dbo].[Idempotences]    Script Date: 6-3-2020 10:59:41 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Idempotences]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Idempotences](
	[Id] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Idempotences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

/****** Object:  Table [dbo].[Positions]    Script Date: 6-3-2020 10:59:41 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Positions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Positions](
	[Id] [uniqueidentifier] NOT NULL,
	[CommitPosition] [bigint] NOT NULL,
	[PreparePosition] [bigint] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Positions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
";
            using (var connection = new SqlConnection(CreateMasterConnectionStringBuilder().ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(dbText, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                using (var command = new SqlCommand(tableText, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                connection.Close();
            }

            return CreateConnectionStringBuilder(database);
        }

        private class SqlServerContainerConfiguration : DockerContainerConfiguration
        {
            public SqlServerContainerConfiguration(SqlConnectionStringBuilder builder, string password, int hostPort)
            {
                Image = new ImageSettings
                {
                    Registry = "",
                    Name = "microsoft/mssql-server-linux"
                };

                Container = new ContainerSettings
                {
                    Name = "tail-db",
                    PortBindings = new[]
                    {
                        new PortBinding
                        {
                            HostPort = hostPort,
                            GuestPort = 1433
                        }
                    },
                    EnvironmentVariables = new[]
                    {
                        "ACCEPT_EULA=Y",
                        $"SA_PASSWORD={password}"
                    }
                };

                WaitUntilAvailable = async attempt =>
                {
                    if (attempt <= 30)
                    {
                        try
                        {
                            //await Task.Delay(3000).ConfigureAwait(false);
                            using (var connection = new SqlConnection(builder.ConnectionString))
                            {
                                await connection.OpenAsync();
                                connection.Close();
                            }

                            return TimeSpan.Zero;
                        }
                        catch
                        {
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