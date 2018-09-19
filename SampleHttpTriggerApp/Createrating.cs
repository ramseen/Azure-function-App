using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace SampleHttpTriggerApp
{
    public static class Createrating
    {
        [FunctionName("Createrating")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            Guid Guidobj = Guid.NewGuid();


            // parse query parameter
            string userId = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "userId", true) == 0)
                .Value;
            string productId = req.GetQueryNameValuePairs()
              .FirstOrDefault(q => string.Compare(q.Key, "productId", true) == 0)
              .Value;
            string locationName = req.GetQueryNameValuePairs()
             .FirstOrDefault(q => string.Compare(q.Key, "locationName", true) == 0)
             .Value;
            string rating = req.GetQueryNameValuePairs()
              .FirstOrDefault(q => string.Compare(q.Key, "rating", true) == 0)
              .Value;
            string userNotes = req.GetQueryNameValuePairs()
              .FirstOrDefault(q => string.Compare(q.Key, "userNotes", true) == 0)
              .Value;

            if (userId == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                userId = data?.name;
            }
            if (productId == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                productId = data?.name;
            }
            if (locationName == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                locationName = data?.name;
            }
            if (rating == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                rating = data?.name;
            }
            if (userNotes == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                userNotes = data?.name;
            }


            bool userIsvalid = Validateuserid(userId);
            bool prodisvalid = Validateproductid(productId);

            string JSONresult = string.Empty;
            if (userIsvalid && prodisvalid == true)
            {
                DateTime now = DateTime.Now;

                Rating ratingObj = new Rating();
                ratingObj.Id = Guidobj.ToString();
                ratingObj.ProductId = productId;
                ratingObj.Userid = userId;
                ratingObj.UserRating = rating;
                ratingObj.Usernotes = userNotes;
                ratingObj.Timestamp = now.ToString();
                ratingObj.Locationname = locationName;
                
                JSONresult = JsonConvert.SerializeObject(ratingObj);
            
                using (WebClient client = new WebClient())
                {
                    string responseString = client.DownloadString("https://rmsichallenge2.azurewebsites.net/api/HttpTriggerCSharp1?code=PJWfy28rRIjv03dsIAqRD9Mndn63upFafeEFQd5mxEcjcNSPTzsOPQ==&userid=" + userId + "&productid=" + productId

                        + "&rating=" + rating + "&usernotes=" + userNotes + "&timestamp=" + ratingObj.Timestamp + "&id=" + ratingObj.Id


                      );


                }

            }


            return req.CreateResponse(HttpStatusCode.OK, JSONresult);


        }


        public static bool Validateuserid(string userid)
        {
            try
            {
                string responseString = string.Empty;
                bool result = false;
                using (WebClient client = new WebClient())
                {
                    responseString = client.DownloadString("https://serverlessohuser.trafficmanager.net/api/GetUser?userId=" + userid);
                    if (responseString != null)
                    {
                        result = true;
                    }


                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static bool Validateproductid(string prodid)
        {
            try
            {
                string responseString = string.Empty;
                bool result = false;
                using (WebClient client = new WebClient())
                {
                    responseString = client.DownloadString("https://serverlessohproduct.trafficmanager.net/api/GetProduct?productId=" + prodid);
                    if (responseString != null)
                    {
                        result = true;
                    }


                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

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
