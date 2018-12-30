using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_Server_Sync
{
    class OrderLineVO
    {
        public int SKU { get; set; }
        public int quantity { get; set; }

        public static explicit operator List<object>(OrderLineVO v)
        {
            throw new NotImplementedException();
        }
    }
}
