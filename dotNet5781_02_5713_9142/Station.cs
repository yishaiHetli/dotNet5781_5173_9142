using System;
using System.Collections.Generic;

namespace dotNet5781_02_5713_9142
{
    /// <summary>
    /// station for bus
    /// </summary>
    public class Station
    {
        /// <summary>
        /// Station key should be unique
        /// </summary>
        public int BusStationKey
        {
            get { return busStationKey; }
            set
            {
                if (value > 0 && value < 1000000)
                {
                    if (keyList.Contains(value))
                        throw new ArgumentException(string.Format("{0} is already exist ", value));
                    keyList.Add(value);
                    busStationKey = value;
                }
                else
                    throw new ArgumentException(string.Format("{0} is not a valid station kay ", value));
            }

        }
        private double latitude;
        private double longitude;
        private int busStationKey;
        private static List<int> keyList = new List<int>();
        public Station()
        {
            Random r = new Random();
            latitude = r.NextDouble() * (33.3 - 31) + 31;
            longitude = r.NextDouble() * (35.5 - 34.3) + 34.3;
            busStationKey = r.Next(0, 1000000);
            while (keyList.Contains(busStationKey))
            {
                busStationKey = r.Next(0, 1000000);
            }
            keyList.Add(busStationKey);
        }
        public String Address { get; set; }
        public override string ToString()
        {
            string station = "Bus Station Code: ";
            station += busStationKey.ToString() + ", ";
            station += string.Format("{0:0.00000}", latitude).ToString() + "°N ";
            station += string.Format("{0:0.00000}", longitude).ToString() + "°E";
            return station;
        }
    }
}