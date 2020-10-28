using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_5713_9142
{
    class Bus
    {
        int licensePlate;
        int mileage;
        int fuelTime;
        DateTime activityDate;
        DateTime repairDate;
        public Bus(int _licensePlate, DateTime _activityDate, int _mileage = 0, int _fuelTime = 0)
        {
            licensePlate = _licensePlate;
            repairDate = activityDate = _activityDate;
            mileage = _mileage;
            fuelTime = _fuelTime;
        }
        public int LicensePlate { get; }
        public int Mileage { get; set; }
        public int FuelTime { get; set; }
        public DateTime RepairDate { get; }
        public void Refuel() { fuelTime = 0; }
        public void Repair() { repairDate = DateTime.Now; mileage = 0; }
        public void PrintBus() { Console.WriteLine("bus {0} have a {1} miles traveled", licensePlate, mileage); }
    }
}
