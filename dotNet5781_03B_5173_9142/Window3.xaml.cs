using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace dotNet5781_03B_5173_9142
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        private Bus bus;
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        public Window3(Bus _buss)
        {
            bus = _buss;
            InitializeComponent();
            drive.Focus();
        }

        /// <summary>
        /// check if the output is only digit
        /// </summary>
        /// <param name="text">text of string</param>
        /// <returns></returns>
        private static bool IsTextAllowed(string text)//check if input is a number
        {
            return !_regex.IsMatch(text);
        }
        /// <summary>
        /// the user put the distance in the text box and by enter the 
        /// func check if the bus can go in this travel 
        /// </summary>
        private void drive_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//if user put enter
            {
                if (drive != null && IsTextAllowed(drive.Text))
                {
                    double x = double.Parse(drive.Text);//Converts the input if it is not a double
                    if (x > 0)
                    {
                        if (x + bus.TreatKms > 20000)
                            MessageBox.Show("this bus can not do this travel\n because of high milage");
                        else if (bus.FuelInKm < x)//if the amount of fuel isn't enough
                            MessageBox.Show("this bus dont have enough fuel");

                        else
                        {
                            if (bus.BusStatus == Status.READY)//if the bus status is 'READY'
                            {
                                ended.Focus();
                                double drives = x;//double 'x' to be used for the input of kilometers of the travel
                                bus.BusStatus = Status.TRAVELING;//changing the bus status to 'TRAVELING'
                                Random r = new Random();//a random number to be used as the travel speed
                                x /= r.Next(20, 51);//random travel speed between 20 and 50
                                x *= 6; // every hour is 6 second
                                bus.condition = new Thread(() =>
                                {
                                    MessageBox.Show($"The bus is travling and will not be available for {Convert.ToInt32(x)} second");
                                    x *= 1000; 
                                    Thread.Sleep(Convert.ToInt32(x));
                                });
                                Thread count = new Thread(() =>
                                {
                                    //initialize the property 'time' according to the number of seconds needed for the travel
                                    int time = Convert.ToInt32(x);
                                    while (time != 0)
                                    {
                                        bus.countDown = string.Format("{0:00}:{1:00}", time / 60, time % 60);//Displays the property 'time' in clock format
                                        bus.Helper = false;//notify if  ther was change in the property countDown
                                        Thread.Sleep(1000);//sleep for onw seconde
                                        bus.Helper = false;//notify if  ther was change in the property countDown
                                        time--;
                                    }
                                    bus.TreatKms += drives;//changing the the kilometers the bus has traveled since its last repair
                                    bus.TotalKm += drives;//addng the current drive to the total kilometers the bus has traveled
                                    bus.BusStatus = Status.READY;//changing the bus status to 'READY'
                                    bus.FuelInKm -= drives;// subtract this drive fuel time
                                    bus.StrHelp = false; //notify if  ther was change in the property printer
                                    bus.Helper = false;//notify if  ther was change in the property countDown
                                    bus.countDown = string.Format(" ", time);//Hide the count down
                                });
                                count.Start();//starting the thread 'count'
                                bus.condition.Start();//starting the thread 'bus.condition'
                            }
                            else//if the bus is bussy 
                                MessageBox.Show(String.Format($"this bus is {bus.BusStatus}"));
                        }
                    }
                    else//if the value is not courect
                        MessageBox.Show("the value is not courect");
                }
                else//if the value is not courect
                    MessageBox.Show("the value is not courect");
            }
        }

        private void ended_Click(object sender, RoutedEventArgs e) // window closer click
        {
            this.Close();
        }
    }
}