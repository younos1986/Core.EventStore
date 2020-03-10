using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Builders;
using Core.EventStore.Configurations;
using Core.EventStore.UnitTest.Infrastructures;
using FluentAssertions;
using IntegrationEvents;
using Xunit;

namespace Core.EventStore.UnitTest.ReaderTests
{
    public class TestInitializedData: GivenWhenThen
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
            
            _builder.RegisterEventStore(initializationConfiguration =>
                {
                    initializationConfiguration.Username = _username;
                    initializationConfiguration.Password = _password;
                    initializationConfiguration.DefaultPort = _defaultPort;

                    initializationConfiguration.IsDockerized = _isDockerized;
                    initializationConfiguration.ConnectionUri = _connectionUri;
                })
                .SubscribeRead(subscriptionConfiguration =>
                {
                    subscriptionConfiguration.AddEvent<CustomerCreated>(nameof(CustomerCreated));
                    subscriptionConfiguration.AddEvent<CustomerModified>(nameof(CustomerModified));
                });

            _container = _builder.Build();
        }

        protected override void When()
        {
           
        }
        
        [Fact]
        public void Then_SubscribeRead_Should_Have_Two_Registered_Events()
        {
            var subscriptionConfiguration = _container.Resolve<ISubscriptionConfiguration>();
            subscriptionConfiguration.SubscribedEvents.Count.Should().Be(2);
        }
        
        [Fact]
        public void Then_InitializationConfiguration_Should_Be_As_Initialized_Ones()
        {
            var initializationConfiguration = _container.Resolve<InitializationConfiguration>();
            initializationConfiguration.Username.Should().Be(_username);
            initializationConfiguration.Password.Should().Be(_password);
            initializationConfiguration.DefaultPort.Should().Be(_defaultPort);
            initializationConfiguration.IsDockerized.Should().Be(_isDockerized);;
            initializationConfiguration.ConnectionUri.Should().Be(_connectionUri);
        }
        
        [Fact]
        public void Then_ConnectionString_Should_Be()
        {
            var initializationConfiguration = _container.Resolve<InitializationConfiguration>();
            EventStoreConnectionBuilder.Build(initializationConfiguration);
            initializationConfiguration.ReadonlyConnectionString.Should().Be("connectto=tcp://admin:changeit@127.0.0.1:1113");
        }
        
    }
}