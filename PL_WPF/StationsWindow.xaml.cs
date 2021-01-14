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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLApi;
using BO;
namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for StationsWindow.xaml
    /// </summary>
    public partial class StationsWindow : Page
    {
        IBL bl;
        List<BusStation> staList;
        public StationsWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            staList = (from number in bl.GetAllStations()
                       select number).ToList();
            list.ItemsSource = staList;

        }

        private void list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.BusStation lsta = (BO.BusStation)list.SelectedItem;
            if (lsta != null)
            {

                StationDetails win = new StationDetails(bl, lsta.LineInStation);
                win.Show();
            }
        }
        private void AddStation_click(object sender, RoutedEventArgs e)
        {
            AddStation win = new AddStation(bl, list);
            win.Show();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.DataContext is BusStation)
            {
                BusStation sta = (BusStation)btn.DataContext;
                bl.RemoveSta(sta);
                staList = (from number in bl.GetAllStations()
                           select number).ToList();
                list.ItemsSource = staList;
            }
        }

        private void UpdateTwoStations(object sender, RoutedEventArgs e)
        {
            UpdatePairWin win = new UpdatePairWin(bl);
            win.Show();
        }
    }
}
