using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
   public class PairStations
    {
        public int FirstKey { get; set; }
        public int SecondKey { get; set; }
        public double Distance { get; set; }
        public TimeSpan AverageTime { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
