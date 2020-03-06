using Xunit;

namespace Core.EventStore.Test
{
    [CollectionDefinition(nameof(EventStoreCollection))]
    public class EventStoreCollection : ICollectionFixture<EventStoreFixture>
    {
    }
    
    [CollectionDefinition(nameof(MongoDbCollection))]
    public class MongoDbCollection : ICollectionFixture<MongoDbFixture>
    {
    }
}