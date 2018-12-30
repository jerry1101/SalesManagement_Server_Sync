using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_Server_Sync
{
    class OrderVO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; }
        public List<OrderLineVO> Lines{ get; set; }



    }
}
