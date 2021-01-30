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
    /// Interaction logic for AddBus.xaml
    /// </summary>
    public partial class AddLine : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        IBL bl;
        List<BusLine> listOfLines;
        List<BusStation> listOfStation;
        ListBox boxList;
        public AddLine(IBL _bl, ListBox _boxList)
        {
            InitializeComponent();
            bl = _bl;
            listOfStation = (from number in bl.GetAllStations()
                             select number).ToList();
            first.ItemsSource = listOfStation;
            last.ItemsSource = listOfStation;
            boxList = _boxList;
        }
        private BusLine bus = new BusLine();
        private BusStation station = new BusStation();
        bool one = false, two = false, three = false;
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (lineNum.Text != "" && IsTextAllowed(lineNum.Text))
            {
                if (one && two && three)
                {
                    bus.LineNumber = Convert.ToInt32(lineNum.Text);
                    bl.AddNewBusLine(bus);
                }
                if (!one)
                    MessageBox.Show("first station is not valid");
                if (!two)
                    MessageBox.Show("lasr station is not valid");
                if (!three)
                    MessageBox.Show("area station is not valid");

                listOfLines = (from number in bl.GetAllLines()
                               select number).ToList();
                boxList.ItemsSource = listOfLines;
                this.Close();
            }
            else
                MessageBox.Show("line number not valid");
        }

        private void first_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            station = (BusStation)first.SelectedItem;
            bus.FirstStation = station.BusStationKey;
            one = true;
        }

        private void last_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            station = (BusStation)last.SelectedItem;
            bus.LastStation = station.BusStationKey;
            two = true;
        }

        private void area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Area a = (Area)area.SelectedIndex;
            bus.Place = a;
            three = true;
        }
    }
}