using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineTrip
    {
        public static int ID { get; set; } = 0;
        public int LineID { get; set; }
        public TimeSpan Frequency { get; set; }
        public TimeSpan StartAt { get; set; }
        public TimeSpan FinishAt { get; set; }
        public override string ToString() => this.ToStringProperty();
    }

}
