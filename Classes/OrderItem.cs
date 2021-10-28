using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Classes
{
    public class OrderItem : Item
    {
        DataTable dtOrderItem;

        public int Order_ID { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }

        public OrderItem() { }

        public override int Add()
        {
            throw new NotImplementedException();
        }
        public override int Delete()
        {
            throw new NotImplementedException();
        }
        public override int Get()
        {
            throw new NotImplementedException();
        }
        public override int Update()
        {
            throw new NotImplementedException();
        }
    }
}
