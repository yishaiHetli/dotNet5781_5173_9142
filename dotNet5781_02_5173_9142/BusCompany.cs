using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;

namespace dotNet5781_02_5173_9142
{
    public class BusCompany : IEnumerable<BusLine>
    {
        private List<BusLine> myBusses = new List<BusLine>();
        public List<BusLine> MyBusses
        {
            get
            {
                List<BusLine> temp = new List<BusLine>(myBusses);
                return temp;
            }
        }
        public BusLine this[int index, int first]

        /// <summary>
        ///  the function adding a line to the list myBusses
        /// </summary>
        /// <param name="line">the line we want to add</param>
        public void AddNewLine(BusLine line)
        {
            line.FirstStation = line.BusStations[0];
            line.LastStation = line.BusStations[line.BusStations.Count - 1];
            int j = 0;
            //chek if the line allready exist
            for (int i = 0; i < myBusses.Count; i++)
            {
                if (line.Number == myBusses[i].Number)
                {
                    if (line.FirstStation.BusStationKey != myBusses[i].LastStation.BusStationKey
                        || line.LastStation.BusStationKey != myBusses[i].FirstStation.BusStationKey)
                    {
                        throw new DuplicateNameException(string.Format("line number-{0} already exist, " +
                          "and they are not back and forth lines ", line.Number));
                    }
                    ++j;
                }
                if (j >= 2)
                {
                    throw new DuplicateNameException(string.Format("line number:{0} already exist, " +
                        "back and forth ", line.Number));
                }
            }
            j = 0;
            //chek if ther is a line with the same route
            for (int i = 0; i < myBusses.Count; i++)
            {
                if (myBusses[i].FirstStation == line.FirstStation && myBusses[i].LastStation == line.LastStation)
                {
                    for (; j < myBusses.Count; j++)
                    {
                        if (myBusses[i].BusStations[j] != line.BusStations[j])
                            break;
                        if (j == myBusses.Count - 1)
                            throw new ArgumentException("this route already exists");
                    }
                }
            }
            myBusses.Add(line);
            //call the func to sets all the Distance and Time 
            myBusses[myBusses.Count - 1].Distance();
            myBusses[myBusses.Count - 1].TravelTime();
        }

        /// <summary>
        /// the function remove a line from the list myBusses
        /// </summary>
        /// <param name="line">the line we want to remove</param>
        public void RemoveLine(BusLine line)
        {
            bool check = false;

            for (int i = 0; i < myBusses.Count; i++)
            {
                if (line.FirstStation.BusStationKey == myBusses[i].FirstStation.BusStationKey//check if this the line to remove
                    && line.Number == myBusses[i].Number)
                {
                    myBusses.Remove(line);//removing the line
                    check = true;
                }
            }
            if (!check)//check if a line was removed
                throw new KeyNotFoundException(string.Format("bus line number {0} not found", line.Number));
        }

        /// <summary>
        /// chek wheach lines containes the station in ther route 
        /// </summary>
        /// <param name="key">the station we want to check</param>
        /// <returns>a list of the lines containes the station in ther route </returns>
        public List<int> CheckStationKey(int key)
        {
            bool check = false;
            List<int> myKeys = new List<int>();
            foreach (var x in myBusses)
            {
                if (x.CheckStation(key))//check if containe th station
                {
                    myKeys.Add(x.Number);//add the line to the list
                    check = true;
                }
            }
            if (!check)//if there is no lines that containes the station in ther route 
                throw new KeyNotFoundException("we dnot have any bus that stop in this station");
            else
                return myKeys;
        }

        /// <summary>
        /// the function sort the list by travel time
        /// </summary>
        /// <returns>the sorted list</returns>
        public List<BusLine> SortList()
        {
            List<BusLine> temp = new List<BusLine>();
            temp = myBusses;
            temp.Sort();
            return temp;
        }

        /// <summary>
        /// method for the interface IEnumerable
        /// </summary>
        /// <returns>GetEnumerator</returns>
        public IEnumerator<BusLine> GetEnumerator()
        {
            return myBusses.GetEnumerator();
        }

        /// <summary>
        /// method for the interface IEnumerable
        /// </summary>
        /// <returns>GetEnumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <summary>
        /// the function check if ther is a line thet contain both of the stations in his route
        /// </summary>
        /// <param name="statoin1">the station that the user want to exit from</param>
        /// <param name="statoin2">the station that the user want to get to</param>
        /// <returns>the bus whose route contains the two statoins in the fastest way</returns>
        public BusLine TowStationsInLine(int statoin1, int statoin2)
        {
            TimeSpan a = new TimeSpan(0);
            int j = 0;
            BusLine fastest = new BusLine();
            for (int i = 0; i < myBusses.Count; i++)
            {
                myBusses[i].Distance();//calls the function Distance
                myBusses[i].TravelTime();//calls the function TravelTime
                if (myBusses[i].TowStations(statoin1, statoin2) != a)//check if the line thet contain both of the stations in his route
                {
                    if (myBusses[i].TowStations(statoin1, statoin2) > fastest.TowStations(statoin1, statoin2))//check which line is fastest

                        fastest = myBusses[i];
                    j++;
                }
            }
            if (j > 0)//check if ther is a line thet contain both of the stations in his route
                return fastest;
            throw new ArgumentException("there is no bus in the company that his route contain both of this stations");
        }

    }
}

