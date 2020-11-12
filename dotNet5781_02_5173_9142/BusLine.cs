using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace dotNet5781_02_5713_9142
{
    public class BusLine : IComparable<BusLine>
    {
        private double totalTime;
        private List<BusStation> busStations = new List<BusStation>();
        public List<BusStation> BusStations
        {
            get
            {
                List<BusStation> temp = new List<BusStation>(busStations);
                return temp;
            }
        }
        public int Number { get; set; }
        public BusStation FirstStation { get; set; }
        public BusStation LastStation { get; set; }
        public void AddFirst(BusStation sta)
        {
            busStations.Insert(0, sta);
            FirstStation = busStations[0];
            if (busStations.Count == 1)
                LastStation = FirstStation;
            busStations[0].TravelTime = TimeSpan.Zero;
            busStations[0].Distance = 0;
        }
        public void AddLast(BusStation sta)
        {
            busStations.Add(sta);
            LastStation = busStations[busStations.Count - 1];
            if (busStations.Count == 1)
                FirstStation = LastStation;
        }
        public void Add(int index, BusStation sta)
        {
            if (index == 0)
                AddFirst(sta);
            else if (index == busStations.Count)
                AddLast(sta);
            else if (index > busStations.Count)
            {
                throw new ArgumentOutOfRangeException("index should be less than or equal to" + busStations.Count);
            }
            else
                busStations.Insert(index, sta);
            Distance();
        }
        public Area Place { get; set; }
        public override string ToString()
        {
            string station = "Bus Line: ";
            station += Number.ToString() + ", Area: "
             + Place + " numbers of bus station: ";
            foreach (var x in busStations)
                station += x.BusStationKey + " ";
            return station;
        }
        public bool CheckStation(int key)
        {
            foreach (var x in busStations)
                if (x.BusStationKey == key)
                    return true;
            return false;
        }
        public double DistanceCal(int s1, int s2)
        {
            bool b1, b2;
            double sum = 0;
            b1 = CheckStation(s1);
            b2 = CheckStation(s2);
            if (b1 == false || b2 == false) //need to throw an exeption
                throw new KeyNotFoundException(string.Format(b1 && b2 == false ? "{0} and {1}" :
                  (b1 == false ? "{0}" : "{1}") + " is not foune in the list\n ", s1, s2));
            b1 = b2 = false;
            foreach (var x in busStations)
            {
                if (b1 == true || b2 == true)
                    sum += x.Distance;
                if (x.BusStationKey == s1)
                {
                    if (b2 == true)
                        break;
                    b1 = true;
                }
                if (x.BusStationKey == s2)
                {
                    if (b1 == true)
                        break;
                    b2 = true;
                }
            }
            return sum;
        }
        public TimeSpan TimeCal(int s1, int s2)
        {
            bool b1, b2;
            TimeSpan sum = new TimeSpan();
            b1 = CheckStation(s1);
            b2 = CheckStation(s2);
            if (b1 == false || b2 == false) //need to throw an exeption
                throw new KeyNotFoundException(string.Format(b1 && b2 == false ? "{0} and {1}" : (b1 == false ? "{0}" : "{1}")
                    + " is not foune in the list\n ", s1, s2));
            b1 = b2 = false;
            foreach (var x in busStations)
            {
                if (b1 == true || b2 == true)
                    sum += x.travelTime;
                if (x.BusStationKey == s1)
                {
                    if (b2 == true)
                        break;
                    b1 = true;
                }
                if (x.BusStationKey == s2)
                {
                    if (b1 == true)
                        break;
                    b2 = true;
                }
            }
            return sum;
        }
        public void Remove(int s1)
        {
            bool b1;
            b1 = CheckStation(s1);
            if (b1 == false) //need to throw an exeption
                throw new KeyNotFoundException(string.Format("{0} is not foune in the list\n ", s1));
            foreach (var x in busStations)
            {
                if (x.BusStationKey == s1)
                {
                    busStations.Remove(x);
                    break;
                }
            }
            Distance();
        }
        public int CompareTo(BusLine other)
        {
            double mytotal = TotalTime();
            double othertotal = other.TotalTime();

            return mytotal.CompareTo(othertotal);
        }
        public double TimeBetween(BusStation one, BusStation two)
        {
            return one.travelTime.Subtract(two.travelTime).TotalMinutes;
        }
        public double TotalTime()
        {
            double total = 0;
            for (int i = 0; i < busStations.Count - 1; i++)
            {
                total += TimeBetween(busStations[i], busStations[i + 1]);
            }

            return total;
        }
        public void Distance()
        {
            busStations[0].distance = 0;
            for (int i = 1; i < busStations.Count - 1; i++)
                busStations[i].distance = DistanceByCoordinates(busStations[i - 1], busStations[i]);
        }
        /// <summary>
        /// calculate the distance between tow station by this formula
        /// "root((squar(Longitude point one lass Longitude point two) 
        ///   +    squar(Longitude point one lass Longitude point two)) "
        /// </summary>
        /// <param name="station1"></param>
        /// 
        /// <param name="station2"></param>
        /// 
        /// <returns></returns>
        public double DistanceByCoordinates(BusStation station1, BusStation station2)
        {
            bool b1, b2;
            b1 = CheckStation(station1.BusStationKey);
            b2 = CheckStation(station2.BusStationKey);
            if (b1 == false || b2 == false) //need to throw an exeption
                throw new KeyNotFoundException(string.Format(b1 && b2 == false ? "{0} and {1}" :
                  (b1 == false ? "{0}" : "{1}") + " is not foune in the list\n ", station1, station2));
            double tempLatitude = station1.Latitude * 100000 - station2.Latitude * 100000;
            double tempLongitude = station1.Longitude * 100000 - station2.Longitude * 100000;
            tempLatitude *= tempLatitude;
            tempLongitude *= tempLongitude;
            double tempDistance = tempLatitude + tempLongitude;
            return (Math.Sqrt(tempDistance));
        }
        public void TravelTime()
        {
            foreach (var item in busStations)
            {
                double temp = item.distance / 80;
                while (temp % 2 != 0 && temp % 3 != 0)
                {
                    temp *= 10;
                    item.travelTime = TimeSpan.FromMinutes(temp);
                }
            }
        }
       
    }
}



