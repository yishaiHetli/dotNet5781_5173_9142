using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5713_9142
{
     public class BusLine
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
            if (busStations.Count - 1 == 0)
                FirstStation = LastStation;
        }
        public void Add(int index, BusStation sta)
        {
            busStations.Insert(index, sta);
            if (index == 0)
                FirstStation = busStations[0];
            if (index == busStations.Count - 1)
                LastStation = busStations[busStations.Count - 1];
        }

    }
}