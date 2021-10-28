using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Classes
{
    public class OrderHeader
    {
        public int ID { get; set; }
        public string State { get; set; }
        public DateTime OrderDate { get; set; }

        List<OrderItem> OrderItems = new List<OrderItem>();
    }
}
