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
using System.Windows.Threading;

namespace OMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        int panelWidth;
        bool hidden;
        public MainWindow()
        {
            InitializeComponent();
            InitializeMenu();
            
          
            #region hi
            //OrderHeader oh = new OrderHeader();
            //DataTable dtAllItems = oh.GetAllOrderItems();
            //string allItems = string.Empty;
            //List<OrderItem> allOrderItems = new List<OrderItem>();
            //foreach (DataRow item in dtAllItems.Rows)
            //{
            //    OrderItem orderItem = new OrderItem(item);
            //    allOrderItems.Add(orderItem);
            //}
            //foreach (OrderItem item in allOrderItems)
            //{
            //    allItems += $"OrderID: {item.Order_ID}\n" +
            //                $"StockItemID: {item.Item_ID}\n" +
            //                $"Description: {item.Description}\n" +
            //                $"Price: {item.Price}\n" +
            //                $"Quantity: {item.Quantity}\n\n";
            //}
            //MessageBox.Show(allItems);
            //OrderHeader oh = new OrderHeader(4, DateTime.Today);
            //MessageBox.Show(oh.ToString());
            //oh.AddNewItemToOrder(5, "Bag of Nails", 65, 10);
            //oh.AddNewItemToOrder(3, "SomeStuff", 44, 25);
            //oh.AddNewItemToOrder(2, "SomeOtherStuff", 55, 23);
            //oh.AddNewItemToOrder(6, "MoreDescriptions", 99, 43);
            //oh.GetOrder();
            //oh.AddNewOrder();
            //oh.DeleteOrder(18);
            //OrderList ol = new OrderList();
            //MessageBox.Show(ol.ToString());
            //StockItem stockItem = new StockItem("Shampoo", 12, 200);
            //MessageBox.Show(stockItem.ToString());
            //stockItem.Update(10, stockItem.Name, stockItem.Price, stockItem.InStock);
            //StockItemList stockItems = new StockItemList();
            //MessageBox.Show(stockItems.ToString());
            #endregion

        }


        // +++++++++++++++++++++ MENU FUNCTIONS +++++++++++++++++++++++++++++++++
        private void InitializeMenu()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += Timer_Tick;
            panelWidth = (int)sidePanel.Width;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                sidePanel.Width += 5;
                imgLogo.Visibility = Visibility.Visible;
                if (sidePanel.Width >= panelWidth)
                {
                    timer.Stop();
                    hidden = false;
                }
            }
            else
            {
                sidePanel.Width -= 5;
                if(sidePanel.Width <= 35)
                {
                    timer.Stop();
                    hidden = true;
                    imgLogo.Visibility = Visibility.Hidden;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();

        }
        private void panelHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }



        // +++++++++++++++++++++++ MENU BUTTONS +++++++++++++++++++++++++++++++++++
        private void btnCloseX_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Key pressed");
            Environment.Exit(0);
        }
        private void btnCloseMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnHomeMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            panelHome.Visibility = Visibility.Visible;
            panelOrders.Visibility = Visibility.Hidden;
            panelInventory.Visibility = Visibility.Hidden;
            panelExport.Visibility = Visibility.Hidden;
        }
        private void btnOrdersMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            panelHome.Visibility = Visibility.Hidden;
            panelOrders.Visibility = Visibility.Visible;
            panelInventory.Visibility = Visibility.Hidden;
            panelExport.Visibility = Visibility.Hidden;
        }
        private void btnInventoryMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            panelHome.Visibility = Visibility.Hidden;
            panelOrders.Visibility = Visibility.Hidden;
            panelInventory.Visibility = Visibility.Visible;
            panelExport.Visibility = Visibility.Hidden;
        }
        private void btnExportMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            panelHome.Visibility = Visibility.Hidden;
            panelOrders.Visibility = Visibility.Hidden;
            panelInventory.Visibility = Visibility.Hidden;
            panelExport.Visibility = Visibility.Visible;
        }

        // +++++++++++++++++++++++++++ PANEL SWAP +++++++++++++++++++++++++++++++

    }
}
