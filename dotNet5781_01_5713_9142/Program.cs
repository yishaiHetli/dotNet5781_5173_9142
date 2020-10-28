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
                switch ((Menu)choice)//convert choice to tipe of menu
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
        /// <summary>the function adds the new bus to the 'busList' in 
        /// condition he'll meet the criteria</summary>
        /// <param name="busList">contains all of the buses</param>
        static void AddNewBus(ref List<Bus> busList)
        {
            bool check = false;
            while (!check)
            {
                Console.WriteLine("enter the license Plate number");
                int license = int.Parse(Console.ReadLine());
                if (LicensePlace(ref busList, license) >= 0)
                {
                    Console.WriteLine("this license Plate already exist");
                    return;
                }
                if (license >= 100000000 || license < 1000000)
                {
                    Console.WriteLine("the license Plate is not valid ");
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
                if (year >= 2018 && license < 10000000 || year < 2018 && license > 9999999)
                {
                    Console.WriteLine("the license Plate number is not valid ");
                    continue;
                }
                else
                    check = true;
                busList.Add(new Bus(license, activDate));//adding the new bus to 'busList' 
            }
        }
        /// <summary>the function picks in a random way a number between 0 to 1200 for the 
        /// amount the kilometer of the drive and (in codition the bus exist)
        /// checks the mileage of the bus and he's fuel
        /// and output if he can make this drive</summary>
        /// <param name="busList">contains all of the buses</param>
        /// <param name="r">the amount of kilometers for the wanted drive</param>
        static void SelectBus(ref List<Bus> busList, ref Random r)
        {
            int kilometer = r.Next(0, 1200);
            int license = int.Parse(Console.ReadLine());
            Console.WriteLine("enter license Plate of the bus");
            int i = LicensePlace(ref busList, license);
            if (i == -1)
            {
                Console.WriteLine("the bus license Plate number was not found");
                return;
            }
            //checking the bus mileage to see if he can make this drive 
            if (busList[i].Mileage + kilometer > 20000)
            {
                Console.WriteLine("the ride can not be made due to high mileage");
                return;
            }
            //checking if the bus can make this drive with his anount of fuel
            if (busList[i].FuelTime + kilometer > 1200)
            {
                Console.WriteLine("the ride can not be made due to lack of fuel");
                return;
            }
            //create a new data time variable in the value of RepairDate + one year
            DateTime x = busList[i].RepairDate;
            x.AddYears(1);
            //checking how long ago was the last drive of this bus
            if (DateTime.Now >= x)
            {
                Console.WriteLine("It has been a year or more since the last repair");
                return;
            }
            //update the amount of fuel after the drive
            busList[i].FuelTime = busList[i].FuelTime + kilometer;
            //update the mileage of after the drive
            busList[i].Mileage = busList[i].Mileage + kilometer;
        }
        /// <summary>if the license exist the function ask if he want to 
        /// refuel or repair</summary> 
        /// <param name="busList">contains all of the buses</param>
        static void RefuelOrFix(ref List<Bus> busList)
        {
            Console.WriteLine("Enter the license number of the bus");
            int license = int.Parse(Console.ReadLine());
            int i = LicensePlace(ref busList, license);
            if (i == -1)
            {
                Console.WriteLine("the bus license Plate number was not found");
                return;
            }
            Console.WriteLine("To refuel press 1 ,To fix the bus press 2");
            int choice = Console.Read();
            while (choice != 1 && choice != 2)// loop untill input equal to one or two
            {
                Console.Write("Wrong number please press again\t");
                choice = Convert.ToInt32(Console.ReadLine());
            }
            if (choice == 1)
                busList[i].Refuel();
            else
                busList[i].Repair();
        }

        static void PrintBuses(ref List<Bus> busList)//prints all of the buses
        {
            for (int i = 0; i < busList.Count; ++i)
            {
                busList[i].PrintBus();
            }
        }
        /// <summary>the function checks if the license plate already exist</summary>
        /// <param name="busList">contains all of the buses</param>
        /// <param name="license">the value of the license plate</param>
        /// <returns>i: if exist. -1: if not exist.</returns>
        static int LicensePlace(ref List<Bus> busList, int license)
        {
            int i = 0;
            for (; i < busList.Count; ++i)
            {
                if (license == busList[i].LicensePlate)
                    break;
            }
            if (i == busList.Count)
                return -1;
            return i;
        }
    }
}
