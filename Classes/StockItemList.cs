using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.SqlHelper;

namespace OMS.Classes
{
    public class StockItemList : List<StockItem>
    {
        DataTable dtStockItems;
        string connectionString = ConfigurationManager.ConnectionStrings["cnnStrOMS"].ConnectionString;
        public StockItemList()
        {
            GetAllStockItems();
        }

        public void GetAllStockItems()
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            dtStockItems = myDal.ExcuteStoredProc("usp_GetAllStockItems");
            foreach (DataRow item in dtStockItems.Rows)
            {
                StockItem stockItem = new StockItem(item);
                this.Add(stockItem);
            }
        }


        public override string ToString()
        {
            string allItems = string.Empty;
            foreach (StockItem item in this)
            {
                allItems += $"ID: {item.Item_ID}\n Name:{item.Name}\n Price: {item.Price:C2}\n Stock: {item.InStock}\n\n";
            }
            return allItems;
        }
    }
}
