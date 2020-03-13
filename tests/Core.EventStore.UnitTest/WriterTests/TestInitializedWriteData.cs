using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.UnitTest.Infrastructures;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Core.EventStore.UnitTest.WriterTests
{
    public class TestInitializedWriteData: GivenWhenThen
    {
        private ContainerBuilder _builder;
        private IContainer _container;
        private readonly string _username = "admin";
        private readonly string _password = "changeit";
        private readonly int _defaultPort = 1113;
        private readonly bool _isDockerized = false;
        private readonly string _connectionUri = "127.0.0.1";
        protected override void Given()
        {
            _builder = new ContainerBuilder();

            // _builder.RegisterEventStore(initializationConfiguration =>
            // {
            //
            //     var configuration = initializationConfiguration.Resolve<IConfiguration>();
            //     var connectionUri = configuration.GetValue<string>("EventStore:ConnectionString");
            //
            //     var init = new InitializationConfiguration()
            //     {
            //         Username = _username,
            //         Password = _password,
            //         DefaultPort = _defaultPort,
            //
            //         //IsDockerized = true;
            //         //DockerContainerName = "eventstore";
            //
            //         IsDockerized = _isDockerized,
            //         ConnectionUri = _connectionUri
            //     };
            //
            //     return init;
            // });

            _container = _builder.Build();
        }

        protected override void When()
        {
           
        }
        
        [Fact]
        public void Then_InitializationConfiguration_Should_Be_The_Same_AS_Initialized_Ones()
        {
            // var initializationConfiguration = _container.Resolve<InitializationConfiguration>();
            // initializationConfiguration.Username.Should().Be(_username);
            // initializationConfiguration.Password.Should().Be(_password);
            // initializationConfiguration.DefaultPort.Should().Be(_defaultPort);
            // initializationConfiguration.IsDockerized.Should().Be(_isDockerized);;
            // initializationConfiguration.ConnectionUri.Should().Be(_connectionUri);
        }
        
        
    }
}