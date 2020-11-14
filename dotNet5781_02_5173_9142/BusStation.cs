using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5173_9142
{
    public class BusStation : Station
    {
        public BusStation()
        {
        }
        /// <summary>
        /// distance from the last bus station
        /// </summary>
        public double DistanceFromLast { get; set; }
        /// <summary>
        /// Travel time from the last stop
        /// </summary>
        public TimeSpan TimeFromLast { get; set; }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}