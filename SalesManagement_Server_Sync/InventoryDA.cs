using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SalesManagement_Server_Sync.Inventory
{
    public class InventoryDA
    {
        private const string _updateInventoryCommandString = "update dbo.Inventories set available = @available where sku = @sku";
        
        public InventoryDA()
        {
        }
        public Tuple<string,string> ReserveInventory(string connString, List<InventoryVO> listInv)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    foreach (var item in listInv)
                    {
                        this.UpdateInventory(conn, item.SKU, item.Quantity);
                }
                }
                
                return Tuple.Create(CRUDResultKey.Success, "Update is done");
            }
            catch (Exception e)
            {

                return Tuple.Create(CRUDResultKey.Fail, e.Message);
            }
        }

        private bool UpdateInventory(SqlConnection sqlConnection, int sku, int available)
        {

            var cmd = new SqlCommand(_updateInventoryCommandString, sqlConnection);
            cmd.Parameters.Add("@available", SqlDbType.BigInt).Value = available;
            cmd.Parameters.Add("@sku", SqlDbType.BigInt).Value = sku;
            return cmd.ExecuteNonQuery() ==1 ? true:false ;
        }
    }
}
