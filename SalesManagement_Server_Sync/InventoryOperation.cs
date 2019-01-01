using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SalesManagement_Server_Sync.Inventory
{
    public static class InventoryOperation
    {
        [FunctionName("BookInventory")]
        public static async Task<HttpResponseMessage> BookInventory([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req,
            TraceWriter log,
            ExecutionContext context)
        {
            log.Info("Processing a BookInventory request.");

            var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory)
                        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                        .Build();

            string _requestBody = await req.Content.ReadAsStringAsync();


            if (_requestBody == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass order info in the request body");
            }
            else
            {


                var _listInv = JsonConvert.DeserializeObject<List<InventoryVO>>(_requestBody);
                var _validator = new InventoryVOValidator();
                foreach (var item in _listInv)
                {


                    var _validaResult = _validator.Validate(item);

                    if (!_validaResult.IsValid)
                    {
                        return req.CreateResponse(HttpStatusCode.BadRequest, _validaResult.Errors);
                    }
                }


                var _invDA = new InventoryDA();
                var _result = _invDA.ReserveInventory(config.GetSection("InventoryServiceSqlConn").Value, _listInv);
                return _result.Item1 == CRUDResultKey.Fail
                ? req.CreateResponse(HttpStatusCode.BadRequest, _result.Item2)
                : req.CreateResponse(HttpStatusCode.OK, _result.Item2);
            }
        }
    }
}
