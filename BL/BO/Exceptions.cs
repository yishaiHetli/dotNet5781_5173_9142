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
}
