using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BLApi;
using BO;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for BusDatails.xaml
    /// </summary>
    public partial class DatailsBus : Window
    {
        IBL bl;
        public Bus myBus { get; set; }
        ListBox list;
        List<Bus> listOfBuses;
        public DatailsBus(IBL _bl, Bus bus, ListBox _list, List<Bus> _listOfBuses)
        {
            bl = _bl;
            list = _list;
            listOfBuses = _listOfBuses;
            myBus = bus;
            InitializeComponent();
            myGrid.DataContext = this;
        }
        private void refule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.GetRefule(myBus);
                myBus = bl.GetBus(myBus.LicenseNum);
                listText.Text = myBus.ToString();
                listOfBuses = (from number in bl.GetAllBuss()
                               orderby number.LicenseNum
                               select number).ToList();
                list.ItemsSource = null;
                list.ItemsSource = listOfBuses;
            }
            catch (BO.BadLisenceException ex)
            { MessageBox.Show("error", ex.Message); }
        }
        private void repair_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.GetRepair(myBus);
                myBus = bl.GetBus(myBus.LicenseNum);
                listText.Text = myBus.ToString();
                listOfBuses = (from number in bl.GetAllBuss()
                               orderby number.LicenseNum
                               select number).ToList();
                list.ItemsSource = null;
                list.ItemsSource = listOfBuses;
            }
            catch (BO.BadLisenceException ex)
            { MessageBox.Show("error", ex.Message); }
        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
