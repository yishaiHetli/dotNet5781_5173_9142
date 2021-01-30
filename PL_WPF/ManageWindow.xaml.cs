using System;
using System.Windows;
using System.Windows.Controls;
using BLApi;
using System.ComponentModel;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for ManageWindow.xaml
    /// </summary>
    public partial class ManageWindow : Page
    {
        IBL bl;
        public BackgroundWorker worker;
        private MainWindow mainWindow;
        int rate = 1;
        public ManageWindow(IBL _bL, MainWindow _mainWindow)
        {
            InitializeComponent();
            bl = _bL;
            mainWindow = _mainWindow;
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (worker.CancellationPending == false)
            {
                UpdateTime.StartTime += TimeSpan.FromSeconds(rate);
                pickerTime.SelectedTime += TimeSpan.FromSeconds(rate);
                speedVal.Text = UpdateTime.StartTime.ToString();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                    worker.ReportProgress(1);
                }
            }
        }
        private void Bus_Click(object sender, RoutedEventArgs e)
        {
            BusWindow win = new BusWindow(bl);
            frmMain.NavigationService.Navigate(win);
        }
        private void Line_Click(object sender, RoutedEventArgs e)
        {
            LineWindow win = new LineWindow(bl);
            frmMain.NavigationService.Navigate(win);
        }
        private void Station_Click(object sender, RoutedEventArgs e)
        {
            StationsWindow win = new StationsWindow(bl,this);
            frmMain.NavigationService.Navigate(win);
        }
      
        private void LogOut_click(object sender, RoutedEventArgs e)
        {
            mainWindow.GoBackToStartPage();
        }

        private void CloseAllWindows(object sender, RoutedEventArgs e)
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter > 0; intCounter--)
                App.Current.Windows[intCounter].Close();
        }

        private void TimePicker_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            pickerTime.SelectedTime = e.NewValue;
            UpdateTime.StartTime = e.NewValue.Value - DateTime.MinValue;
        }

        private void Simulator_Click(object sender, RoutedEventArgs e)
        {
            if (Simulator.Content.ToString() == "Start simulation")
            {
                try
                {
                    rate = int.Parse(speedVal.Text.ToString());
                    speedVal.Text = "";
                    speedVal.IsEnabled = false;
                    pickerTime.IsEnabled = false;
                    if (worker.IsBusy != true)
                    {
                        worker.RunWorkerAsync();
                        Simulator.Content = "Stop simulation";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("the value is not correct",ex.Message);
                }
            }
            else if (Simulator.Content.ToString() == "Stop simulation")
            {
                if (worker.WorkerSupportsCancellation == true)
                {
                    worker.CancelAsync();
                    Simulator.Content = "Start simulation";
                    speedVal.IsEnabled = true;
                    pickerTime.IsEnabled = true;
                    speedVal.Text = "";
                }
            }

        }
    }
}
