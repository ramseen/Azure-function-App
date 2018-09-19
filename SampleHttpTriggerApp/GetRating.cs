using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace SampleHttpTriggerApp
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
           


            return req.CreateResponse(HttpStatusCode.OK, "");


        }
        public class Rating
        {
            public string Id { get; set; }
            public string Userid { get; set; }
            public string ProductId { get; set; }
            public string Timestamp { get; set; }
            public string UserRating { get; set; }
            public string Usernotes { get; set; }
            public string Locationname { get; set; }

        }
    }
}
