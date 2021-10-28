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
    public class StockItem : Item
    {
        string connectionString = ConfigurationManager.ConnectionStrings["cnnStrOMS"].ConnectionString;
        DataTable dtStockItem;

        public string Name { get; set; }
        public int InStock { get; set; }

        public StockItem() : base()
        {
        }
        public StockItem(string name, double price, int instock)
        {
            this.Name = name;
            this.Price = price;
            this.InStock = instock;
        }
        public StockItem(DataRow datarow) : base()
        {
            LoadStockItemProperties(datarow);
        }
        public StockItem(int stockItemID) : base()
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@StockItemID", stockItemID) };
                this.dtStockItem = myDal.ExcuteStoredProc("usp_GetStockItem", parameters);
                if (dtStockItem != null && dtStockItem.Rows.Count > 0)
                {
                    LoadStockItemProperties(dtStockItem.Rows[0]);
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Unable to retrieve stock item details!", ex);
            }
            finally
            {
                this.dtStockItem.Dispose();
                this.dtStockItem = null;
                myDal = null;
            }
        }

        public override int Add()
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            SqlParameter[] parameters = {new SqlParameter("@Name", this.Name),
                                         new SqlParameter("@Price", this.Price),
                                         new SqlParameter("@InStock", this.InStock)};
            return myDal.ExecuteNonQuerySP("usp_AddStockItem", parameters);
        }
        public override int Delete()
        {
            throw new NotImplementedException();
        }
        public override int Get()
        {

            try
            {
                SqlDataAccessLayer myDal = new SqlDataAccessLayer();
                SqlParameter[] parameters = { new SqlParameter("@StockItemID", this.Item_ID) };
                this.dtStockItem = myDal.ExcuteStoredProc("usp_GetStockItem", parameters);
                if (dtStockItem != null && dtStockItem.Rows.Count > 0)
                {
                    LoadStockItemProperties(dtStockItem.Rows[0]);
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve the stock item!", ex);
            }

        }
        public override int Update()
        {
            throw new NotImplementedException();
        }

        private void LoadStockItemProperties(DataRow dataRow)
        {
            this.Item_ID = (int)dataRow["Id"];
            this.Name = dataRow["Name"].ToString();
            this.Price = double.Parse(dataRow["Price"].ToString());
            this.InStock = (int)dataRow["InStock"];
        }

        public override string ToString()
        {
            return $"ID: {this.Item_ID}\n Name:{this.Name}\n Price: {this.Price:C2}\n Stock: {this.InStock}";
        }
    }
}
