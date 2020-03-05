# Core.EventStore

![.NET Core](https://github.com/younos1986/Core.EventStore/workflows/.NET%20Core/badge.svg)
 

A library to facilitate communication between CommandService and QueryService. The Idea is when any event occures in commandService, it should be persisted in QueryService in MongoDb


<img src="https://raw.githubusercontent.com/younos1986/Core.EventStore/master/images/what_it_does.png" />


- CommandService only keeps events in EventStore
- QueryService's Projectors will be triggered when any event is stored in EventStore by CommandService


# Nuget

<a href="https://www.nuget.org/packages/Core.EventStore/"> Core.EventStore Nuget Package <a/>


# Features

* Keep track of last event position
* Idempotency
* Define multiple projectors for one event


# Dependencies

* Autofac Version="5.1.2"
* EventStore.Client Version="5.0.6"
* MongoDB.Driver Version="2.10.0"

# How to use 

In CommandService

```
using Autofac;
using Core.EventStore.Autofac;

namespace CommandService.IoCC.Modules
{
    public class EventStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterEventStore(initializationConfiguration =>
            {
                initializationConfiguration.Username = "admin";
                initializationConfiguration.Password = "changeit";
                initializationConfiguration.DefaultPort = 1113;
                //initializationConfiguration.IsDockerized = true;
                //initializationConfiguration.DockerContainerName = "eventstore";

                initializationConfiguration.IsDockerized = false;
                initializationConfiguration.ConnectionUri = "127.0.0.1";
            });
        }
    }
}


```

In QueryService

```
using Autofac;
using Core.EventStore.Autofac;
using IntegrationEvents;
using QueryService.InvokerPipelines;

namespace QueryService.IoCC.Modules
{
        public class EventStoreModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterEventStore(initializationConfiguration =>
                {
                    initializationConfiguration.Username = "admin";
                    initializationConfiguration.Password = "changeit";
                    initializationConfiguration.DefaultPort = 1113;

                    //initializationConfiguration.IsDockerized = true;
                    //initializationConfiguration.DockerContainerName = "eventstore";

                    initializationConfiguration.IsDockerized = false;
                    initializationConfiguration.ConnectionUri = "127.0.0.1";
                })
                    .SubscribeRead(subscriptionConfiguration =>
                    {
                        subscriptionConfiguration.AddEvent<CustomerCreated>(nameof(CustomerCreated));
                        subscriptionConfiguration.AddEvent<CustomerModified>(nameof(CustomerModified));
                    }, new CustomProjectorInvoker())
                    .KeepPositionInMongo(configuration =>
                        {
                            configuration.ConnectionString = "mongodb://127.0.0.1";
                            configuration.DatabaseName = "TestDB";
                        })
                    .KeepIdempotencyInMongo();
            }
        }
    }
```

And register projectors

```
And Register 
using Autofac;
using Core.EventStore.Contracts;
using IntegrationEvents;
using QueryService.Projectors;

namespace QueryService.IoCC.Modules
{
    public class ProjectorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerCreatedEventProjector>().As<IProjector<CustomerCreated>>();
            builder.RegisterType<CustomerInsertedEventProjector>().As<IProjector<CustomerCreated>>();
        }
    }
}

```



Then persist occured event in CommandService
```

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandService.Commands;
using CommandService.Dtos;
using Core.EventStore.Dependencies;
using IntegrationEvents;
using MediatR;

namespace CommandService.CommandHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        readonly IEventStoreDbContext _eventStoreDbContext;

        public CreateCustomerCommandHandler(IEventStoreDbContext eventStoreDbContext)
        {
            _eventStoreDbContext = eventStoreDbContext;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand cmd, CancellationToken cancellationToken)
        {
            var @event = new CustomerCreated(cmd.FirstName, cmd.LastName, DateTime.UtcNow);

            //do sth
            
            var res = new CustomerDto()
            {
                FirstName = cmd.FirstName,
                LastName = cmd.LastName,
            };

            await _eventStoreDbContext.AppendToStreamAsync(@event);
            return res;
        }
    }
}

```


Then Projectors in QueryService will be triggered


```
using Core.EventStore.Contracts;
using IntegrationEvents;
using System.Threading.Tasks;
using QueryService.MongoDbConfigs;

namespace QueryService.Projectors
{
    public class CustomerCreatedEventProjector : IProjector<CustomerCreated>
    {
        private IMongoDb _mongoDb; 
        public CustomerCreatedEventProjector(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }
        
        public async Task HandleAsync(CustomerCreated integrationEvent)
        {
            await _mongoDb.InsertOneAsync(integrationEvent);
        }
    }
}

```




