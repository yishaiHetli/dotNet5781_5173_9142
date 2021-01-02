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
using BL;
using BO;
using System.Collections.ObjectModel;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        IBL bl = BLFactory.GetBL("1");
        public MainWindow()
        {
            InitializeComponent();
            //ObservableCollection<Bus> listOfBus;
            //listOfBus = new ObservableCollection<Bus>();
            //foreach (var item in bl.GetAllBuss())
            //    listOfBus.Add(item);
            //list.ItemsSource = listOfBus; 
        }

        private void btnGO_Click(object sender, RoutedEventArgs e)
        {
            if (rbDriver.IsChecked == true)
            {
                //DriverWindow winD = new DriverWindow(bl);
                //winD.Show();
                MessageBox.Show("This method is under construction!",
                    "TBD", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else if (rbManage.IsChecked == true)
            {
                ManageWindow winM = new ManageWindow(bl);
                winM.Show();
            }
        }
    }
   
}
