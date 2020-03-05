using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.EventStore.IdempotencyServices
{
    [BsonIgnoreExtraElements]
    public class EventStoreIdempotency
    {
        public Guid Id  { get; set; }
        public DateTime CreatedOn  { get; set; }
        
    }
}