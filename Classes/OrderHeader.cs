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
    public class OrderHeader
    {
        DataTable dtOrderHeader;

        private string connectionString = ConfigurationManager.ConnectionStrings["cnnStrOMS"].ConnectionString;

        public int ID { get; set; }
        public int State { get; set; }
        public DateTime OrderDate { get; set; }
        List<OrderItem> OrderItems = new List<OrderItem>();

        public OrderHeader(){}
        public OrderHeader(DataRow dataRow)
        {
            LoadOrderHeaderProperties(dataRow);
        }



        public int Get()
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
        public int Add()
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            SqlParameter[] parameters = {new SqlParameter("@Name", this.State),
                                         new SqlParameter("@Price", this.OrderDate)};
            return myDal.ExecuteNonQuerySP("usp_AddOrderHeader", parameters);
        }
        //public int Delete(int orderID)
        //{
        //    SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
        //    return myDal.ExecuteNonQuerySP("usp_DeleteStockItem", new SqlParameter[] { new SqlParameter("@Item_ID", itemID) });
        //}
        //public int Update(int itemID, string name, double price, int instock)
        //{
        //    SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
        //    SqlParameter[] parameters = {new SqlParameter("@Item_ID", itemID),
        //                                 new SqlParameter("@Name", name),
        //                                 new SqlParameter("@Price", price),
        //                                 new SqlParameter("@InStock", instock)};
        //    return myDal.ExecuteNonQuerySP("usp_UpdateStockItem", parameters);
        //}

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

    }   
}
