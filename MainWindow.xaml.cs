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
using System.Text.RegularExpressions;

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
            LoadInventoryListBox();

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


        // +++++++++++++++++++++ MENU +++++++++++++++++++++++++++++++++++++++++
        //Init
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

        //Buttons
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
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




        // +++++++++++++++++++++++++++ HOME TAB +++++++++++++++++++++++++++++++
        private void lblNewOrder_MouseEnter(object sender, MouseEventArgs e)
        {
            lblNewOrder.Foreground = new SolidColorBrush(Color.FromRgb(236, 109, 16));
        }
        private void lblNewOrder_MouseLeave(object sender, MouseEventArgs e)
        {
            lblNewOrder.Foreground = new SolidColorBrush(Color.FromRgb(215, 200, 24));
        }
        private void lblAddItem_MouseEnter(object sender, MouseEventArgs e)
        {
            lblAddItem.Foreground = new SolidColorBrush(Color.FromRgb(236, 109, 16));
        }
        private void lblAddItem_MouseLeave(object sender, MouseEventArgs e)
        {
            lblAddItem.Foreground = new SolidColorBrush(Color.FromRgb(215, 200, 24));
        }
        private void lblLoadInvList_MouseEnter(object sender, MouseEventArgs e)
        {
            lblLoadInvList.Foreground = new SolidColorBrush(Color.FromRgb(236, 109, 16));
        }
        private void lblLoadInvList_MouseLeave(object sender, MouseEventArgs e)
        {
            lblLoadInvList.Foreground = new SolidColorBrush(Color.FromRgb(215, 200, 24));
        }
        private void lblHelp_MouseEnter(object sender, MouseEventArgs e)
        {
            lblHelp.Foreground = new SolidColorBrush(Color.FromRgb(236, 109, 16));
        }
        private void lblHelp_MouseLeave(object sender, MouseEventArgs e)
        {
            lblHelp.Foreground = new SolidColorBrush(Color.FromRgb(215, 200, 24));
        }
        private void lblNewOrder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            panelHome.Visibility = Visibility.Hidden;
            panelOrders.Visibility = Visibility.Visible;
            panelInventory.Visibility = Visibility.Hidden;
            panelExport.Visibility = Visibility.Hidden;
            btnOrdersMenu.Focus();
        }
        private void lblAddItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            panelHome.Visibility = Visibility.Hidden;
            panelOrders.Visibility = Visibility.Hidden;
            panelInventory.Visibility = Visibility.Visible;
            panelExport.Visibility = Visibility.Hidden;
        }
        private void lblLoadInvList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            panelHome.Visibility = Visibility.Hidden;
            panelOrders.Visibility = Visibility.Hidden;
            panelInventory.Visibility = Visibility.Visible;
            panelExport.Visibility = Visibility.Hidden;
        }

        private void lblHelp_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++











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
            lblInventoryAmount.Content = "";
        }

        //Buttons
        private void btnOrderAdd_Click(object sender, RoutedEventArgs e)
        {
            OrderHeader oh = new OrderHeader(1, DateTime.Now);
            oh.AddNewOrder();
            LoadOrderHeaderListView();
            lvOrderHeaders.SelectedIndex = 0;
            lvOrderHeaders.ScrollIntoView(lvOrderHeaders.SelectedIndex);
        }
        private void btnOrderAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                StockItem addedStockItem = new StockItem();
                int orderIndex = lvOrderHeaders.SelectedIndex;
                OrderHeader order = (OrderHeader)lvOrderHeaders.SelectedItem;
                OrderItem orderItem = new OrderItem();
                StockItemList stockItems = new StockItemList();
                foreach (var item in stockItems)
                {
                    if (item.Name == cboSearchItem.Text)
                    {
                        orderItem.Item_ID = item.Item_ID;
                        orderItem.Price = item.Price;
                        addedStockItem = item;
                    }
                }
                orderItem.Order_ID = order.ID;
                orderItem.Description = txtDescription.Text;
                orderItem.Quantity = int.Parse(txtQuantity.Text);
                if(orderItem.Quantity <= addedStockItem.InStock)
                {
                    addedStockItem.InStock -= orderItem.Quantity;
                    addedStockItem.Update();
                    orderItem.AddItemToOrder();
                }
                else
                {
                    MessageBox.Show($"Could not add {addedStockItem.Name} to order. Stock insufficient", "Not enough items in Stock", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                txtQuantity.Clear();
                txtDescription.Clear();

                LoadOrderHeaderListView();
                lvOrderHeaders.SelectedIndex = orderIndex;
                lvOrderHeaders.ScrollIntoView(orderIndex);
                lvItems.SelectedIndex = 0;
                lvItems.ScrollIntoView(lvItems.SelectedIndex);
                LoadItemListView();
            }catch(Exception)
            {
                MessageBox.Show($"Could not add item to order! Item has already been added", "Duplicate Item", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnOrderDelete_Click(object sender, RoutedEventArgs e)
        {
            
            if (lvOrderHeaders.SelectedItem != null)
            {
                
                OrderHeader selectedOrder = (OrderHeader)lvOrderHeaders.SelectedItem;
                if (selectedOrder.Count > 0)
                {
                    MessageBox.Show("Please delete all items before deleting the order", "Order not empty", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string message = $"Are you sure yo want to delete this Order? \n" +
                                        $"The Order Details will be permanently deleted!";
                    string caption = "Delete Order?";
                    if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                        MessageBoxResult.Yes)
                    {
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
            }
            else
            {
                MessageBox.Show("Please select an Order to be deleted first.");
            }
        }
        private void btnOrderDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (lvItems.SelectedItem != null)
            {
                int orderIndex = lvOrderHeaders.SelectedIndex;
                string message = $"Are you sure yo want to delete this Item from the order? \n" +
                                    $"This item will be permanently deleted!";
                string caption = "Delete Item?";
                if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                    MessageBoxResult.Yes)
                {
                    OrderItem item = (OrderItem)lvItems.SelectedItem;
                    try
                    {
                        StockItemList stockItems = new StockItemList();
                        StockItem deletedStock = new StockItem();
                        foreach(var stockItem in stockItems)
                        {
                            if (stockItem.Item_ID == item.Item_ID) 
                                deletedStock = stockItem;
                        }
                        int deletedQuantity = item.Quantity;
                        if (item.DeleteOrderItem() == 1)
                        {
                            deletedStock.InStock += deletedQuantity;
                            deletedStock.Update();
                            MessageBox.Show("Item successfully deleted!", "Success", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                            LoadOrderHeaderListView();
                            lvOrderHeaders.SelectedIndex = orderIndex;
                            lvOrderHeaders.ScrollIntoView(orderIndex);
                            lvItems.SelectedIndex = 0;
                            lvItems.ScrollIntoView(lvItems.SelectedIndex);
                            LoadItemListView();
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

        //Helpers
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
        private string OrderItemsToString()
        {
            OrderHeader oh = (OrderHeader)lvOrderHeaders.SelectedItem;
            StockItemList allStockItems = new StockItemList();
            if (oh != null)
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
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = regex.IsMatch(e.Text);
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }
        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 1 && i <= 99999;
        }
        private void lvOrderHeaders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadItemListView();
        }
        private void cboSearchItem_TextChanged(object sender, EventArgs e)
        {
            StockItemList allStockItems = new StockItemList();
            foreach(StockItem item in allStockItems)
            {
                if(item.Name == cboSearchItem.Text)
                {
                    lblInventoryAmount.Content = $"({item.InStock} In Stock)";
                }
            }
        }
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++








        // +++++++++++++++++++++++++ INVENTORY TAB ++++++++++++++++++++++++++++++++
        //Init
        public void LoadInventoryListBox()
        {
            StockItemList allStockItems = new StockItemList();
            lvInventoryList.ItemsSource = allStockItems;
        }

        //Buttons
        private void btnSearchStockItem_Click(object sender, RoutedEventArgs e)
        {
            StockItemList allItems = new StockItemList();
            List<StockItem> searchedItems = new List<StockItem>();
            foreach(var item in allItems)
            {
                if (item.Name.ToUpper().Contains(txtInvSearch.Text.ToUpper()))
                {
                    searchedItems.Add(item);
                }
            }
            lvInventoryList.ItemsSource = searchedItems;
        }
        private void btnStockItemAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StockItem item = new StockItem();
                item.Name = txtAddStockItemName.Text;
                item.Price = int.Parse(txtAddStockItemPrice.Text);
                item.InStock = int.Parse(txtAddStockItemInStock.Text);
                item.Add();
                LoadInventoryListBox();
                txtAddStockItemName.Clear();
                txtAddStockItemPrice.Clear();
                txtAddStockItemInStock.Clear();
            }
            catch (Exception ex) {
                MessageBox.Show($"Please fill in all fields to add an item\n Error: {ex.Message}", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }
        private void btnDeleteStockItem_Click(object sender, RoutedEventArgs e)
        {
            StockItemList stockItems = new StockItemList();
            StockItem itemToDelete = (StockItem)lvInventoryList.SelectedItem;
            if(MessageBox.Show($"Do you really want to delete {itemToDelete.Name}?", "Delete Item?", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {

                foreach (var item in stockItems)
                {
                    if (item.Item_ID == itemToDelete.Item_ID)
                    {
                        item.Delete();
                    }
                }
                LoadInventoryListBox();
                }catch(Exception ex)
                {
                    MessageBox.Show($"Something went wrong\nError: {ex.Message}", "Unable To Delete", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Helpers
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    }
}
