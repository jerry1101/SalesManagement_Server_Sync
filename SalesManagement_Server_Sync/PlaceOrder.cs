using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace SalesManagement_Server_Sync
{
    public static class PlaceOrder
    {
        [FunctionName("AddOrder")]
        public static async Task<HttpResponseMessage> AddOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, 
            TraceWriter log)
        {
            log.Info("Processing a AddOrder request.");
            string _requestBody = await req.Content.ReadAsStringAsync();
            

            if (_requestBody == null)
            {
                // Get request body
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass data info on the query string or in the request body");
            }
            else
            {
                var _OrderLines = JsonConvert.DeserializeObject<List<OrderLineVO>>(_requestBody);
                return _OrderLines.Count < 1
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass skus in the request body")
                : req.CreateResponse(HttpStatusCode.OK, $"Hello order id is {(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000}");
            }

            
        }
    }
}
