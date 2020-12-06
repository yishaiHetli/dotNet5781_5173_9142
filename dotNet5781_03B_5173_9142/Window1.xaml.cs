using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dotNet5781_03B_5173_9142
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); 
        private ObservableCollection<Bus> _ppList;

        private DateTime startTime;
        private int licenceNum = 0;
        private double fuelTime = 0;
        private double milage = 0;

        public Window1(ObservableCollection<Bus> ppList)
        {
            _ppList = ppList; // get the list of bus from the main window
            InitializeComponent();
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
            if (startTime != DateTime.MinValue && licenceNum != 0) 
            {
                _ppList.Add(new Bus(licenceNum, startTime, milage, fuelTime));// add to the list from main this new bus
                this.Close();
            }
            else { MessageBox.Show("enter at least start activity \ndate and lisence plate courect"); }
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
                        if (licenceNum == 0 || x.Year >= 2018 && (licenceNum >= 10000000 && licenceNum < 100000000)
                       || x.Year < 2018 && (licenceNum < 10000000 && licenceNum > 1000000)) // if the user enter a licence and they are not match to this date
                        {
                            startTime = x;
                        }
                        else { MessageBox.Show("lisence number does not match the year of manufacture"); }
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
                    { milage = x; } //set the milage 
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
                    { fuelTime = x; } // set fuel 
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
                    if (startTime != DateTime.MinValue && (startTime.Year >= 2018 && (x >= 10000000 && x < 100000000)
                        || startTime.Year < 2018 && (x < 10000000 && x > 1000000))) // if the lisence match to start activity date 
                    {
                        licenceNum = x;
                    }
                    else if (startTime == DateTime.MinValue && x >= 1000000 && x < 100000000) // if date isn't initialized and lisence is between 7-8 digits
                    {
                        licenceNum = x;//set lisence
                    }
                    else { MessageBox.Show("lisence number does not match the year of manufacture"); } 
                }
                else { MessageBox.Show("the value is not courect"); } // if isn't a number
            }
        }
    }
}
