using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.EventStore.IntegrationTest.Infrastructures
{
    public static  class HttpClientExtensions
    {
        
        public static async Task<HttpResponseMessage> TryGetAsync(this HttpClient client, string requestUri)
        {
            var timeToRest = 0;
            var counter = 0;
            HttpResponseMessage response = null;
            do
            {
                await Task.Delay(timeToRest);
                response = await client.GetAsync(requestUri);
                timeToRest = 1;
            } while (response.StatusCode == HttpStatusCode.NoContent &&  counter < 10);
            return response;
        }
        
    }
}