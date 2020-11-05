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
        public double Latitude { get; }
        public double Longitude { get; }
        public int BusStationKey { get; }
        public override string ToString()
        {
            string station = "";
            station += busStationKey.ToString() + ", ";
            station += string.Format("{0:0.000000}", latitude).ToString() + "°N ";
            station += string.Format("{0:0.000000}", longitude).ToString() + "°E";
            return station;
        }

    }
}
