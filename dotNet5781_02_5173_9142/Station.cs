using System;
using System.Collections.Generic;

namespace dotNet5781_02_5173_9142
{
    /// <summary>
    /// station for a bus
    /// </summary>
    public class Station
    {

        /// <summary>
        /// maximum values for the variables
        /// </summary>
        private const int MAXVAL = 1000000;
        private const int MIN_LAT = -90;
        private const int MAX_LAT = 90;
        private const int MIN_LON = -180;
        private const int MAX_LON = 180;
        private static List<int> keyList = new List<int>();
        private int busStationKey;
        private double latitude;
        private double longitude;
        /// <summary>
        /// Station number need to be unique and in max value of 1000000
        /// </summary>
        public int BusStationKey
        {
            get { return busStationKey; }
            set
            {
                if (value > 0 && value < MAXVAL)
                {
                    if (keyList.Contains(value))
                    {
                        throw new ArgumentException(string.Format("{0} is already exist ", value));
                    }
                    keyList.Add(value);
                    busStationKey = value;
                }
                else
                    throw new ArgumentOutOfRangeException(string.Format("{0} is not a valid station kay ", value));
            }
        }
        /// <summary>
        /// latitude need to be between 90 to -90
        /// </summary>
        public double Latitude
        {
            get { return latitude; }
            set
            {
                if (value >= MIN_LAT && value <= MAX_LAT)
                {
                    latitude = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Latitude",
                        String.Format("{0} should be between {1} and {2}", value, MIN_LAT, MAX_LAT));
                }
            }
        }
        /// <summary>
        /// longitude need to be between 180 to -180
        /// </summary>
        public double Longitude
        {
            get { return longitude; }
            set
            {
                if (value >= MIN_LON && value <= MAX_LON)
                {
                    longitude = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Longitude",
                        String.Format("{0} should be between {1} and {2}", value, MIN_LON, MAX_LON));
                }
            }

        }
        public String Address { get; set; }
        /// <summary>
        /// sets toString of varibels in tipe of this class
        /// </summary>
        public override string ToString()
        {
            string station = "Bus Station Code: ";
            station += busStationKey.ToString() + ", ";
            station += string.Format("{0:0.00000}", Math.Abs(Latitude)).ToString() +
                string.Format((Latitude > 0) ? "°N" : "°S") + ",  ";
            station += string.Format("{0:0.00000}", Math.Abs(Longitude)).ToString()
                + string.Format((Latitude > 0) ? "°E" : "°W");
            return station;
        }
    }
}