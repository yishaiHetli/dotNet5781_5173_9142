using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dotNet5781_02_5713_9142
{
    public class BusStation : Station
    {
        /// <summary>
        /// distance from the last bus station
        /// </summary>
        public double Distance { get; set; }
        /// <summary>
        /// Travel time from the last stop
        /// </summary>
        public TimeSpan TravelTime { get; set; }
    }
}