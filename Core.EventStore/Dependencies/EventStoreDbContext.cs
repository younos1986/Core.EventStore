using Core.EventStore.Autofac;
using Core.EventStore.Invokers;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.EventStore.IdGeneration;

namespace Core.EventStore.Dependencies
{
    public class EventStoreDbContext : IEventStoreDbContext
    {
        readonly IEventStoreConnection _eventStoreConnection;

        public EventStoreDbContext(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

        public async Task AppendToStreamAsync<T>(T command, Guid? eventId = null)
        {
            string commandName = command.GetType().Name;
            string jsonData = JsonConvert.SerializeObject(command);
            byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);

            EventData eventData = new EventData(
                eventId: eventId ?? CombGuid.Generate(),
                type: commandName,
                isJson: true,
                data: dataBytes,
                metadata: null);


            string streamName = command.GetType().Name;


            await AppendToStreamAsync(streamName, eventData);
        }


        private async Task AppendToStreamAsync(string streamName, params EventData[] events)
        {
            await _eventStoreConnection.AppendToStreamAsync(streamName, ExpectedVersion.Any, events);
        }
    }
}