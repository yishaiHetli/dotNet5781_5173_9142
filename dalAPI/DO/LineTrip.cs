using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineTrip
    {
        public int LineID { get; set; }
        public TimeSpan StartAt { get; set; }
        public override string ToString() => this.ToStringProperty();
    }

}
