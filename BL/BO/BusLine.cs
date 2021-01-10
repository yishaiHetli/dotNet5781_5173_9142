using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace BO
{
    public class BusLine
    {
        public int LineID { get; set; }
        public Area Place { get; set; }
        public int LineNumber { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public IEnumerable<LineStation> LinesSta { get; set; }
        public override string ToString()
        {
            return String.Format($"Line ID: {LineID}\n" + $"Place: {Place}\n" +
              $"Line Number: {LineNumber}\n" + $"First Station: {FirstStation}\n"
              + $"Last Station: {LastStation}");
        }
    }
}
