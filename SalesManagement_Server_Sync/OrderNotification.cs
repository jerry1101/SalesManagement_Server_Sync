using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace SalesManagement_Server_Sync
{
    public static class OrderNotification
    {
        [FunctionName("OrderNotification")]
        public static void Run([ServiceBusTrigger("inventory-booked", AccessRights.Listen, Connection = "Endpoint=sb://demosales.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=rJGhFDX2T9huBboCtXINxtXgJx4242WM6FrsnOkOCBo=")]string myQueueItem, TraceWriter log)
        {
            log.Info($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
