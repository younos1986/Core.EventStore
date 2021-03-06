﻿using System;
using System.Threading.Tasks;
using Core.EventStore.Autofac;
using Core.EventStore.Configurations;
using Core.EventStore.Contracts;
using Core.EventStore.IdGeneration;
using Core.EventStore.Mongo.Autofac;
using EventStore.ClientAPI;
using MongoDB.Driver;

namespace Core.EventStore.Mongo.Implementations
{
    public class PositionReaderService: IPositionReaderService
    {
        private readonly IMongoConfiguration _mongoConfiguration; 
        public PositionReaderService(IMongoConfiguration mongoConfiguration)
        {
            _mongoConfiguration = mongoConfiguration;
        }
        
        public async Task<EventStorePosition> GetCurrentPosition()
        {
            var position =
                await _mongoConfiguration.GetPositionCollection
                    .Find(x => true)
                    .SortByDescending(x => x.CreatedOn)
                    .FirstOrDefaultAsync();

            if (position is null)
            {
                var defaultPosition = await CreateDefaultPosition();
                return defaultPosition;
            }

            return position;
        }
        
        private async Task<EventStorePosition> CreateDefaultPosition()
        {
            var defaultPosition =new EventStorePosition()
            {
                Id = CombGuid.Generate(),
                CommitPosition = Position.Start.CommitPosition,
                PreparePosition =Position.Start.PreparePosition, 
                CreatedOn = DateTime.UtcNow,
            }; 
            await _mongoConfiguration.GetPositionCollection.InsertOneAsync(defaultPosition);

            var createdPosition = await  _mongoConfiguration.GetPositionCollection
                .Find(x => true)
                .FirstOrDefaultAsync();

            return createdPosition;
        }
    }
}