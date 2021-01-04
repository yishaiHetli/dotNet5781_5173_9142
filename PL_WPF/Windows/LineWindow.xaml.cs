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
    /// Interaction logic for LineWindow.xaml
    /// </summary>
    public partial class LineWindow : Window
    {
        IBL bl;
        List<BusLine> listOfLines = new List<BusLine>();
        List<BusStation> listOfStation = new List<BusStation>();
        public LineWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            listOfLines = (from number in bl.GetAllLines()
                           orderby number.LineID
                           select number).ToList();
            list.ItemsSource = listOfLines;
            listOfStation = (from number in bl.GetAllStations()
                             orderby number.BusStationKey
                             select number).ToList();
            first.ItemsSource = listOfStation;
            last.ItemsSource = listOfStation;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
                   
        }

        private void list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BusLine bus = (BusLine)list.SelectedItem;
            if (bus != null)
            {
                LineStationDatails winLSD = new LineStationDatails(bl, bus.LinesSta);
                winLSD.Show();
            }
        }
    }
}
