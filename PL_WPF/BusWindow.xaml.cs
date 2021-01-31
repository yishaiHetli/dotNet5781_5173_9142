using BLApi;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BO;
using System.Text.RegularExpressions;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for BusWindow.xaml
    /// </summary>
    public partial class BusWindow : Page
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        IBL bl;
        List<Bus> listOfBus;
        /// <summary>
        /// the program shows the list of buses
        /// </summary>
        /// <param name="_bl">the performance of the BL</param>
        public BusWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            listOfBus = new List<Bus>();
            listOfBus = (from number in bl.GetAllBuss()
                         select number).ToList();
            list.ItemsSource = listOfBus;
          
        }
   
        private void list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Bus bus = (Bus)list.SelectedItem;
            if (bus != null)
            {
                DatailsBus winD = new DatailsBus(bl, bus, list, listOfBus);
                winD.Show();
            }
        }
        private void removeBus_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.DataContext is Bus)
            {
                Bus bus = (Bus)btn.DataContext;
                if (bus.BusStatus == Status.READY)
                {
                    bl.RemoveBus(bus.LicenseNum);
                    listOfBus = (from number in bl.GetAllBuss()
                                 select number).ToList();
                    list.ItemsSource = null;
                    list.ItemsSource = listOfBus;
                }
                else 
                {
                    MessageBox.Show($"this bus is {bus.BusStatus}");
                }
            }
        }
        private void AddBus_click(object sender, RoutedEventArgs e)
        {
            AddBus win = new AddBus(bl,list);
            win.Show();
        }

    }
}

