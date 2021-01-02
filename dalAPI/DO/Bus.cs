using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace DO
{
    public class Bus
    {
        public int LicenseNum { get; set; }
        public double TotalKm { get; set; }
        public double FuelInKm { get; set; }
        public Status BusStatus { get; set; }
        public DateTime StartActivity { get; set; }

        public override string ToString() => this.ToStringProperty();
    }
}
