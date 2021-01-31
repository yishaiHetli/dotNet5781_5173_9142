using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BLApi;
using BO;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for BusDatails.xaml
    /// </summary>
    public partial class DatailsBus : Window
    {
        IBL bl;
        public Bus myBus { get; set; }
        ListBox list;
        List<Bus> listOfBuses;
        BackgroundWorker worker;
        /// <summary>
        /// the progrem shows the datails of the selected bus
        /// </summary>
        /// <param name="_bl">the performance of the BL</param>
        /// <param name="bus">the bus we want to see his datails</param>
        /// <param name="_list">the listBox of the buses that are in display</param>
        /// <param name="_listOfBuses">the list of buses</param>
        public DatailsBus(IBL _bl, Bus bus, ListBox _list, List<Bus> _listOfBuses)
        {
            bl = _bl;
            list = _list;
            listOfBuses = _listOfBuses;
            myBus = bus;
            InitializeComponent();
            myGrid.DataContext = this;
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
        }
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
                Status status = myBus.BusStatus;
                myBus.BusStatus = Status.READY;
                if(status == Status.REFUELING)
                bl.GetRefule(myBus);
                else
                    bl.GetRepair(myBus);
                myBus = bl.GetBus(myBus.LicenseNum);
                listText.Text = myBus.ToString();
                listOfBuses = (from number in bl.GetAllBuss()
                               select number).ToList();
                list.ItemsSource = null;
                list.ItemsSource = listOfBuses;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int x = 0;
            if (myBus.BusStatus == Status.REFUELING)
                x = 15;
            else if (myBus.BusStatus == Status.REPAIRING)
                x = 25;
            for (int i = 0; i < x; ++i)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);

                }
            }
            worker.ReportProgress(1);
        }
        private void refule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myBus.BusStatus != Status.READY)
                {
                    MessageBox.Show($"this bus is {myBus.BusStatus}");
                }
                else
                {
                    myBus.BusStatus = Status.REFUELING;
                    bl.UpdateStatus(myBus);
                    listText.Text = myBus.ToString();
                    listOfBuses = (from number in bl.GetAllBuss()
                                   select number).ToList();
                    list.ItemsSource = null;
                    list.ItemsSource = listOfBuses;
                    worker.RunWorkerAsync();
                }
            }
            catch (BO.BadLisenceException ex)
            { MessageBox.Show("error", ex.Message); }
        }
        private void repair_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myBus.BusStatus != Status.READY)
                {
                    MessageBox.Show($"this bus is {myBus.BusStatus}");
                }
                else
                {
                    myBus.BusStatus = Status.REPAIRING;
                    bl.UpdateStatus(myBus);
                    listText.Text = myBus.ToString();
                    listOfBuses = (from number in bl.GetAllBuss()
                                   select number).ToList();
                    list.ItemsSource = null;
                    list.ItemsSource = listOfBuses;
                    worker.RunWorkerAsync();
                }
            }
            catch (BO.BadLisenceException ex)
            { MessageBox.Show("error", ex.Message); }
        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
