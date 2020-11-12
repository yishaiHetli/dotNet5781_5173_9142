using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_5713_9142
{
    class Program
    { 
        static void Main(string[] args)
        {
            List<BusStation> myStations = new List<BusStation>();
            List<BusLine> myLines = new List<BusLine>();
            CompanyBuild(ref myStations, ref myLines);

            BusCompany myComp = new BusCompany();
            foreach (var x in myLines)
                myComp.AddNewLine(x);
           

        }
        static public void CompanyBuild(ref List<BusStation> myStations, ref List<BusLine> myLines)
        {
            BusStation mySta = new BusStation();
            Random r = new Random();
            for (int i = 0; i < 40; ++i)
            {
                mySta.Latitude = r.NextDouble() * (33.3 - 31) + 31;
                mySta.Longitude = r.NextDouble() * (35.5 - 34.3) + 34.3;
                int val = r.Next(0, 1000000);
                foreach (var x in myStations)
                    if (x.BusStationKey == val && (x.Latitude != mySta.Latitude || x.Longitude != mySta.Longitude))
                    {
                        --i;
                        mySta = new BusStation();
                        continue;
                    }
                mySta.BusStationKey = val;
                myStations.Add(mySta);
                mySta = new BusStation();
            }
            BusLine myLine = new BusLine();
            int count = 0;
            for (int i = 0; i < 10; ++i)
            {
                int val = r.Next(0, 1000);
                bool check = true;
                foreach (var x in myLines)
                    if (x.Number == val)
                    {
                        --i;
                        check = false;
                        myLine = new BusLine();
                        break;
                    }
                if (!check)
                    continue;
                for (int j = 0; j < 5 && count < 40; j++, count++)
                {
                    myLine.Add(j, myStations[count]);
                }
                count--;
                myLine.Number = val;
                myLines.Add(myLine);
                myLine = new BusLine();
            }
        }
    }
}
