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
        List<BusLine> listOfLines;
        List<BusStation> listOfStation;

        public LineWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            listOfLines = new List<BusLine>();
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
    }
}
