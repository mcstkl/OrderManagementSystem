using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Classes
{
    public class OrderItem
    {
        DataTable dtOrderItem;

        public int Item_ID { get; set; }
        public int Order_ID { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }

        public OrderItem() { }

        public int Add()
        {
            throw new NotImplementedException();
        }
        public int Delete(int itemID)
        {
            throw new NotImplementedException();
        }
        public int Get()
        {
            throw new NotImplementedException();
        }
        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
