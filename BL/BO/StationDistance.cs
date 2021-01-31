using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BO
{
    public class StationDistance
    {
        public TimeSpan StartTime { get; set; }
        public int LineID { get; set; }
        public int LineNumber { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public TimeSpan Average { get; set; }

        public override string ToString()
        {
            return String.Format($"Line ID: {LineID}\t" + $"Arriving at: { String.Format("{0:hh\\:mm\\:ss}", Average)}\n"
                + $"LineNumber: {LineNumber}\t" + $"FirstStation: {FirstStation}\n" + $"LastStation: {LastStation} "
                + $"Start time: {StartTime}\n");
        }
    }
}
