using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_5713_9142
{
    class Program
    {
        enum Menu
        {
            Exit, Add, Select, Refix, Print
        }
        static void Main(string[] args)
        {
            int choice;
            Random r = new Random(DateTime.Now.Millisecond);
            List<Bus> busList = new List<Bus>();
            Console.Write("To exit press 0\nTo add a new bus press 1\n" +
                   "To select a bus press 2\nTo refueling or fixing your bus  press 3\n" +
                   "To show all the buses in the compeny press 4\n");
            do
            {
                choice = Convert.ToInt32(Console.ReadLine());
                switch ((Menu)choice)
                {
                    case Menu.Add:
                        AddNewBus(ref busList);
                        break;
                    case Menu.Select:
                        SelectBus(ref busList, ref r);
                        break;
                    case Menu.Refix:
                        RefuelOrFix(ref busList);
                        break;
                    case Menu.Print:
                        PrintBuses(ref busList);
                        break;
                    case Menu.Exit:
                        break;
                    default:
                        Console.WriteLine("Wrong number");
                        break;

                }
            } while (choice != 0);

        }
        static void AddNewBus(ref List<Bus> busList)
        {
            bool check = false;
            while (!check)
            {
                Console.WriteLine("enter the license number");
                int license = int.Parse(Console.ReadLine());
                if (LicenesPlace(ref busList, license) >= 0)
                {
                    Console.WriteLine("this license already exist");
                    return;
                }
                if (license >= 100000000 || license < 1000000)
                {
                    Console.WriteLine("the licenes is not valid ");
                    continue;
                }
                Console.WriteLine("enter the activity start time");
                Console.Write("Year : ");
                int year = Convert.ToInt32(Console.ReadLine());
                Console.Write("Month : ");
                int month = Convert.ToInt32(Console.ReadLine());
                Console.Write("Day : ");
                int day = Convert.ToInt32(Console.ReadLine());
                if (month > 12 || month < 1 || day > 31 || day < 1)
                {
                    Console.WriteLine("Wrong date");
                    return;
                }
                DateTime activDate = new DateTime(year, month, day);
                check = LicensCheck(year, license);
                if (!check)
                {
                    Console.WriteLine("the licenes number is not valid ");
                    continue;
                }
                busList.Add(new Bus(license, activDate));
            }
        }
        static bool LicensCheck(int year, int license)
        {
            if (year >= 2018 && license < 10000000)
                return false;
            if (year < 2018 && license >= 10000000)
                return false;
            return true;
        }

        static void SelectBus(ref List<Bus> busList, ref Random r)
        {
            int kilometer = r.Next(1200);
            int license = int.Parse(Console.ReadLine());
            Console.WriteLine("enter license of the bus");
            int i = LicenesPlace(ref busList, license);
            if (i == -1)
            {
                Console.WriteLine("the bus license number was not found");
                return;
            }
            if (busList[i].Mileage + kilometer > 20000)
            {
                Console.WriteLine("the ride can not be made due to high mileage");
                return;
            }
            if (busList[i].FuelTime + kilometer > 1200)
            {
                Console.WriteLine("the ride can not be made due to lack of fuel");
                return;
            }
            DateTime x = busList[i].RepairDate;
            x.AddYears(1);
            if (DateTime.Now >= x)
            {
                Console.WriteLine("It has been a year or more since the last repair");
                return;
            }
            busList[i].FuelTime = busList[i].FuelTime + kilometer;
            busList[i].Mileage = busList[i].Mileage + kilometer;
        }

        static void RefuelOrFix(ref List<Bus> busList)
        {
            Console.WriteLine("Enter the license number of the bus");
            int license = int.Parse(Console.ReadLine());
            int i = LicenesPlace(ref busList, license);
            if (i == -1)
            {
                Console.WriteLine("the bus license number was not found");
                return;
            }
            Console.WriteLine("To refuel press 1 ,To fix the bus press 2");
            int choice = Console.Read();
            while (choice != 1 && choice != 2)
            {
                Console.Write("Wrong number please press again\t");
                choice = Convert.ToInt32(Console.ReadLine());
            }
            if (choice == 1)
                busList[i].Refuel();
            else
                busList[i].Repair();
        }
        static void PrintBuses(ref List<Bus> busList)
        {
            for (int i = 0; i < busList.Count; ++i)
            {
                busList[i].PrintBus();
            }
        }

        static int LicenesPlace(ref List<Bus> busList, int license)
        {
            int i = 0;
            for (; i < busList.Count; ++i)
            {
                if (license == busList[i].LicenseNum)
                    break;
            }
            if (i == busList.Count)
                return -1;
            return i;
        }
    }
}
