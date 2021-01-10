using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusStation
    {
        public int BusStationKey { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name{ get; set; }
        public IEnumerable<BusLine> LineInStation { get; set; }
        public override string ToString() {

            return $"Bus station key = {BusStationKey}\nLatitude = {Latitude}\nLongitude = {Longitude}\n" +
                $"station name = {Name}";
        }

    }
}
