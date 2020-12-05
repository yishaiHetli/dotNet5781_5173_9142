using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using MahApps.Metro.Controls;

namespace dotNet5781_03B_5173_9142
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private Bus bus;
        private ObservableCollection<Bus> buses = new ObservableCollection<Bus>();
        public Window2(Bus _bus)
        {
            bus = _bus;
            buses.Add(bus);
            bus.StrHelp = false;
            InitializeComponent();
            this.myGrid.Background = Brushes.BlueViolet;
            myText.ItemsSource = buses;
        }

        private void finish_click(object sender, RoutedEventArgs e)//Closing the window

        {
            this.Close();
        }

        private void Refule_Click(object sender, RoutedEventArgs e)
        {
            if (bus.FuelInKm < 1200)//check if the bus need a refuel
            {
                if (bus.BusStatus == Status.READY)//if the bus status is 'READY'
                {
                    bus.BusStatus = Status.REFUELING;//changing the bus status to 'REFUELING'
                    bus.condition = new Thread(() =>
                    {
                        MessageBox.Show("The bus is refuling and will not be available for 12 seconds");
                        Thread.Sleep(12000);//sleep for 12 seconds
                    });

                    Thread count = new Thread(() =>
                    {
                        int time = 12;//initialize the property 'time' according to the number of seconds per refule
                        while (time != 0)
                        {
                            bus.countDown = string.Format("{0:00}:{1:00}", time / 60, time % 60);//Displays the property 'time' in clock format
                            bus.Helper = false;//notify if  ther was change in the property countDown
                            Thread.Sleep(1000);//sleep for onw seconde
                            bus.Helper = false;//notify if  ther was change in the property countDown
                            time--;
                        }
                        bus.BusStatus = Status.READY;//changing the bus status to 'READY'
                        bus.FuelInKm = 1200;//reseting the fuel in the bus to 1200
                        bus.StrHelp = false; //notify if  ther was change in the property printer
                        bus.Helper = false;//notify if  ther was change in the property countDown
                        bus.countDown = string.Format(" ", time);//Hide the count down
                    });
                    count.Start();//starting the thread 'count'
                    bus.condition.Start();//starting the thread 'bus.condition'
                }
                else//if the bus is bussy 
                {
                    MessageBox.Show(String.Format($"this bus is {bus.BusStatus}"));
                }
            }
            else//if the bus dos not need a refuel
                MessageBox.Show("This bus allready has a full tank");
        }

        private void Repair_Click(object sender, RoutedEventArgs e)
        {
            if ((bus.TreatKms > 20000 || DateTime.Now > bus.TreatTime.AddYears(1)) || bus.FuelInKm < 1200)
            {
                if (bus.BusStatus == Status.READY)//if the bus status is 'READY'
                {
                    bus.BusStatus = Status.REPAIRING;//changing the bus status to 'REPAIRING'

                    Thread count = new Thread(() =>
                    {
                        int time = 144;//initialize the property 'time' according to the number of seconds per repair
                        while (time != 0)
                        {
                            bus.countDown = string.Format("{0:00}:{1:00}", time / 60, time % 60);//Displays the property 'time' in clock format
                            bus.Helper = false;//notify if  ther was change in the property countDown
                            Thread.Sleep(1000);//sleep for onw seconde
                            bus.Helper = false;//notify if  ther was change in the property countDown
                            time--;
                        }
                        bus.BusStatus = Status.READY;//changing the bus status to 'READY'
                        bus.TreatKms = 0;//changing the the kilometers the bus has traveled since its last repair
                        bus.TreatTime = DateTime.Now;//changing the the date of last repair to now  
                        bus.FuelInKm = 1200;//reseting the fuel in the bus to 1200
                        bus.StrHelp = false; //notify if  ther was change in the property printer
                        bus.Helper = false;//notify if  ther was change in the property countDown
                        bus.countDown = string.Format(" ", time);//Hide the count down
                    });
                    count.Start();//starting the thread 'count'
                    bus.condition = new Thread(() =>
                    {
                        MessageBox.Show("The bus is repairing and will not be available for 144 seconds");
                        Thread.Sleep(144000);
                    });
                    bus.condition.Start();//starting the thread 'bus.condition'
                }
                else//if the bus is bussy 
                {
                    MessageBox.Show(String.Format($"this bus is {bus.BusStatus}"));
                }
            }
            else//if the bus dos not need a repair
            {
                MessageBox.Show("This bus does not need a repair");
            }
        }
    }
}