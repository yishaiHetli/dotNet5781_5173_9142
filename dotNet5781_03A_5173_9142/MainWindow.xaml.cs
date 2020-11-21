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
using dotNet5781_02_5173_9142;
namespace dotNet5781_03A_5173_9142
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BusCompany company;
        public MainWindow()
        {
            InitializeComponent();
            company = new BusCompany();
            List<BusStation> myStations = new List<BusStation>();
            List<BusLine> myLines = new List<BusLine>();
            CompanyBuild(ref myStations, ref myLines); // call the func to create a list of busses   
            foreach (var item in myLines)
                company.AddNewLine(item);
            cbBusLines.ItemsSource = company;
            cbBusLines.DisplayMemberPath = "Number";            
        }

        private BusLine currentDisplayBusLine;
        private void ShowBusLine(int index)
        {
            foreach (var item in company)
            {
                if (item.Number == index)
                    currentDisplayBusLine = item;//
            }
            UpGrid.DataContext = currentDisplayBusLine;
            lbBusLineStations.DataContext = currentDisplayBusLine.BusStations;
        }
        public void CompanyBuild(ref List<BusStation> myStations, ref List<BusLine> myLines)
        {
            BusStation mySta = new BusStation();
            Random r = new Random();
            for (int i = 0; i < 40; ++i)
            {
                bool checking = true;
                mySta.Latitude = r.NextDouble() * (33.3 - 31) + 31; // rendom double number between 33.3 to 31 
                mySta.Longitude = r.NextDouble() * (35.5 - 34.3) + 34.3; // rendom double number between 35.5 to 34.3 
                int val = r.Next(0, 1000000);
                foreach (var x in myStations)
                    if (x.BusStationKey == val && (x.Latitude != mySta.Latitude || x.Longitude != mySta.Longitude))
                    {
                        --i; // if the value is not valid than continue and don't add to the list
                        mySta = new BusStation();
                        checking = false;
                    }
                if (!checking)
                    continue;
                mySta.BusStationKey = val; // equal to the rendom number
                myStations.Add(mySta); // add the station to the list of stations
                mySta = new BusStation();
            }
            // efter we add all the stations to the list we add from the list to the list of bus lines
            BusLine myLine = new BusLine();
            int count = 0;
            for (int i = 0; i < 10; ++i)
            {
                int val = r.Next(0, 1000); // get a rendom for the line number
                bool check = true;
                foreach (var x in myLines)
                    if (x.Number == val) //check if the number alredy in the list
                    {
                        --i;
                        check = false;
                        myLine = new BusLine();
                        break;
                    }
                if (!check) // if the number alredy in the list
                    continue;
                for (int j = 0; j < 6 && count < 40; j++, count++)
                {
                    myLine.Add(j, myStations[count]);//send to add the line number with the stations
                }
                count -= 2; // make that every round add the same two stations to another line
                myLine.Number = val;
                myLine.Place = (Area)(r.Next(0, 5));
                myLines.Add(myLine);// add the line to the list
                myLine = new BusLine();
            }
        }
        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).Number);

        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cbBusLines.SelectedValue as BusLine).Number != 0)
                ShowBusLine((cbBusLines.SelectedValue as BusLine).Number);
        }
    }
}
