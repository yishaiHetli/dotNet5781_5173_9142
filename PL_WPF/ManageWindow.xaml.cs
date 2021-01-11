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
using BLApi;
using BL;
using System.Collections.ObjectModel;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for ManageWindow.xaml
    /// </summary>
    public partial class ManageWindow : Page
    {
        IBL bl;
        private MainWindow mainWindow;
        public ManageWindow(IBL _bL, MainWindow _mainWindow)
        {
            InitializeComponent();
            bl = _bL;
            mainWindow = _mainWindow;
        }
        private void Bus_Click(object sender, RoutedEventArgs e)
        {
            BusWindow win = new BusWindow(bl);
            frmMain.NavigationService.Navigate(win);
        }
        private void Line_Click(object sender, RoutedEventArgs e)
        {
            LineWindow win = new LineWindow(bl);
            frmMain.NavigationService.Navigate(win);
        }
        private void Station_Click(object sender, RoutedEventArgs e)
        {
            StationsWindow win = new StationsWindow(bl);
            frmMain.NavigationService.Navigate(win);
        }
      
        private void LogOut_click(object sender, RoutedEventArgs e)
        {
            mainWindow.GoBackToStartPage();
        }
    }
}
