using System;

namespace dotNet5781_02_5713_9142
{
    public class BusStation : Station
    {
        /// <summary>
        /// distance from the last bus station
        /// </summary>
        public double distance;
        /// <summary>
        /// Travel time from the last stop
        /// </summary>
        public TimeSpan travelTime;
    }
}