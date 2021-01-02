using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class UserTravel
    {
        public static int ID { get; set; } = 0;
        public int TravelID { get; set; }
        public string UserName { get; set; }
        public int LineID { get; set; }
        public int InStationKey { get; set; }
        public TimeSpan InAt { get; set; }
        public int OutStationKey { get; set; }
        public TimeSpan OutAt { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
