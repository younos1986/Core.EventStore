using System;
using Autofac;
using Core.EventStore.Contracts;
using Core.EventStore.Mongo.Implementations;

namespace Core.EventStore.Mongo.Autofac
{
    public static class Registration
    {
        public static ContainerBuilder KeepPositionInMongo(this ContainerBuilder containerBuilder, Action<MongoConfiguration> mongoConfiguration = null)
        {
            if (mongoConfiguration != null)
            {
                MongoConfiguration configuration = new MongoConfiguration();
                mongoConfiguration.Invoke(configuration);
                containerBuilder.RegisterInstance(configuration).As<IMongoConfiguration>()
                    .IfNotRegistered(typeof(IMongoConfiguration)).SingleInstance();
            }
            
            containerBuilder.RegisterType<PositionReaderService>().As<IPositionReaderService>().SingleInstance();
            containerBuilder.RegisterType<PositionWriteService>().As<IPositionWriteService>().SingleInstance();

            return containerBuilder;
        }
        
        public static ContainerBuilder KeepIdempotenceInMongo(this ContainerBuilder containerBuilder, Action<MongoConfiguration> mongoConfiguration = null)
        {
            if (mongoConfiguration != null)
            {
                MongoConfiguration configuration = new MongoConfiguration();
                mongoConfiguration.Invoke(configuration);
                containerBuilder.RegisterInstance(configuration).As<IMongoConfiguration>()
                    .IfNotRegistered(typeof(IMongoConfiguration)).SingleInstance();
            }

            containerBuilder.RegisterType<IdempotenceWriterService>().As<IIdempotenceWriterService>().SingleInstance();
            containerBuilder.RegisterType<IdempotenceReaderService>().As<IIdempotenceReaderService>().SingleInstance();
            
            return containerBuilder;
        }
    }
}
