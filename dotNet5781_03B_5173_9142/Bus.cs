using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;

namespace dotNet5781_03B_5173_9142
{
    public class Bus : INotifyPropertyChanged 
    {
        public Bus(int license, DateTime date, double mile = 0, double fuel = 1200) 
        {
            //ctor that initializ the properties
            BusLicence = license;
            StartActivity = date;
            TreatTime = date;
            TreatKms = mile;
            TotalKm = mile;
            FuelInKm = fuel;
            BusStatus = Status.READY;
        }

        public Thread condition;
        double totalKm;
        double fuelInKm;
        double treatKms;
        DateTime treatTime;
        bool helper;
        Status busStatus;
        bool strHelp;
        public string countDown;

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// return the licence in a string format
        /// </summary>
        public string LicenceInString
        {
            get
            {
                return (BusLicence.ToString().Length == 7) ? String.Format("{0:##-###-##}  ",
                    BusLicence) : String.Format("{0:###-##-###}", BusLicence);
            }
        }

        public int BusLicence { get; set; }
        public DateTime StartActivity { get; set; }
        public string CountDown { get { return countDown; } }

        // whan this varible change it notify that TotalKm change 
        public double TotalKm { get { return totalKm; } set { totalKm = value; NotifyPropertyChanged("TotalKm"); } }

        // whan this varible change it notify that FuelInKm change 
        public double FuelInKm { get { return fuelInKm; } set { fuelInKm = value; NotifyPropertyChanged("FuelInKm"); } }

        // whan this varible change it notify that TreatTime change 
        public DateTime TreatTime { get { return treatTime; } set { treatTime = value; NotifyPropertyChanged("TreatTime"); } }

        // whan this varible change it notify that TreatKms change 
        public double TreatKms { get { return treatKms; } set { treatKms = value; NotifyPropertyChanged("TreatKms"); } }

        // whan this varible change it notify that BusStatus change 
        public Status BusStatus { get { return busStatus; } set { busStatus = value; NotifyPropertyChanged("BusStatus"); } }
        /// <summary>
        /// whan this varible change it notify that countdown change 
        /// </summary>
        public bool Helper { get { return helper; } set { helper = value; NotifyPropertyChanged("CountDown"); } }
        /// <summary>
        /// whan this varible change it notify that printer change 
        /// </summary>
        public bool StrHelp { get { return strHelp ; } set { strHelp = value; NotifyPropertyChanged("Printer"); } }

        public string Printer  // print data
        {
            get
            {
                return String.Format($"Bus Number: {LicenceInString}\n" + $"fuel in km {FuelInKm}\n" +
                                   "Activity date:{0}", String.Format("{0:dd/MM/yyyy}\n", StartActivity)
                                   + String.Format("Last repair date: {0:dd/MM/yyyy}\n", TreatTime)
                                   + $"Milage since last repair: {TreatKms}\n" + $"Total milge: {TotalKm}");
            }
        }
        private void NotifyPropertyChanged(string property) // check if the Property change
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                PropertyChanged(this, new PropertyChangedEventArgs("DisplayMember"));
            }
        }

    }
}