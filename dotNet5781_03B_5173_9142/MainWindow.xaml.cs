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
using System.Threading;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls;
using System.Windows.Threading;

namespace dotNet5781_03B_5173_9142
{

    public partial class MainWindow : Window
    {
        //creating an observable collection 
        public ObservableCollection<Bus> company = new ObservableCollection<Bus>();
        public MainWindow()
        {
            InitializeComponent();
            this.myGrid.Background = Brushes.Gray;
            company.Add(new Bus(60251537, new DateTime(2020, 6, 11), 0, 1200));
            company.Add(new Bus(31442230, new DateTime(2020, 3, 13), 100, 1100));
            company.Add(new Bus(5934862, new DateTime(2003, 11, 4), 310000, 250));
            company.Add(new Bus(60251579, new DateTime(2019, 10, 24), 15000, 780));
            company.Add(new Bus(1538752, new DateTime(2010, 9, 15), 150000, 900));
            company.Add(new Bus(03258743, new DateTime(2018, 12, 21), 25000, 500));
            company.Add(new Bus(6448456, new DateTime(2015, 7, 1), 100000, 900));
            company.Add(new Bus(3184684, new DateTime(2012, 8, 19), 120000));
            company.Add(new Bus(26772403, new DateTime(2019, 10, 24), 15000, 1200));
            company.Add(new Bus(2437043, new DateTime(2017, 11, 27), 50000, 1200));
            list.ItemsSource = company;
        }
        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            Window1 secondWindow = new Window1(company);//creating a new window 1
            secondWindow.Show();//opening window 1
        }
        private void list_Statue_Click(object sender, MouseButtonEventArgs e)
        {
            Bus bus = (Bus)list.SelectedItem;//creating a bus with the values of the bus that was pressed 

            if (bus != null)//if the bus that was created has real values
            {
                Window2 secondWindow = new Window2(bus);//creating a new window 2
                secondWindow.Show();//opening window 2
            }
        }
        private void Refule_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.DataContext is Bus)
            {
                Bus bus = (Bus)btn.DataContext;//creating a bus with the values of the bus that his button was pressed 
                if (bus.FuelInKm < 1200)//check if the bus need a refuel
                {

                    if (bus.BusStatus == Status.READY)//if the bus status is 'READY'
                    {
                        bus.BusStatus = Status.REFUELING;//changing the bus status to 'REFUELING'
                        Thread count = new Thread(() =>
                        {
                            int time = 12;//initialize the property 'time' according to the number of seconds per refule
                            while (time != 0)
                            {
                                bus.countDown = string.Format(" ", time);//reset the clock
                                bus.countDown = string.Format("{0:00}:{1:00}", time / 60, time % 60);//Displays the property 'time' in clock format
                                bus.Helper = false; // halp the countdown to update 
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
                        bus.condition = new Thread(() =>
                        {
                            MessageBox.Show("The bus is refuling and will not be available for 12 seconds");
                            Thread.Sleep(12000);
                        });
                        bus.condition.Start();//starting the thread 'bus.condition'
                    }
                    else//if the bus is bussy 
                        MessageBox.Show(String.Format($"this bus is {bus.BusStatus}"));
                }
                else//if the bus dos not need a refuel
                    MessageBox.Show("This bus allready has a full tank");
            }
        }
        private void Drive_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender; //convert the sender to button  
            if (btn.DataContext is Bus) // check if date context is a bus
            {
                Bus buss = (Bus)btn.DataContext;//creating a bus with the values of the bus that his button was pressed 
                if (buss.BusStatus == Status.READY)//if the bus status is 'READY'
                    MessageBox.Show(String.Format($"this bus is {buss.BusStatus}"));

                //check if the bus need a repair or a refuel
                else if (buss.TreatKms > 20000 || buss.TreatTime.AddYears(1) < DateTime.Now || buss.FuelInKm == 0)
                    MessageBox.Show("This bus need to be rapeir");

                else//if the bus is not bussy 
                {
                    Window3 second = new Window3(buss);//creating a new window 3
                    second.Show();//opening window 3
                }
            }
        }
    }
}