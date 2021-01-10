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
        private double fuelTime = -1;
        private double milage = -1;
        IBL bl;
        List<Bus> listOfBus;
        ListBox list;
        public AddBus(IBL _bl,ListBox _list)
        {
            InitializeComponent();
            bl = _bl;
          
            list = _list;

        }
        /// <summary>
        /// check if the text is a number
        /// </summary>
        /// <param name="text">the text entered by the user</param>
        /// <returns>return true if is a number</returns>
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        /// <summary>
        /// butten to create a new bus 
        /// </summary>
        private void createBusLine(object sender, RoutedEventArgs e)
        {
            // check if the user enter at least the start time and the licence plate
            if (startTime != DateTime.MinValue && licenceNum > 0)
            {
                if (milage < 0)
                {
                    milage = 0;
                }
                if (fuelTime < 0)
                {
                    fuelTime = 1200;
                }
                Bus a = new Bus();
                a.LicenseNum = licenceNum;
                a.StartActivity = startTime;
                a.FuelInKm = fuelTime;
                a.TotalKm = milage;

                try { bl.AddNewBus(a); }
                catch (BadLisenceException ex)
                {
                    MessageBox.Show($"{ex.Message}", "error");
                    next();
                    return;
                }
                listOfBus = (from number in bl.GetAllBuss()
                             select number).ToList();
                list.ItemsSource = listOfBus;
                this.Close();
            }
            else { MessageBox.Show("enter at least start activity \ndate and lisence plate courect"); next(); }
        }

        /// <summary>
        /// get the start activity date
        /// </summary>
        private void myDate_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//if the user press on enter
            {
                if (myDate != null) //if there is a text
                {
                    try
                    {
                        DateTime x = new DateTime();
                        x = Convert.ToDateTime(myDate.Text); // convert to a date
                            startTime = x;
                            next(); // set the focus by some order
                    }
                    catch // if is not a date
                    {
                        MessageBox.Show("the value is not courect");
                    }
                } // if there isn't a text
                else { MessageBox.Show("the value is not courect"); }
            }
        }
        /// <summary>
        // get the milage 
        private void milages_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // if the user press enter
            {
                if (milages != null && IsTextAllowed(milages.Text)) // check if there is  a text and if is a number 
                {
                    double x = double.Parse(milages.Text); // convert to double
                    if (x > 0)
                    {
                        milage = x;//set the milage 
                        next(); // set the focus by some order
                    }
                    else { MessageBox.Show("the value is not courect"); } // if is small than 0
                }
                else { MessageBox.Show("the value is not courect"); } // if is not a number
            }
        }
        /// <summary>
        // get the fuel
        private void fuels_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (fuels != null && IsTextAllowed(fuels.Text))// check if there is  a text and if is a number 
                {
                    double x = double.Parse(fuels.Text); // convert the text to double
                    if (x >= 0 && x <= 1200) // if is in between 0 to 1200
                    {
                        fuelTime = x;// set fuel
                        next(); // set the focus by some order
                    }
                    else { MessageBox.Show("the value is not courect"); } // if is not in the range 
                }
                else { MessageBox.Show("the value is not courect"); } // if isn't a number
            }
        }
        /// <summary>
        // get the lisence 
        private void lisence_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // if the user press enter
            {
                if (lisence != null && IsTextAllowed(lisence.Text))// check if there is  a text and if is a number 
                {
                    int x = int.Parse(lisence.Text); // convert the text to double
                        licenceNum = x;
                        next(); // set the focus by some order
                }
                else { MessageBox.Show("the value is not courect"); } // if isn't a number
            }
        }
        /// <summary>
        /// to add a date from DatePicker  
        /// </summary>
        private void myDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = myDate.SelectedDate;
            if (selectedDate.HasValue)
            {
                DateTime x = new DateTime();
                x = Convert.ToDateTime(selectedDate.Value); // convert to a date
                startTime = x;
                next(); // set the focus by some order
            }
        }

        void next()
        {
            if (startTime == DateTime.MinValue)
            {
                myDate.Focus();
            }
            else if (licenceNum == 0)
            {
                lisence.Focus();
            }
            else if (fuelTime == -1)
            {
                fuels.Focus();
            }
            else if (milage == -1)
            {
                milages.Focus();
            }
            else if (startTime != DateTime.MinValue && licenceNum != 0 && milage >= 0 && fuelTime >= 0)
                createBus.Focus();
        }
    }
}
//if (good)
//{

//}
