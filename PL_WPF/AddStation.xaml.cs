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
    /// Interaction logic for AddStation.xaml
    /// </summary>
    public partial class AddStation : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        IBL bl;
        BusStation station = new BusStation();
        ListBox list;
        /// <summary>
        /// the program get a parameters for a new station and if thay 
        /// are corect adding the station and update the list of stations accordingly
        /// </summary>
        /// <param name="_bl">the performance of the BL</param>
        /// <param name="_list">the list of the stations</param>
        public AddStation(IBL _bl, ListBox _list)
        {
            InitializeComponent();
            bl = _bl;
            list = _list;
        }
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            bool one = false, two = false, three = false, four = false;
            if (code.Text != "" && IsTextAllowed(code.Text))
            {
                station.BusStationKey = int.Parse(code.Text);
                one = true;
            }
            else
                MessageBox.Show("the code can't be empty");
            if (name.Text != "")
            {
                station.Name = name.Text;
                two = true;
            }
            else
                MessageBox.Show("the name can't be empty");
            if (latitude.Text != "" && IsTextAllowed(latitude.Text))
            {
                station.Latitude = double.Parse(latitude.Text);
                three = true;
            }
            else
                MessageBox.Show("the latitude can't be empty");
            if (longtitude.Text != "" && IsTextAllowed(longtitude.Text))
            {
                station.Longitude = double.Parse(longtitude.Text);
                four = true;
            }
            else
                MessageBox.Show("the longtitude can't be empty");
            if (one && two && three && four)
            {
                try
                {
                    bl.AddBusStation(station);
                    List<BusStation> listOfStation = (from number in bl.GetAllStations()
                                                      orderby number.BusStationKey
                                                      select number).ToList(); ;
                    list.ItemsSource = listOfStation;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.Close();
            }
        }
    }
}
