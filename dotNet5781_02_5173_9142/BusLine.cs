using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace dotNet5781_02_5173_9142
{
    public class BusLine : IComparable<BusLine>
    {

        private List<BusStation> busStations = new List<BusStation>();
        /// <summary>
        /// return a list of busStations
        /// </summary>
        public List<BusStation> BusStations
        {
            get
            {
                List<BusStation> temp = new List<BusStation>(busStations);
                return temp;
            }
        }
        /// <summary>
        /// line number of this bus get and set
        /// </summary>
        public int Number { get; set; }
        public BusStation FirstStation { get; set; }
        public BusStation LastStation { get; set; }

        public void AddFirst(BusStation sta)
        {
            busStations.Insert(0, sta);// the first place in the list
            FirstStation = busStations[0]; //updat the first station 
            if (busStations.Count == 1) // in case there is only one station
                LastStation = FirstStation;
            //updat tha Distance and TravelTime
            Distance();
            TravelTime();
        }
        public void AddLast(BusStation sta)
        {
            busStations.Add(sta);// Add func add to the last
            LastStation = busStations[busStations.Count - 1]; //updat the last station  
            if (busStations.Count == 1)// in case there is only one station
                FirstStation = LastStation;
            //updat tha Distance and TravelTime
            Distance();
            TravelTime();
        }
        public void Add(int index, BusStation sta)
        {
            if (index == 0)
                AddFirst(sta);
            else if (index == busStations.Count)
                AddLast(sta);
            else if (index > busStations.Count) // if the index is out of the range 
            {
                throw new ArgumentOutOfRangeException("index should be less than or equal to " + busStations.Count);
            }
            else
            {
                busStations.Insert(index, sta);
                Distance();
                TravelTime();
            }
        }
        public Area Place { get; set; }

        /// <summary>
        /// override of the function ToString 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string station = "Bus Line: ";
            station += Number.ToString() + ", Area: "
             + Place + " numbers of bus station: ";
            foreach (var x in busStations)
                station += x.BusStationKey + " ";
            return station;
        }

        /// <summary>
        /// check if the station is on the bus route
        /// </summary>
        /// <param name="key">the variable busStationKey of the station</param>
        /// <returns></returns>
        public bool CheckStation(int key)
        {
            foreach (var x in busStations)
                if (x.BusStationKey == key)
                    return true;
            return false;
        }

        /// <summary>
        /// method of IComparabl which allow us to compare to object of BusLine
        /// </summary>
        /// <param name="ob">the object of BusLine thet we want to compare to</param>
        /// <returns></returns>
        public int CompareTo(BusLine ob)
        {
            double total1 = totalTime();
            double total2 = ob.totalTime();

            return total1.CompareTo(total2);
        }

        public double TimeBetween(BusStation one, BusStation two)
        {
            return one.TimeFromLast.Subtract(two.TimeFromLast).TotalMinutes;
        }

        /// <summary>
        /// the function calculates the time between two bus station
        /// </summary>
        /// <param name="one">the station that the user want to exit from</param>
        /// <param name="two">the station that the user want to get to</param>
        /// <returns>time between two bus station </returns>
        public TimeSpan Time(BusStation one, BusStation two)
        {
            double distance = 0;
            TimeSpan time = new TimeSpan();
            //double time = 0;
            for (int i = 0; i < busStations.Count; i++)
            {
                if (busStations[i].BusStationKey == one.BusStationKey)//chech if arivde to the first station
                {
                    for (int j = i; j < busStations.Count; j++)//start to add the variable TimeFromLast to the variable distance
                    {
                        if (busStations[j].BusStationKey == two.BusStationKey)//chech if arivde to the secend station
                        {
                            distance += busStations[j].DistanceFromLast;
                            distance /= 70;//dividing by the travel speed
                            distance *= 60;//multiply 60 minutes
                            return time.Add(TimeSpan.FromMinutes(distance));//add the variable distance to time inTimeSpan format
                        }
                        distance += busStations[j].DistanceFromLast; //adding the variable TimeFromLast to the variable time 
                    }
                }
            }
            throw new ArgumentException("this line dose not contain both of this stations in his route");
        }

        /// <summary>
        /// the function calculates the time of the bus route by adding the value
        /// of TimeFromLast of each one of the station
        /// </summary>
        /// <returns>the time of the bus route</returns>
        private double totalTime()
        {
            double total = 0;
            for (int i = 0; i < busStations.Count - 1; i++)
            {
                total += TimeBetween(busStations[i], busStations[i + 1]);// adding the value of TimeFromLast of the next station
            }
            return total;
        }

        /// <summary>
        /// put the value of DistanceFromLast in each one of the station
        /// </summary>
        public void Distance()
        {
            busStations[0].DistanceFromLast = 0;
            for (int i = 1; i < busStations.Count - 1; i++)
                busStations[i].DistanceFromLast = DistanceByCoordinates(busStations[i - 1], busStations[i]);
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

        /// <summary>
        /// the function determines the time by speed  60 to 80 kilometers per hour
        /// so the function dividing the distance by 80
        /// </summary>
        /// </summary>
        public void TravelTime()
        {
            foreach (var item in busStations)
            {
                double temp = item.DistanceFromLast / 70;// dividing the distance by 70
                Random r = new Random();
                temp *= r.Next(60, 80);//multiply by 10 until the number is rational
                item.TimeFromLast = TimeSpan.FromMinutes(temp);// add the time in a minutes format

            }
        }

        /// <summary>
        /// the function check if the line hav both of the stations in his route
        /// </summary>
        /// <param name="statoin1">the station that the user want to exit from</param>
        /// <param name="statoin2">the station that the user want to get to</param>
        /// <returns></returns>
        public TimeSpan TowStations(int statoin1, int statoin2)
        {
            int index1 = 0;
            int index2 = 0;
            BusStation a = new BusStation();
            BusStation b = new BusStation();

            foreach (var item in busStations)
            {
                if (item.BusStationKey == statoin1)//if equal to the first station
                {
                    a = item;
                    index1++;
                }
                if (item.BusStationKey == statoin2)//if equal to the secend station
                {
                    b = item;
                    index2++;
                }
            }
            if (index1 > 0 && index2 > 0)//if the line hav both of the stations in his route
                return Time(a, b);//send to Time
            TimeSpan c = new TimeSpan(0);//send TimeSpan with the value zero
            return c;
        }
    }
}



