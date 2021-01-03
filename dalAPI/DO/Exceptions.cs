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
}
