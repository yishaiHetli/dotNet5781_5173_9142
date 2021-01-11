using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public class BadLisenceException : Exception
    {
        public int lisence;
        public BadLisenceException(string message, Exception innerException) :
            base(message, innerException) => lisence = ((DO.BadLisenceException)innerException).Lisence;
        public override string ToString() => base.ToString() + $", bad lisence number: {lisence}";
    }

    [Serializable]
    public class BadLineException : Exception
    {
        public int id;
        public BadLineException(string message, Exception innerException) :
            base(message, innerException) => id = ((DO.BadLineException)innerException).ID;
        public override string ToString() => base.ToString() + $", bad lisence number: {id}";
    }

    [Serializable]
    public class BadStationException : Exception
    {
        public int Key;
        public BadStationException(string message, Exception innerException) :
            base(message, innerException) => Key = ((DO.BadStationException)innerException).Key;
        public override string ToString() => base.ToString() + $", bad lisence number: {Key}";
    }
}
