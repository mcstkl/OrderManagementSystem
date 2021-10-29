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
        OrderList allOrders = new OrderList();
        OrderHeader allItems = new OrderHeader();
        DispatcherTimer timer;
        int panelWidth;
        bool hidden;


        public MainWindow()
        {
            InitializeComponent();
            InitializeMenu();
            LoadOrderHeaderListView();

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
                panelHome.Width -= 5;
                panelOrders.Width -= 5;
                panelInventory.Width -= 5;
                panelExport.Width -= 5;
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
                panelHome.Width += 5;
                panelOrders.Width += 5;
                panelInventory.Width += 5;
                panelExport.Width += 5;
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


        // +++++++++++++++++++++++++++ ORDER TAB +++++++++++++++++++++++++++++++

        //Init
        private void LoadOrderHeaderListView()
        {
            allOrders = null;
            allOrders = new OrderList();
            lvOrderHeaders.ItemsSource = allOrders;
           LoadComboBoxSearchItem();


        }
        private void LoadItemListView()
        {
            allItems = null;
            allItems = new OrderHeader();
            OrderHeader order = (OrderHeader)lvOrderHeaders.SelectedItem;
            lvItems.ItemsSource = order;
        }


        private void LoadComboBoxSearchItem()
        {
            cboSearchItem.Items.Clear();
            try
            {
                StockItemList stockItems = new StockItemList();
                stockItems.Sort();
                foreach (var row in stockItems)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = row.Name;
                    item.Tag = row.Item_ID;
                    cboSearchItem.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void lvOrderHeaders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadItemListView();
        }

        private string OrderItemsToString()
        {
            OrderHeader oh = (OrderHeader)lvOrderHeaders.SelectedItem;
            StockItemList allStockItems = new StockItemList();
            if(oh != null)
            {

            
                DataTable dtAllOrders = oh.GetAllOrderItems();
                string orderDetails = String.Empty;
                if (oh.OrderItems != null)
                {
                    foreach (DataRow row in dtAllOrders.Rows)
                    {
                        if ((int)row["OrderHeaderId"] == oh.ID)
                        {
                        
                                foreach (StockItem item in allStockItems)
                                {
                                    if (item.Item_ID == (int)row["StockItemId"])
                                    {
                                        orderDetails += $"Item: {item.Name}\n";
                                    }
                                }
                            orderDetails += $"Quantity: {row["Quantity"]}\n" +
                                            $"Description: {row["Description"]}\n\n";
                        }
                    }
                }
            return orderDetails;
            }
            return "Nothing added";
        }

        private void btnOrderAdd_Click(object sender, RoutedEventArgs e)
        {
            OrderHeader oh = new OrderHeader(1, DateTime.Now);
            oh.AddNewOrder();
            LoadOrderHeaderListView();
            lvOrderHeaders.SelectedIndex = 0;
            lvOrderHeaders.ScrollIntoView(lvOrderHeaders.SelectedIndex);
        }
        private void btnOrderDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrderHeaders.SelectedItem != null)
            {
                string message = $"Are you sure yo want to delete this Order? \n" +
                                    $"The Order Details and all Order items will be permanently deleted!";
                string caption = "Delete Order?";
                if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                    MessageBoxResult.Yes)
                {
                    OrderHeader selectedOrder = (OrderHeader)lvOrderHeaders.SelectedItem;
                    try
                    {
                        if (selectedOrder.DeleteOrder(selectedOrder.ID) == 1)
                        {
                            MessageBox.Show("Order successfully deleted!", "Success", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            LoadOrderHeaderListView();
                            lvOrderHeaders.SelectedIndex = 0;
                            lvOrderHeaders.ScrollIntoView(lvOrderHeaders.SelectedIndex);
                        }
                    }
                    catch (Exception ex)
                    {
                        message = $"Something went wrong!\nThe Order details were not deleted!\n{ex.Message}";
                        MessageBox.Show(message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Order was not deleted", "Operation canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select an Order to be deleted first.");
            }
        }

        private void btnOrderAddItem_Click(object sender, RoutedEventArgs e)
        {
        }
        private void btnOrderDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (lvItems.SelectedItem != null)
            {
                string message = $"Are you sure yo want to delete this Item from the order? \n" +
                                    $"This item will be permanently deleted!";
                string caption = "Delete Item?";
                if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                    MessageBoxResult.Yes)
                {
                    OrderItem item = (OrderItem)lvItems.SelectedItem;
                    try
                    {
                        if (item.DeleteOrderItem() == 1)
                        {
                            MessageBox.Show("Item successfully deleted!", "Success", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            LoadItemListView();
                            lvItems.SelectedIndex = 0;
                            lvItems.ScrollIntoView(lvItems.SelectedIndex);
                        }
                    }
                    catch (Exception ex)
                    {
                        message = $"Something went wrong!\nThe Item details were not deleted!\n{ex.Message}";
                        MessageBox.Show(message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Item was not deleted", "Operation canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select an Item to be deleted first.");
            }
           
        }
    }
}
