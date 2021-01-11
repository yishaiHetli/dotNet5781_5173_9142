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
            listOfLines = (from number in bl.GetAllLines()
                           select number).ToList();
            list.ItemsSource = listOfLines;
            listOfStation = (from number in bl.GetAllStations()
                             select number).ToList();
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
        private void AddStop_click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.DataContext is BusLine)
            {
                BusLine bus = (BusLine)btn.DataContext;
                AddStop win = new AddStop(bl, bus, list);
                win.Show();
            }

        }
        private void AddLine_click(object sender, RoutedEventArgs e)
        {
            AddLine win = new AddLine(bl, list);
            win.Show();
        }

        private void Remove_click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.DataContext is BusLine)
            {
                BusLine bus = (BusLine)btn.DataContext;
                bl.RemoveLine(bus);
                listOfLines = (from number in bl.GetAllLines()
                               select number).ToList();
                list.ItemsSource = listOfLines;
            }
        }
    }
}
