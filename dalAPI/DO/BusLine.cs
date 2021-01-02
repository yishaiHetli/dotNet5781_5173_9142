using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace DO
{
    public class BusLine
    {
        public static int ID { get; set; } = 0;
        public int LineID { get; set; }
        public Area Place { get; set; }
        public int LineNumber { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }

        public override string ToString() => this.ToStringProperty();
    }
}
