using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
// KeyNotFoundException,DuplicateNameException,ArgumentOutOfRangeException,ArgumentException
namespace dotNet5781_02_5173_9142
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                List<BusStation> myStations = new List<BusStation>();
                BusCompany myComp = new BusCompany();
                List<BusLine> myLines = new List<BusLine>();
                CompanyBuild(ref myStations, ref myLines); // call the func to create a list of busses
                foreach (var item in myLines)//add all the lines to myComp
                    myComp.AddNewLine(item);
                myLines = myComp.MyBusses;
                foreach (var item in myLines)//print all the line
                    Console.WriteLine(item.ToString());
                int x = 0;
                int choice = 0;
                do
                {
                    try
                    {
                        Console.Write("To exit press 0\nTo add a new bus line or station press 1\n" +
         "To remove a bus line or station press 2\nTo search a bus line in your station press 3\n" +
         "To print all the busses in the compny press 4 \n");
                        choice = Convert.ToInt32(Console.ReadLine());
                        switch ((Menu)choice)//convert choice to tipe of menu
                        {
                            case Menu.ADD:
                                Console.WriteLine("To add a new bus line press 0 ," +
                                    " To add a new station press 1");
                                x = Convert.ToInt32(Console.ReadLine());
                                if (x == 0)
                                    AddLine(ref myComp, ref myStations);
                                if (x == 1)
                                    AddNewStation(ref myComp, ref myStations);
                                break;
                            case Menu.REMOVE:
                                Console.WriteLine("To remove a bus line press 0 ," +
                                    " To remove a station from some bus line press 1");
                                x = Convert.ToInt32(Console.ReadLine());
                                if (x == 0)
                                    RemoveLine(ref myComp);
                                if (x == 1)
                                    RemoveStation(ref myComp);
                                break;
                            case Menu.SEARCH:
                                Console.WriteLine("To search a bus line that move in your station press 0 ," +
                                    " To search bus lines that move between two stations press 1");
                                x = Convert.ToInt32(Console.ReadLine());
                                if (x == 0)
                                    SearchStation(ref myComp);
                                if (x == 1)
                                    SearchRoute(ref myComp);
                                break;
                            case Menu.PRINT:
                                Console.WriteLine("To print all the bus lines in the company press 0 ," +
                                    " To print all the stations and the numbers of lines who get there press 1");
                                x = Convert.ToInt32(Console.ReadLine());
                                if (x == 0)
                                    PrintBuses(ref myComp);
                                if (x == 1)
                                    PrintStations(ref myComp, ref myStations);
                                break;
                            case Menu.EXIT:
                                break;
                            default:
                                Console.WriteLine("Wrong number");
                                break;
                        }
                    }
                    catch (KeyNotFoundException e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("-----------------------------------------------------------------");
                    }
                    catch (DuplicateNameException e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("-----------------------------------------------------------------");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("-----------------------------------------------------------------");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("-----------------------------------------------------------------");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("-----------------------------------------------------------------");
                    }
                } while (choice != 0);
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("-----------------------------------------------------------------");
            }
            catch (DuplicateNameException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("-----------------------------------------------------------------");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("-----------------------------------------------------------------");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("-----------------------------------------------------------------");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("-----------------------------------------------------------------");
            }
            Console.ReadKey();
        }
        /// <summary>
        /// build all the lines and there stations
        /// </summary>
        /// <param name="myStations">list of all station there are</param>
        /// <param name="myLines">list of all the lines the func add </param>
        static public void CompanyBuild(ref List<BusStation> myStations, ref List<BusLine> myLines)
        {
            BusStation mySta = new BusStation();
            Random r = new Random();
            for (int i = 0; i < 40; ++i)
            {
                bool checking = true;
                mySta.Latitude = r.NextDouble() * (33.3 - 31) + 31; // rendom double number between 33.3 to 31 
                mySta.Longitude = r.NextDouble() * (35.5 - 34.3) + 34.3; // rendom double number between 35.5 to 34.3 
                int val = r.Next(0, 1000000);
                foreach (var x in myStations)
                    if (x.BusStationKey == val && (x.Latitude != mySta.Latitude || x.Longitude != mySta.Longitude))
                    {
                        --i; // if the value is not valid than continue and don't add to the list
                        mySta = new BusStation();
                        checking = false;
                    }
                if (!checking)
                    continue;
                mySta.BusStationKey = val; // equal to the rendom number
                myStations.Add(mySta); // add the station to the list of stations
                mySta = new BusStation();
            }
            // efter we add all the stations to the list we add from the list to the list of bus lines
            BusLine myLine = new BusLine();
            int count = 0;
            for (int i = 0; i < 10; ++i)
            {
                int val = r.Next(0, 1000); // get a rendom for the line number
                bool check = true;
                foreach (var x in myLines)
                    if (x.Number == val) //check if the number alredy in the list
                    {
                        --i;
                        check = false;
                        myLine = new BusLine();
                        break;
                    }
                if (!check) // if the number alredy in the list
                    continue;
                for (int j = 0; j < 6 && count < 40; j++, count++)
                {
                    myLine.Add(j, myStations[count]);//send to add the line number with the stations
                }
                count -= 2; // make that every round add the same two stations to another line
                myLine.Number = val;
                myLine.Place = (Area)(r.Next(0, 5));
                myLines.Add(myLine);// add the line to the list
                myLine = new BusLine();
            }
        }
        /// <summary>
        /// add line to the company osf busses and add some stations randomly to this line
        /// </summary>
        /// <param name="myComp">company that hold all the busses lines</param>
        /// <param name="myStations">list of all the stations we have so far</param>
        static public void AddLine(ref BusCompany myComp, ref List<BusStation> myStations)
        {
            BusLine bus = new BusLine();
            BusStation mySta = new BusStation();
            Console.WriteLine("enter the line number to add");
            int line = Convert.ToInt32(Console.ReadLine());//read the line number
            bus.Number = line;
            Console.WriteLine("enter the area of this bus \n0 for GENERAL, " +
                " 1 for NORTH, 2 for SOUTH, 3 for CENTER, 4 for JERUSALEM ");
            int place = Convert.ToInt32(Console.ReadLine()); // read the place of the station
            bus.Place = (Area)place;
            Random r = new Random();
            // adding two station for this line
            mySta.Latitude = r.NextDouble() * (33.3 - 31) + 31;
            mySta.Longitude = r.NextDouble() * (35.5 - 34.3) + 34.3;
            mySta.BusStationKey = r.Next(0, 1000000);
            myStations.Add(mySta);
            bus.AddFirst(mySta);
            mySta = new BusStation();
            mySta.Latitude = r.NextDouble() * (33.3 - 31) + 31;
            mySta.Longitude = r.NextDouble() * (35.5 - 34.3) + 34.3;
            mySta.BusStationKey = r.Next(0, 1000000);
            myStations.Add(mySta); // add the stations to the list of stations
            bus.AddLast(mySta);
            myComp.AddNewLine(bus);// add the bus line to the company
        }
        /// <summary>
        /// Adding a stop to a particular bus line
        /// </summary>
        /// <param name="myComp">company that hold all the busses lines</param>
        /// <param name="myStations">list of all the stations we have so far</param>
        static public void AddNewStation(ref BusCompany myComp, ref List<BusStation> myStations)
        {
            Console.WriteLine("choose between all the bus lines in the company the line number that you want to add to");
            int line = Convert.ToInt32(Console.ReadLine());//enter the line
            Console.WriteLine("enter the first station in this line");
            int first = Convert.ToInt32(Console.ReadLine());
            if (line == myComp[line, first].Number)// if the line willn't found myComp[line] throw exciption
            {
                Console.WriteLine("enter the place that you want to enter your station between 0 to {0}",
                    myComp[line, first].BusStations.Count);
                int index = Convert.ToInt32(Console.ReadLine());// read the place in the list to add to
                BusStation mySta = new BusStation();
                Random r = new Random();
                // adding a new station for this line
                mySta.Latitude = r.NextDouble() * (33.3 - 31) + 31;
                mySta.Longitude = r.NextDouble() * (35.5 - 34.3) + 34.3;
                Console.WriteLine("enter the Bus Station Key");
                mySta.BusStationKey = Convert.ToInt32(Console.ReadLine());
                myStations.Add(mySta);// add the station to the list of station
                myComp[line, first].Add(index, mySta);// add the station to the bus line
            }
        }
        /// <summary>
        /// remove bus line that the user select from the company 
        /// </summary>
        /// <param name="myComp">company that hold all the busses lines</param>
        static public void RemoveLine(ref BusCompany myComp)
        {
            Console.WriteLine("enter the line to remove");
            int line = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("enter the first station");
            int first = Convert.ToInt32(Console.ReadLine());//read the first station of this line because there might be the same line on the other side
            // if the line willn't found myComp[line] throw an exciption
            myComp.RemoveLine(myComp[line, first]);
        }
        /// <summary>
        /// ramove station from a bus that user select to remove 
        /// </summary>
        /// <param name="myComp">company that hold all the busses lines</param>
        static public void RemoveStation(ref BusCompany myComp)
        {
            Console.WriteLine("enter the bus line");
            int line = Convert.ToInt32(Console.ReadLine());// read the line to remove the station from
            Console.WriteLine("enter the first station");
            int first = Convert.ToInt32(Console.ReadLine()); // read the first station in this line 
            if (myComp[line, first].Number == line)  // if this loine not exist send Exception
            {
                int sta = Convert.ToInt32(Console.ReadLine());
                myComp[line, first].Remove(sta); // remove the station
            }
        }
        /// <summary>
        /// check weach lines are stping in the station   
        /// </summary>
        /// <param name="myComp">contane the list of the lines</param>
        static public void SearchStation(ref BusCompany myComp)
        {
            Console.WriteLine("enter the station number");
            int sta = Convert.ToInt32(Console.ReadLine());
            List<int> lines = myComp.CheckStationKey(sta);
            foreach (var x in lines)
            {
                Console.Write(x + " ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// the function check if the line hav both of the stations in his route
        /// </summary>
        /// <param name="myComp">contane the list of the lines</param>
        static public void SearchRoute(ref BusCompany myComp)
        {
            Console.WriteLine("Enter the station where you want to get on the bus, " +
                              "and the station you want to get to");
            int station1 = Convert.ToInt32((Console.ReadLine()));
            int station2 = Convert.ToInt32((Console.ReadLine()));
            Console.WriteLine(myComp.TowStationsInLine(station1, station2).ToString());
        }
        /// <summary>
        /// print a list of the lines
        /// </summary>
        /// <param name="myComp">contane the list of the lines</param>
        static public void PrintBuses(ref BusCompany myComp)
        {
            List<BusLine> myLines = myComp.SortList();
            foreach (var item in myLines)
                Console.WriteLine(item.ToString());
        }
        /// <summary>
        /// print a list of the stations and weach lines are stop in each station
        /// /// </summary>
        /// <param name="myComp">contane the list of the lines</param>
        /// <param name="myStations">contane the list of the stations</param>
        static public void PrintStations(ref BusCompany myComp, ref List<BusStation> myStations)
        {
            List<BusLine> lines = myComp.MyBusses;
            foreach (var i in myStations)
            {
                Console.Write(i.ToString() + "  bus line number ");
                foreach (var j in lines)
                {
                    if (j.CheckStation(i.BusStationKey))//check if the line stpos in the station
                        Console.Write(j.Number + " ");
                }
                Console.WriteLine();
            }
        }
    }
}

