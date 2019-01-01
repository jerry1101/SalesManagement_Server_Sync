using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_Server_Sync
{
    class OrderDA
    {
        private const string _insertOrderCommandString = "insert into dbo.Orders (orderid,orderstatus,customerid,ModifiedDate) values (@orderid,@status,@customerid,@ModifiedDate)";
        private const string _insertLinesCommandString = "insert into dbo.OrderLines (orderid,sku,qty,ModifiedDate) values (@orderid,@sku,@qty,@ModifiedDate) ";
        public Tuple<string, string> CreateOrder(string connectionString, OrderVO order)
        {
            long OrderId;
            try
            {
                
                using (SqlConnection sqlconnection = new SqlConnection(connectionString))
                {
                    var cmd = new SqlCommand(_insertOrderCommandString, sqlconnection);
                    
                    sqlconnection.Open();
                    OrderId = this.InsertOrderHead(sqlconnection,order.CustomerId);
                    this.InsertOrderLines(sqlconnection, OrderId, order.Lines);

                }
                return Tuple.Create(CRUDResultKey.Success, $"Order Id is {OrderId}");
            }
            catch (Exception e)
            {

                return Tuple.Create(CRUDResultKey.Fail, e.Message);
            }

            
        }

        private long InsertOrderHead(SqlConnection sqlConnection,int customerId)
        {
            var OrderId = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            var cmd = new SqlCommand(_insertOrderCommandString, sqlConnection);
            cmd.Parameters.Add("@orderid", SqlDbType.BigInt).Value = OrderId;
            cmd.Parameters.Add("@customerid", SqlDbType.BigInt).Value = customerId;
            cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = OrderStatusConst.Added;
            cmd.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.ExecuteNonQuery();
            return OrderId;
        }
        private void InsertOrderLines(SqlConnection sqlConnection, long orderId, IEnumerable<OrderLineVO> lines)
        {
            foreach (var line in lines)
            {
                var cmd = new SqlCommand(_insertLinesCommandString, sqlConnection);
                cmd.Parameters.Add("@orderid", SqlDbType.BigInt).Value = orderId;
                cmd.Parameters.Add("@sku", SqlDbType.BigInt).Value = line.SKU;
                cmd.Parameters.Add("@qty", SqlDbType.Int).Value = line.Quantity;
                cmd.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.ExecuteNonQuery();

            }
            


            

        }


    }
}
