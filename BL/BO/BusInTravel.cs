using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusInTravel
    {
        public static int ID { get; set; } = 0;
        public int SerialNum { get; set; }
        public int LisenceNum { get; set; }
        public int LineNum { get; set; }
        public TimeSpan ExitTime { get; set; }
        public TimeSpan ActualExitTime { get; set; }
        public int LastStation { get; set; }
        public TimeSpan LastStationTime { get; set; }
        public TimeSpan NextStationTime { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
