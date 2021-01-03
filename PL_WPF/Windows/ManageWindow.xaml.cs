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
    public partial class ManageWindow : Window
    {

        
        IBL bl;
        public ManageWindow(IBL _bL)
        {
            InitializeComponent();
            bl = _bL;
          
        }

        private void Bus_Click(object sender, RoutedEventArgs e)
        {
            BusWindow win = new BusWindow(bl);
            win.Show();
        }

        private void Line_Click(object sender, RoutedEventArgs e)
        {
            LineWindow win = new LineWindow(bl);
            win.Show();
        }

        private void Station_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
