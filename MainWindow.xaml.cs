using OMS.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using App.SqlHelper;
namespace OMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            OrderHeader oh = new OrderHeader();
            string output = $"ID: {oh.ID}\n State: {oh.State}\n Date: {oh.OrderDate}\n";
            MessageBox.Show(output);

            OrderList ol = new OrderList();
            string list = string.Empty;
            foreach(OrderHeader item in ol)
            {
                list += $"ID: {item.ID}\n State: {item.State}\n Date: {item.OrderDate}\n\n";
            }
            MessageBox.Show(list);







            //StockItem stockItem = new StockItem("Shampoo", 12, 200);
            //MessageBox.Show(stockItem.ToString());
            //stockItem.Update(10, stockItem.Name, stockItem.Price, stockItem.InStock);


            //StockItemList stockItems = new StockItemList();
            //MessageBox.Show(stockItems.ToString());

        }
    }
}
