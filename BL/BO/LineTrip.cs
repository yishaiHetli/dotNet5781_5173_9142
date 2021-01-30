using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTrip
    {
        public int LineID { get; set; }
        public TimeSpan StartAt { get; set; }
        public TimeSpan FinishAt { get; set; }
        public override string ToString()
        {
            return String.Format($"Line ID: {LineID}\n" + 
                 $"StartAt: {String.Format("{0:hh\\:mm\\:ss}", StartAt)}\n" +
                 $"FinishAt: {String.Format("{0:hh\\:mm\\:ss}", FinishAt)}");
        }
    }

}
