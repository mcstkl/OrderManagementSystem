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
    public class OrderHeader : List<OrderItem>
    {
        public DataTable dtOrderHeader;
        

        private string connectionString = ConfigurationManager.ConnectionStrings["cnnStrOMS"].ConnectionString;

        public int ID { get; set; }
        public int State { get; set; }
        public DateTime OrderDate { get; set; }
        public int NumberOfItems
        {
            get {
                return this.Count; }
            
        }


        public string ConvertedState
        {
            get {   if (this.State == 1) return OrderState.New.ToString();
                    if (this.State == 2) return OrderState.Pending.ToString();
                    if (this.State == 3) return OrderState.Rejected.ToString();
                    if (this.State == 4) return OrderState.Complete.ToString();
                return null;
            }
            set { ConvertedState = value; }
        }

        public List<OrderItem> OrderItems = new List<OrderItem>();

        public OrderHeader()
        {
            AddItemsToHeader();
        }


        public OrderHeader(int state, DateTime orderDate) 
        {
            this.State = state;
            this.OrderDate = orderDate;
            AddItemsToHeader();
        }
        public OrderHeader(DataRow dataRow)
        {
            LoadOrderHeaderProperties(dataRow);
            AddItemsToHeader();
        }



        public int GetOrder()
        {
            try
            {
                SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
                SqlParameter[] parameters = { new SqlParameter("@OrderHeaderID", this.ID) };
                this.dtOrderHeader = myDal.ExcuteStoredProc("usp_GetOrderHeader", parameters);
                if (dtOrderHeader != null && dtOrderHeader.Rows.Count > 0)
                {
                    LoadOrderHeaderProperties(dtOrderHeader.Rows[0]);
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve the order!", ex);
            }

        }
        public int AddNewOrder()
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO OrderHeader(OrderStateId, OrderDate) output INSERTED.ID VALUES(@State, @OrderDate)", connection))
                {
                    cmd.Parameters.AddWithValue("@State", this.State);
                    cmd.Parameters.AddWithValue("@OrderDate", this.OrderDate);
                    connection.Open();

                    this.ID = (int)cmd.ExecuteScalar();

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            AddAllItemsToOrder();
            return 0;
        }
        public int DeleteOrder(int orderID)
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            OrderItems = new List<OrderItem>();
            myDal.ExecuteNonQuerySP("usp_DeleteAllItemsFromOrder", new SqlParameter[] { new SqlParameter("@Order_ID", orderID) });
            return myDal.ExecuteNonQuerySP("usp_DeleteOrder", new SqlParameter[] { new SqlParameter("@Order_ID", orderID) });
        }
        public int UpdateOrder(int orderID, int state, DateTime orderDate)
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            SqlParameter[] parameters = {new SqlParameter("@Order_ID", orderID),
                                         new SqlParameter("@State", state),
                                         new SqlParameter("@OrderDate", orderDate)};
            return myDal.ExecuteNonQuerySP("usp_UpdateOrder", parameters);
        }
        public void AddNewItemToOrder(int itemID, string description, double price, int quantity)
        {
            this.Add(new OrderItem(this.ID, itemID, description, price, quantity));
        }
        public DataTable GetAllOrderItems()
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            DataTable dtOrderItems = new DataTable();
            dtOrderItems = myDal.ExcuteStoredProc("usp_GetAllOrderItems");
            return dtOrderItems;
        }


        public override string ToString()
        {
            return $"ID: {this.ID}\n State:{this.State}\n Date: {this.OrderDate}";
        }


        private void LoadOrderHeaderProperties(DataRow dataRow)
        {
            this.ID = (int)dataRow["Id"];
            this.State = (int)dataRow["OrderStateId"];
            this.OrderDate = (DateTime)dataRow["OrderDate"];
        }
        private int AddAllItemsToOrder()
        {
            int rowsAffected = 0;
            foreach (OrderItem item in this)
            {
                SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
                SqlParameter[] parameters = {   new SqlParameter("@OrderID", this.ID),
                                                new SqlParameter("@StockItemID", item.Item_ID),
                                                new SqlParameter("@Description", item.Description),
                                                new SqlParameter("@Price", item.Price),
                                                new SqlParameter("@Quantity", item.Quantity)};
                rowsAffected += myDal.ExecuteNonQuerySP("usp_AddOrderItem", parameters);
            }
            return rowsAffected;
        }


        private void AddItemsToHeader()
        {
            DataTable dtOrderItems = GetAllOrderItems();
            foreach (DataRow row in dtOrderItems.Rows)
            {
                if (this.ID == (int)row["OrderHeaderId"])
                {
                    OrderItem item = new OrderItem(row);
                    this.Add(item);
                }
            }
        }

    }   
}
