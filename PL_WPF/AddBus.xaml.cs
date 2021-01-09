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
    /// Interaction logic for AddBus.xaml
    /// </summary>
    public partial class AddBus : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private DateTime startTime;
        private int licenceNum = 0;
        private double fuelTime = 0;
        private double milage = 0;
        IBL bl;
        List<Bus> listOfBus;
        ListBox list;
        public AddBus(IBL _bl,ListBox _list)
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
            bool good = false;
            if (licens.Text != "" && IsTextAllowed(licens.Text))
            {
                int x = int.Parse(licens.Text);
                licenceNum = x;
            }
            else MessageBox.Show("The field 'License Plate' can't be empty");
            if (totalTrip.Text != "" && IsTextAllowed(totalTrip.Text)) // check if there is  a text and if is a number 
            {
                double x = double.Parse(totalTrip.Text); // convert to double
                if (x > 0)
                    milage = x; //set the milage 
            }
            if (fuel.Text != "" && IsTextAllowed(fuel.Text))// check if there is  a text and if is a number 
            {
                double x = double.Parse(fuel.Text); // convert the text to double
                if (x >= 0 && x <= 1200) // if is in between 0 to 1200
                    fuelTime = x;  // set fuel 
            }
            if (myDate.Text != "") //if there is a text
            {
                try
                {
                    DateTime x = new DateTime();
                    x = Convert.ToDateTime(myDate.Text); // convert to a date
                    if (licenceNum == 0 || x.Year >= 2018 && (licenceNum >= 10000000 && licenceNum < 100000000)
                   || x.Year < 2018 && (licenceNum < 10000000 && licenceNum > 1000000)) // if the user enter a licence and they are not match to this date
                    {
                        startTime = x;
                        if (x > DateTime.Now)
                        {
                            MessageBox.Show($"the date canot be grater than{DateTime.Now}");
                            myDate.Text = "";
                        }
                        else
                            good = true;
                    }
                    else
                        MessageBox.Show("lisence number does not match the year of manufacture");
                }
                catch // if is not a date
                {
                    MessageBox.Show("the value is not courect");
                }
            } // if there isn't a text
            else
                MessageBox.Show("the value is not courect");

            if (good)
            {
                Bus a = new Bus();
                a.LicenseNum = licenceNum;
                a.StartActivity = startTime;
                a.FuelInKm = fuelTime;
                a.TotalKm = milage;
                fuel.Text = "";
                licens.Text = "";
                totalTrip.Text = "";
                myDate.Text = "";
                bl.AddNewBus(a);
                listOfBus = (from number in bl.GetAllBuss()
                             orderby number.LicenseNum
                             select number).ToList();
                list.ItemsSource = listOfBus;
            }
        }
    }
}
