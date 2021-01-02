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
using System.Text.RegularExpressions;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for AddBus.xaml
    /// </summary>
    public partial class AddBus : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        IBL bl;
        private DateTime startTime;
        private int licenceNum = 0;
        private double fuelTime = -1;
        private double milage = -1;
        ListBox list;
        List<Bus> listOfBuses;
        public AddBus(IBL _bl,List<Bus> _listOfBuses,ListBox _list)
        {
            bl = _bl;
            list = _list;
            listOfBuses = _listOfBuses;
            InitializeComponent();
            myDate.Focus();
        }
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void createBusLine(object sender, RoutedEventArgs e)
        {
            // check if the user enter at least the start time and the licence plate
            if (startTime != DateTime.MinValue && licenceNum != 0)
            {
                Bus bus = (new Bus
                {
                    LicenseNum = licenceNum,
                    StartActivity = startTime,
                    BusStatus = Status.READY,
                    FuelInKm = fuelTime,
                    TotalKm = milage
                });
                try
                {
                    bl.AddNewBus(bus);
                    listOfBuses = (from number in bl.GetAllBuss()
                                 orderby number.LicenseNum
                                 select number).ToList();
                    list.ItemsSource = null;
                    list.ItemsSource = listOfBuses;
                    this.Close();
                }
                catch (BO.BadLisenceException ex)
                { MessageBox.Show(ex.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error); next(); }
            }
            else { MessageBox.Show("enter at least start activity \ndate and lisence plate courect"); next(); }
        }
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
                        if (x > DateTime.Now)
                            MessageBox.Show($"the date canot be grater than{DateTime.Now}");
                        else
                        {
                            startTime = x;
                            next(); // set the focus by some order
                        }
                    }
                    catch // if is not a date
                    {
                        MessageBox.Show("the value is not courect");
                    }
                } // if there isn't a text
                else { MessageBox.Show("the value is not courect"); }
            }
        }
        private void milages_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (milages != null && !IsTextAllowed(milages.Text))
            {
                milages.Text = string.Empty;
            }
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
        private void fuels_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (fuels != null && !IsTextAllowed(fuels.Text))
            {
                fuels.Text = string.Empty;
            }
            if (e.Key == Key.Enter)
            {
                if (fuels != null && IsTextAllowed(fuels.Text))// check if there is  a text and if is a number 
                {
                    double x = double.Parse(fuels.Text); // convert the text to double
                    fuelTime = x;// set fuel
                    next(); // set the focus by some order
                }   
            }
        }
        private void lisence_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (lisence != null && !IsTextAllowed(lisence.Text))
            {
                lisence.Text = string.Empty;
            }
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
        private void myDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = myDate.SelectedDate;
            if (selectedDate.HasValue)
            {
                DateTime x = new DateTime();
                x = Convert.ToDateTime(selectedDate.Value); // convert to a date
                if (x > DateTime.Now)
                    MessageBox.Show($"the date canot be grater than{DateTime.Now}");
                else
                {
                    startTime = x;
                    next(); // set the focus by some order
                }
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
