using BLApi;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using BO;
namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for BusWindow.xaml
    /// </summary>
    public partial class BusWindow : Window
    {
        IBL bl;
        public List<Bus> listOfBus { get; set; }
        public BusWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            listOfBus = new List<Bus>();
            listOfBus = (from number in bl.GetAllBuss()
                         orderby number.LicenseNum
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
                bl.RemoveBus(bus.LicenseNum);
                listOfBus = (from number in bl.GetAllBuss()
                             orderby number.LicenseNum
                             select number).ToList();
                list.ItemsSource = null;
                list.ItemsSource = listOfBus;
            }
        }

        private void AddNewBus_Click(object sender, RoutedEventArgs e)
        {
            AddBus winA = new AddBus(bl,listOfBus,list);
            winA.Show();
        }
    }
}

