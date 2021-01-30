using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    public class DataXml
    {
        private XElement busStationRoot;
        string pathBusStation = System.IO.Directory.GetCurrentDirectory() + @"\..\Xml\BusStation.xml";
        public void AddBusStationToXml(DO.BusStation busStation)
        {
            //LoadBusStationData();
            XElement newBusStation = new XElement("BusStation");
            XElement busStationKey = new XElement("Id", busStation.BusStationKey);
            XElement name = new XElement("IdTypes", busStation.Name);
            XElement longitude = new XElement("IdTypes", busStation.Longitude);
            XElement latitude = new XElement("IdTypes", busStation.Latitude);
            busStationRoot.Add(busStationKey, name, longitude, latitude);

            busStationRoot.Save(pathBusStation);
            

        }
    }
}
