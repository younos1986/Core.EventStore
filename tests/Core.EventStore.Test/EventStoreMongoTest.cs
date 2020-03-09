﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Core.EventStore.Autofac;
using Core.EventStore.Dependencies;
using Core.EventStore.Mongo.Autofac;
using Core.EventStore.Test.DockerFramework.Containers;
using Core.EventStore.Test.MongoServers;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using FluentAssertions;
using IntegrationEvents;
//using Microsoft.AspNetCore.TestHost;
using MongoDB.Driver;
using Newtonsoft.Json;
using Xunit;

namespace Core.EventStore.Test
{
   
    public class EventStoreMongoTest  :IClassFixture<DbFixture>
    {
      
        private HttpClient _queryApi;
        private HttpClient _commandApi;
        
        
        public EventStoreMongoTest(DbFixture fixture)
        {
            var eventStoreContainer =  fixture.Container.Resolve<EventStoreContainer>();
            var mongoDbContainer =  fixture.Container.Resolve<MongoDbContainer>();

            _commandApi = new MongoCommandServiceApi().GetClient().GetAwaiter().GetResult();
            _queryApi =  new MongoQueryServiceApi().GetClient().GetAwaiter().GetResult();
        }
       

        [Fact]
        public async Task Raised_Event_Should_Be_In_Mongo()
        {
            
            var command = new CustomerCreated(Guid.NewGuid(),"Younes" , "Baghaie Moghaddam", DateTime.Now);
            var jsonCustomer = JsonConvert.SerializeObject(command);
            
            // create a customer
            var customerData = new StringContent(jsonCustomer, UTF8Encoding.UTF8, "application/json");
            var createCustomerResponse = await _commandApi.PostAsync("api/Customers/CreateCustomer", customerData);
            createCustomerResponse.EnsureSuccessStatusCode();

            var createCustomerString = await createCustomerResponse.Content.ReadAsStringAsync();
            var createdCustomer = JsonConvert.DeserializeObject<CustomerCreated>(createCustomerString);

            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            
            var getCustomerResponse = await _queryApi.GetAsync($@"api/Customers/GetCustomer?id={ createdCustomer.Id }");
            var getCustomerString = await getCustomerResponse.Content.ReadAsStringAsync();
            var retrievedCustomer = JsonConvert.DeserializeObject<CustomerCreated>(getCustomerString);

            createdCustomer.Id.Should().Be(retrievedCustomer.Id);

        }
    }
}