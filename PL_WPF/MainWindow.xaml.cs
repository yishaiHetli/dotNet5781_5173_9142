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
        private object content;
        public MainWindow()
        {
            InitializeComponent();
            content = Content;
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
                if (userName.Text == string.Empty)
                {
                    userName.Focus();
                }
                else if (password.Password == string.Empty)
                {
                    password.Focus();
                }
                else
                {
                    if (bl.userCheck(userName.Text, password.Password, true))
                    {
                        userName.Text = "";
                        password.Password = "";
                        ManageWindow winM = new ManageWindow(bl,this);
                        this.Content = winM.Content;

                    }
                    else
                    {
                        MessageBox.Show("username or password incorrect", "error");
                        userName.Focus();
                    }
                }
            }

        }
        public void GoBackToStartPage()
        {
            Content = content;
        }
    }
}
