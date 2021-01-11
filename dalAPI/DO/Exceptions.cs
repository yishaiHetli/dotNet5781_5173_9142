using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class BadLisenceException : Exception
    {
        public int Lisence;
        public BadLisenceException(int id) : base() => Lisence = id;
        public BadLisenceException(int id, string message) :
            base(message) => Lisence = id;
        public BadLisenceException(int id, string message, Exception innerException) :
            base(message, innerException) => Lisence = id;

        public override string ToString() => base.ToString() + $", bad lisence number: {Lisence}";
    }
    [Serializable]
    public class BadLineException : Exception
    {
        public int ID;
        public BadLineException(int id) : base() => ID = id;
        public BadLineException(int id, string message) :
            base(message) => ID = id;
        public BadLineException(int id, string message, Exception innerException) :
            base(message, innerException) => ID = id;

        public override string ToString() => base.ToString() + $", bad line ID number: {ID}";
    }
    [Serializable]
    public class BadStationException : Exception
    {
        public int Key;
        public BadStationException(int id) : base() => Key = id;
        public BadStationException(int id, string message) :
            base(message) => Key = id;
        public BadStationException(int id, string message, Exception innerException) :
            base(message, innerException) => Key = id;

        public override string ToString() => base.ToString() + $", bad line ID number: {Key}";
    }
}
