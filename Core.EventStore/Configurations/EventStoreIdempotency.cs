using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.EventStore.Configurations
{
    [BsonIgnoreExtraElements]
    public class EventStoreIdempotence
    {
        public Guid Id  { get; set; }
        public DateTime CreatedOn  { get; set; }
        
    }
}