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
    public class StockItem : IComparable
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["cnnStrOMS"].ConnectionString;
        DataTable dtStockItem;

        public int Item_ID { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public int InStock { get; set; }

        public StockItem()
        {
        }
        public StockItem(string name, double price, int instock)
        {
            this.Name = name;
            this.Price = price;
            this.InStock = instock;
        }
        public StockItem(DataRow datarow)
        {
            LoadStockItemProperties(datarow);
        }
        public StockItem(int stockItemID)
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

        public int Add()
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            SqlParameter[] parameters = {new SqlParameter("@Name", this.Name),
                                         new SqlParameter("@Price", this.Price),
                                         new SqlParameter("@InStock", this.InStock)};
            return myDal.ExecuteNonQuerySP("usp_AddStockItem", parameters);
        }
        public int Delete()
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            return myDal.ExecuteNonQuerySP("usp_DeleteStockItem", new SqlParameter[] { new SqlParameter("@Item_ID", this.Item_ID)});
        }
        public int Get()
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
        public int Update()
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            SqlParameter[] parameters = {new SqlParameter("@Item_ID", this.Item_ID),
                                         new SqlParameter("@Name", this.Name),
                                         new SqlParameter("@Price", this.Price),
                                         new SqlParameter("@InStock", this.InStock)};
            return myDal.ExecuteNonQuerySP("usp_UpdateStockItem", parameters);
        }
        public int Update(int itemID, string name, double price, int instock)
        {
            SqlDataAccessLayer myDal = new SqlDataAccessLayer(connectionString);
            SqlParameter[] parameters = {new SqlParameter("@Item_ID", itemID),
                                         new SqlParameter("@Name", name),
                                         new SqlParameter("@Price", price),
                                         new SqlParameter("@InStock", instock)};
            return myDal.ExecuteNonQuerySP("usp_UpdateStockItem", parameters);
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

        public int CompareTo(object obj)
        {
            StockItem other = (StockItem)obj;
            return Name.CompareTo(other.Name);
        }
    }
}
