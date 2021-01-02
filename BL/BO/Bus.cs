using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace BO
{
    public class Bus
    {
        public int LicenseNum { get; set; }
        public double TotalKm { get; set; }
        public double FuelInKm { get; set; }
        public Status BusStatus { get; set; }
        public DateTime StartActivity { get; set; }
        public string LicenceInString
        {
            get
            {
                return (LicenseNum.ToString().Length == 7) ? String.Format("{0:##-###-##}  ",
                    LicenseNum) : String.Format("{0:###-##-###}", LicenseNum);
            }
        }
        public override string ToString() {

            return String.Format($"Bus Number: {LicenceInString}\n" + $"fuel in km {FuelInKm}\n" +
                                       "Activity date:{0}", String.Format("{0:dd/MM/yyyy}\n", StartActivity)
                                       + $"Total milge: {TotalKm}\n" + $"Bus Status {BusStatus}\n");
        }
    }
}
