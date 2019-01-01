using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SalesManagement_Server_Sync
{
    public static class PlaceOrder
    {
        [FunctionName("AddOrder")]
        public static async Task<HttpResponseMessage> AddOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req,
            TraceWriter log,
            ExecutionContext context)
        {
            log.Info("Processing a AddOrder request.");

            var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory)
                        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                        .Build();

            string _requestBody = await req.Content.ReadAsStringAsync();


            if (_requestBody == null)
            {
                // Get request body
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass data info on the query string or in the request body");
            }
            else
            {
               

                var order = JsonConvert.DeserializeObject<OrderVO>(_requestBody);
                var _orderDA = new OrderDA();
                var _result = _orderDA.CreateOrder(config.GetConnectionString("SqlConnectionString"), order);
                return _result.Item1 == CRUDResultKey.Fail
                ? req.CreateResponse(HttpStatusCode.BadRequest, _result.Item2)
                : req.CreateResponse(HttpStatusCode.OK, _result.Item2);
            }


        }
    }
}
