using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            busStation A = new busStation();
            Console.WriteLine("Bus Station Code: " + A.ToString());
            Console.ReadKey();
        }

    }
   using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class busStation
    {
        int busStationKey;
        double latitude;
        double longitude;
        public busStation()
        {
            Random r = new Random();
            latitude = r.NextDouble() * (33.3 - 31) + 31;
            longitude = r.NextDouble() * (35.5 - 34.3) + 34.3;
            busStationKey = Convert.ToInt32(latitude * longitude);
            while (busStationKey > 999999 || busStationKey < 100000)
            {
                if (busStationKey > 999999)
                    busStationKey /= 10;
                if (busStationKey < 100000)
                    busStationKey *= 10;
            }
        }
        public double Latitude { get { return latitude; } }
        public double Longitude { get { return longitude; } }
        public int BusStationKey { get { return busStationKey; } }

        public override string ToString()
        {
            string station = "";
            station += busStationKey.ToString() + ", ";
            station += string.Format("{0:0.000000}", latitude).ToString() + "°N ";
            station += string.Format("{0:0.000000}", longitude).ToString() + "°E";
            return station;
        }
    }
    class stationLine
    {
        busStation currentStation;
        double distanceFromLast;//מרחק
        double timeFromLast;//זמן נסיעה
        public double DistanceFromLast { get; set; }
        public double TimeFromLast { get; set; }
    }
    class BusLine
    {
        List<stationLine> listBusLine;
        int BusNumber;
        busStation FirstStation;
        busStation LastStation;
        string Area;
        public BusLine() { listBusLine = new List<stationLine>(); }
        public void ListBusLine(stationLine a) { listBusLine.Add(a); }
        public void DistanceFromLast()
        {
            for (int i = 0; i < listBusLine.Count; i++)
            { 
                if (i == 0)
                {
                    listBusLine[i].DistanceFromLast = 0;
                    listBusLine[i].TimeFromLast = 0;
                }
                else
                {

                }

            }
        }
        public override string ToString()
        {
            string station = "Bus Line: ";
            station += BusNumber.ToString() + ", Area: ";
            station += string.Format("{0:0.000000}", latitude).ToString() + "°N ";
            station += string.Format("{0:0.000000}", longitude).ToString() + "°E";
            return station;
        }
    }
    class colactionBusLine: 
    {
        public List<BusLine> list;
        public List<BusLine> a;
        public int code;

    }
}

}
