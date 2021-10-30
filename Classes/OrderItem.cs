using App.SqlHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Classes
{
    public class OrderItem
    {
        DataTable dtOrderItem;
        private string connectionString = ConfigurationManager.ConnectionStrings["cnnStrOMS"].ConnectionString;
        public int Order_ID { get; set; }
        public int Item_ID { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public string ItemDetail
        {
            get {
                StockItemList list = new StockItemList();
                string orderDetail = string.Empty;
                foreach(var item in list)
                {
                    if(item.Item_ID == this.Item_ID)
                    {
                        orderDetail += $"{item.Name,-15}\t  {this.Price:C2}  \tQTY: {this.Quantity}";
                      
                    }
                }
                return orderDetail; 
            }
        }


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
        public int DeleteOrderItem()
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@Order_ID", this.Order_ID),
                                                                new SqlParameter("@StockItem_ID", this.Item_ID)};
            return myDal.ExecuteNonQuerySP("usp_DeleteOrderItem", parameters);
        }

        public int AddItemToOrder()
        {
                SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@Order_ID", this.Order_ID),
                                                                new SqlParameter("@StockItem_ID", this.Item_ID),
                                                                new SqlParameter("@Description", this.Description),
                                                                new SqlParameter("@Price", this.Price),
                                                                new SqlParameter("@Quantity", this.Quantity)};
                return myDal.ExecuteNonQuerySP("usp_AddOrderItem", parameters);
        }
    }
}
