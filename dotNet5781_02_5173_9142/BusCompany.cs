using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.Schema;

namespace dotNet5781_02_5713_9142
{
    public class BusCompany : IEnumerable<BusLine>
    {
        public DateTime StartYear { get; set; }
        public override string ToString()
        {
            return String.Format("Bus {0} was klita be {1}", NumLine, StartYear.Year.ToString());
        }

        private List<BusLine> busses = new List<BusLine>();
        public int NumLine { get; set; }
        public List<BusLine> Busses { get; }
        public void AddNewLine(BusLine bus)
        {
            foreach (var item in Busses)//Bus Company    
            {
                if (item.Number == bus.Number)
                    throw new ArgumentException("the line number already exist");
                int j = 0;
                for (int i = 0; i < Busses.Count; i++)//bus line
                {
                    if (Busses[i].FirstStation == bus.FirstStation && Busses[i].LastStation == bus.LastStation)
                    {
                        for (; j < Busses.Count; j++)//bus station
                        {
                            if (Busses[i].BusStations[j] != bus.BusStations[j])
                                break;
                            if (j == Busses.Count - 1)
                                throw new ArgumentException("this route already exists");
                        }
                    }
                }
            }
            busses.Add(bus);
        }
        public void RemoveLine(int busNumber)
        {
            bool check = false;
            foreach (var item in busses)
            {
                if (item.Number == busNumber)
                {
                    busses.Remove(item);
                    check = true;
                }
                if (!check)
                    throw new KeyNotFoundException(string.Format("bus line number {0} not found", line.Number));
            }
        }
        public IEnumerator<BusLine> GetEnumerator()
        {
            return busses.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return busses.GetEnumerator();
        }
        public List<BusLine> SortBrTime()
        {
            List<BusLine> sorted = new List<BusLine>(busses);
            for (int i = 0; i < busses.Count; i++)
                sorted[i] = busses[i];
            sorted.Sort();
            return sorted;
        }
    }
}
