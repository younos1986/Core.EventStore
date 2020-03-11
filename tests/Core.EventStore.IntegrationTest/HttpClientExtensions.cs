using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.EventStore.IntegrationTest
{
    public static  class HttpClientExtensions
    {
        
        public static async Task<HttpResponseMessage> TryGetAsync(this HttpClient client, string requestUri)
        {
            var timeToRest = 0;
            HttpResponseMessage response = null;
            do
            {
                await Task.Delay(timeToRest);
                response = await client.GetAsync(requestUri);
                timeToRest = 1;

            } while (response.StatusCode == HttpStatusCode.NoContent);
            return response;
        }
        
    }
}