using BLApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BO;
using System.ComponentModel;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for SimulationWin.xaml
    /// </summary>
    public partial class SimulationWin : Window
    {
        private IBL bl;
        List<StationDistance> stationLines;
        List<BusLine> lines;
        BackgroundWorker worker;
        ManageWindow manageWindow;
        BusStation lsta;
        /// <summary>
        /// the program get a time and number of secondes to skeep in every seconde
        /// and shows the estimated time of arrival 
        /// of the lines to the station accordingly
        /// </summary>
        /// <param name="_bl">the performance of the BL</param>
        /// <param name="_lsta">the list of lines that stoping in the station</param>
        /// <param name="_manageWindow">the performance of the page manageWindow</param>
        /// <param name="_lines">list of the lines</param>
        public SimulationWin(IBL _bl, BusStation _lsta, ManageWindow _manageWindow, IEnumerable<BusLine> _lines)
        {
            InitializeComponent();
            bl = _bl;
            lsta = _lsta;
            manageWindow = _manageWindow;
            lines = _lines.ToList();
            if (lines.Count() == 0) // if there are no lines that stops in this station
            {
                expected.Text = "There is no buses stoping in this station";
                stopingIn.Text = "";
            }
            else
            {
                list2.ItemsSource = lines;
                stationLines = bl.Avarge(lsta, UpdateTime.StartTime);
                list.ItemsSource = stationLines;
                worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.WorkerSupportsCancellation = true;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.WorkerReportsProgress = true;
                worker.RunWorkerAsync();
            }
        }
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            stationLines = bl.Avarge(lsta, UpdateTime.StartTime);
            list.ItemsSource = null;
            list.ItemsSource = stationLines;
            if (manageWindow.worker.IsBusy == false) // if we shot down manage worker
                this.Close();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true) // loop until it's cancel
            {
                if (worker.CancellationPending == true || manageWindow.worker.IsBusy == false)
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
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (lines.Count() != 0 && worker.IsBusy == true) // cancel worker when the window is closing
                worker.CancelAsync();
        }
    }
}
