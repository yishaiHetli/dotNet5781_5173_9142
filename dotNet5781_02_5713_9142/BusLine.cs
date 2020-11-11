using System;
using System.Collections.Generic;

namespace dotNet5781_02_5713_9142
{
    public class BusLine : IComparable<BusLine>
    {
        private List<BusStation> busStations = new List<BusStation>();
        public List<BusStation> BusStations { get; }
        public int Number { get; set; }
        public BusStation FirstStation { get; private set; }
        public BusStation LastStation { get; private set; }
        public void AddFirst(BusStation sta)
        {
            busStations.Insert(0, sta);
            FirstStation = busStations[0];
            if (busStations.Count - 1 == 0)
                LastStation = FirstStation;
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
            if (index > busStations.Count)
            {
                throw new ArgumentOutOfRangeException("index", "index should be less than or equal to" + busStations.Count);
            }
            busStations.Insert(index, sta);
            if (index == 0)
                FirstStation = busStations[0];
            if (index == busStations.Count)
                LastStation = busStations[busStations.Count];
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
                    sum += x.TravelTime;
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
                if(x.BusStationKey == s1)
                {
                    busStations.Remove(x);
                    break;
                }
            }
        }

        public int CompareTo(BusLine other)
        {
            double mytotal = totalTime();
            double othertotal = other.totalTime();

            return mytotal.CompareTo(othertotal);
        }
        public double TimeBetween(BusStation one, BusStation two)
        {
            return one.TravelTime.Subtract(two.TravelTime).TotalMinutes;
        }
        private double totalTime()
        {
            double total = 0;
            for (int i = 0; i < busStations.Count - 1; i++)
            {
                total += TimeBetween(busStations[i], busStations[i + 1]);
            }

            return total;
        }

    }

}