using App.SqlHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Classes
{
    public class OrderList : List<OrderHeader>
    {
        DataTable dtOrders;
        string connectionString = ConfigurationManager.ConnectionStrings["cnnStrOMS"].ConnectionString;

        public OrderList()
        {
            GetAllOrders();
        }

        public void GetAllOrders()
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            dtOrders = myDal.ExcuteStoredProc("usp_GetAllOrders");
            foreach (DataRow item in dtOrders.Rows)
            {
                OrderHeader order = new OrderHeader(item);
                this.Add(order);
            }
        }
    }
}
