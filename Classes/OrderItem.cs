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

        public int Order_ID { get; set; }
        public int Item_ID { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public OrderItem() { }
        public OrderItem(int orderID, int itemID, string description, double price, int quantity) {
            this.Order_ID = orderID;
            this.Item_ID = itemID;
            this.Description = description;
            this.Price = price;
            this.Quantity = quantity;
        }
        public OrderItem(DataRow datarow)
        {
            LoadOrderItemProperties(datarow);
        }

        private void LoadOrderItemProperties(DataRow dataRow)
        {
            this.Order_ID = (int)dataRow["OrderHeaderId"];
            this.Item_ID = (int)dataRow["StockItemId"];
            this.Description = dataRow["Description"].ToString();
            this.Price = double.Parse(dataRow["Price"].ToString());
            this.Quantity = (int)dataRow["Quantity"];
        }
    }
}
