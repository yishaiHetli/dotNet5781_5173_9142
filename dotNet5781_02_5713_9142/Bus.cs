using System;

namespace dotNet5781_02_5713_9142
{
    public class Bus
    {
        public int Mispar { get; set; }
        public DateTime StartYear { get; set; }

        public override string ToString()
        {
            return String.Format("Bus {0} was klita be {1}", Mispar, StartYear.Year.ToString());
        }
    }
}