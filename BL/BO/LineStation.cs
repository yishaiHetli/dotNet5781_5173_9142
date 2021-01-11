using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public class LineStation
    {
        public int BusStationKey { get; set; }
        public int LineID { get; set; }
        public int LIneStationIndex { get; set; }
        public double Distance { get; set; }
        public TimeSpan AverageTime { get; set; }
        public override string ToString()
        {
            return $"Bus station key: {BusStationKey}\nLineID: {LineID}\nlIne station index: {LIneStationIndex}\n" +
                $"Distance: {String.Format("{0:0.000}", Distance)}\nAverage time: {String.Format("{0:0.000}", AverageTime.TotalMinutes)}";
        }
    }
}
