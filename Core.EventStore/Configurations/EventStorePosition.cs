using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.EventStore.Configurations
{
    [BsonIgnoreExtraElements]
    public class EventStorePosition
    {
        public Guid Id { get; set; }
        /// <summary>The commit position of the record</summary>
        public long CommitPosition{ get; set; }
        /// <summary>The prepare position of the record.</summary>
        public long PreparePosition{ get; set; }
        public DateTime CreatedOn { get; set; }
    }
}