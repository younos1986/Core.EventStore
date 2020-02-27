using System;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;

namespace Core.EventStore.IntegrationTest.Extensions
{
    static class HttpClientExtensions
    {
        public static HttpClient CreateIdempotentClient(this TestServer server, Uri uri)
        {
            var client = server.CreateClient();
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.Add("x-requestid", Guid.NewGuid().ToString());
            return client;
        }
    }
}