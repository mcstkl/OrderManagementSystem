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
        public OrderHeader(int state, DateTime orderDate) 
        {
            this.State = state;
            this.OrderDate = orderDate;
        }
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
            SqlParameter[] parameters = {new SqlParameter("@State", this.State),
                                         new SqlParameter("@OrderDate", this.OrderDate)};
            return myDal.ExecuteNonQuerySP("usp_AddOrderHeader", parameters);
        }
        public int Delete(int orderID)
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            return myDal.ExecuteNonQuerySP("usp_DeleteOrder", new SqlParameter[] { new SqlParameter("@Order_ID", orderID) });
        }
        public int Update(int orderID, int state, DateTime orderDate)
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            SqlParameter[] parameters = {new SqlParameter("@Order_ID", orderID),
                                         new SqlParameter("@State", state),
                                         new SqlParameter("@OrderDate", orderDate)};
            return myDal.ExecuteNonQuerySP("usp_UpdateOrder", parameters);
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

    }   
}
