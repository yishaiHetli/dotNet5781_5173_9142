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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for AddStop.xaml
    /// </summary>
    public partial class AddStop : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        List<BusStation> listOfStation;
        BusLine bus1 = new BusLine();
        BusStation sta = new BusStation();
        IBL bl;
        ListBox list;
        public AddStop(IBL _bl, BusLine bus, ListBox _list)
        {

            InitializeComponent();
            bl = _bl;
            listOfStation = (from number in bl.GetAllStations()
                             select number).ToList();
            station.ItemsSource = listOfStation;
            bus1 = bus;
            list = _list;
        }
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void station_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (station.SelectedItem != null)
                sta = station.SelectedItem as BusStation;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (lineNum.Text != string.Empty && IsTextAllowed(lineNum.Text) && sta != null)
            {
                try
                {
                    bl.AddStop(Convert.ToInt32(lineNum.Text), bus1, sta);
                    list.ItemsSource = (from number in bl.GetAllLines()
                                        select number).ToList();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("not valid index");
        }
    }
}
