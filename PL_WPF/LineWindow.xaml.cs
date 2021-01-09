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
    public partial class LineWindow : Page
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
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BusLine lines = (BusLine)list.SelectedItem;
            if (lines != null)
            {

                LineStationDatails win = new LineStationDatails(bl, lines.LinesSta);
                win.Show();
            }
        }

        private void AddLine_click(object sender, RoutedEventArgs e)
        {
            AddLine win = new AddLine(bl,list);
            win.Show();
        }
    }
}
