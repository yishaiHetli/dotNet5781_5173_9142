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
        {
            get
            {
                foreach (var item in myBusses)
                {
                    if (item.Number == index && item.FirstStation.BusStationKey == first)
                        return item;
                }
                throw new ArgumentException("this line number is not exist in the list");
            }
        }
        public void AddNewLine(BusLine line)
        {
            line.FirstStation = line.BusStations[0];
            line.LastStation = line.BusStations[line.BusStations.Count - 1];
            int j = 0;
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
            for (int i = 0; i < myBusses.Count; i++)//bus line
            {
                if (myBusses[i].FirstStation == line.FirstStation && myBusses[i].LastStation == line.LastStation)
                {
                    for (; j < myBusses.Count; j++)//bus station
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
        public void RemoveLine(BusLine line)
        {
            bool check = false;

            for (int i = 0; i < myBusses.Count; i++)
            {
                if (line.FirstStation.BusStationKey == myBusses[i].FirstStation.BusStationKey
                    && line.Number == myBusses[i].Number)
                {
                    myBusses.Remove(line);
                    check = true;
                }
            }
            if (!check)
                throw new KeyNotFoundException(string.Format("bus line number {0} not found", line.Number));
        }
        public List<int> CheckStationKey(int key)
        {
            bool check = false;
            List<int> myKeys = new List<int>();
            foreach (var x in myBusses)
            {
                if (x.CheckStation(key))
                {
                    myKeys.Add(x.Number);
                    check = true;
                }
            }
            if (!check)
                throw new KeyNotFoundException("we dnot have any bus that stop in this station");
            else
                return myKeys;
        }
        public List<BusLine> SortList()
        {
            List<BusLine> temp = new List<BusLine>();
            temp = myBusses;
            temp.Sort();
            return temp;
        }
        public IEnumerator<BusLine> GetEnumerator()
        {
            return myBusses.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public BusLine TowStationsInLine(int statoin1, int statoin2)
        {
            TimeSpan a = new TimeSpan(0);
            int j = 0;
            BusLine fastest = new BusLine();
            for (int i = 0; i < myBusses.Count; i++)
            {
                myBusses[i].Distance();
                myBusses[i].TravelTime();
                if (myBusses[i].TowStations(statoin1, statoin2) != a)
                {
                    if (myBusses[i].TowStations(statoin1, statoin2) > fastest.TowStations(statoin1, statoin2))
                        fastest = myBusses[i];
                    j++;
                }
            }
            if (j > 0)
                return fastest;
            throw new ArgumentException("there is no bus in the company that his route contain both of this stations");
        }

    }
}

