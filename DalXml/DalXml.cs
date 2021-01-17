using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using DalApi;
using DO;
using DS;

namespace Dal
{
    class DalXml : IDal
    {
        #region singelton
        static readonly DalXml instance = new DalXml();
        DalXml() { } // default => private 
        static DalXml()
        {
               // XDocument xmlDocumentBus = new XDocument(
               //     new XDeclaration("1.0", "utf-8", "yes"),
               //     new XElement("Buses",
               //         from bus in DataSource.buss
               //         select new XElement("Bus",
               //                new XElement("LicenseNum", bus.LicenseNum),
               //                new XElement("TotalKm", bus.TotalKm),
               //                new XElement("FuelInKm", bus.FuelInKm),
               //                new XElement("BusStatus", bus.BusStatus),
               //                new XElement("StartActivity", bus.StartActivity))));
               // xmlDocumentBus.Save(@"C:\Users\User\source\repos\yishaiHetli\dotNet5781_5173_9142\bin\Bus.xml");

               // XDocument xmlDocumentBusLine = new XDocument(
               //     new XDeclaration("1.0", "utf-8", "yes"),
               // new XElement("BusLines",
               //         from bus in DataSource.busLine
               //         select new XElement("BusLine",
               //                 new XElement("LineID", bus.LineID),
               //                new XElement("Place", bus.Place),
               //                new XElement("LineNumber", bus.LineNumber),
               //                new XElement("FirstStation", bus.FirstStation),
               //                new XElement("LastStation", bus.LastStation))));
               // xmlDocumentBusLine.Save(@"C:\Users\User\source\repos\yishaiHetli\dotNet5781_5173_9142\bin\BusLine.xml");

               // XDocument xmlDocumentBusStation = new XDocument(
               //     new XDeclaration("1.0", "utf-8", "yes"),
               // new XElement("BusStations",
               //         from bus in DataSource.busSta
               //         select new XElement("BusStation",
               //                new XElement("BusStationKey", bus.BusStationKey),
               //                new XElement("Latitude", bus.Latitude),
               //                new XElement("Longitude", bus.Longitude),
               //                new XElement("Name", bus.Name))));
               // xmlDocumentBusStation.Save(@"C:\Users\User\source\repos\yishaiHetli\dotNet5781_5173_9142\bin\BusStation.xml");

               // XDocument xmlDocumentLineStation = new XDocument(
               //    new XDeclaration("1.0", "utf-8", "yes"),
               //new XElement("LineStations",
               //         from bus in DataSource.lineSta
               //         select new XElement("LineStation",
               //                new XElement("LineID", bus.LineID),
               //               new XElement("BusStationKey", bus.BusStationKey),
               //               new XElement("LIneStationIndex", bus.LIneStationIndex))));
               // xmlDocumentLineStation.Save(@"C:\Users\User\source\repos\yishaiHetli\dotNet5781_5173_9142\bin\LineStation.xml");

               // XDocument xmlDocumentPairStation = new XDocument(
               //   new XDeclaration("1.0", "utf-8", "yes"),
               //new XElement("PairStations",
               //         from bus in DataSource.pairSta
               //         select new XElement("PairStation",
               //                new XElement("FirstKey", bus.FirstKey),
               //                new XElement("SecondKey", bus.SecondKey),
               //                new XElement("Distance", bus.Distance),
               //                new XElement("AverageTime", bus.AverageTime))));
               // xmlDocumentPairStation.Save(@"C:\Users\User\source\repos\yishaiHetli\dotNet5781_5173_9142\bin\PairStation.xml");

               // XDocument xmlDocumentUsers = new XDocument(
               //  new XDeclaration("1.0", "utf-8", "yes"),
               //new XElement("Users",
               //         from bus in DataSource.userList
               //         select new XElement("User",
               //                new XElement("UserName", bus.UserName),
               //                new XElement("Password", bus.Password),
               //                new XElement("Management", bus.Management))));
               // xmlDocumentUsers.Save(@"C:\Users\User\source\repos\yishaiHetli\dotNet5781_5173_9142\bin\Users.xml");
                XDocument xmlDocumentBusLines = XDocument.Load(@"..\bin\BusLine.xml");
            BusLine.ID = xmlDocumentBusLines.Descendants("BusLine").Count();
        }
        public static IDal Instance { get => instance; }// The public Instance property to use
        #endregion
        public void AddNewBus(Bus other)
        {
            int longs = other.LicenseNum.ToString().Length;
            if (longs < 7 || longs > 8)
                throw new DO.BadLisenceException(other.LicenseNum, "bus lisence nember is illegal");
            if ((longs == 8 && other.StartActivity.Year < 2018) ||
                (longs == 7 && other.StartActivity.Year >= 2018))
                throw new DO.BadLisenceException(other.LicenseNum, "bus lisence nember dont match the date of manfacture");
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\Bus.xml");
            if (xmlDocumentBus.Descendants("Bus").FirstOrDefault(x =>
            (int)x.Element("LicenseNum") == other.LicenseNum) != null)
                throw new DO.BadLisenceException(other.LicenseNum, "Duplicate of lisence number");
            if (other.FuelInKm > 1200)
                other.FuelInKm = 1200;
            if (other.FuelInKm < 0)
                other.FuelInKm = 0;
            xmlDocumentBus.Element("Buses").Add(
                new XElement("Bus",
                new XElement("LicenseNum", other.LicenseNum),
                new XElement("FuelInKm", other.FuelInKm),
                new XElement("BusStatus", other.BusStatus),
                new XElement("StartActivity", other.StartActivity),
                new XElement("TotalKm", other.TotalKm)));
            xmlDocumentBus.Save(@"..\bin\Bus.xml");
        }

        public void RemoveBus(int license)
        {
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\Bus.xml");
            xmlDocumentBus.Root.Elements().Where(x => (int)x.Element("LicenseNum") == license)
                .FirstOrDefault().Remove();
            xmlDocumentBus.Save(@"..\bin\Bus.xml");
        }

        public Bus GetBus(int licenseNum)
        {
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\Bus.xml");
            return xmlDocumentBus.Descendants("Bus").Where(x => (int)x.Element("LicenseNum") == licenseNum).Select(bus =>
                  new DO.Bus
                  {
                      LicenseNum = (int)bus.Element("LicenseNum"),
                      FuelInKm = (double)bus.Element("FuelInKm"),
                      BusStatus = (Status)Enum.Parse(typeof(Status), (string)bus.Element("BusStatus"), true),
                      StartActivity = (DateTime)bus.Element("StartActivity"),
                      TotalKm = (double)bus.Element("TotalKm")
                  }).FirstOrDefault();
        }

        public void UpdateBus(Bus bus)
        {
            RemoveBus(bus.LicenseNum);
            AddNewBus(bus);
        }

        public IEnumerable<Bus> GetAllBuss()
        {
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\Bus.xml");
            IEnumerable<DO.Bus> buses = from bus in xmlDocumentBus.Descendants("Bus")
                                        select new DO.Bus
                                        {
                                            LicenseNum = (int)bus.Element("LicenseNum"),
                                            FuelInKm = (double)bus.Element("FuelInKm"),
                                            BusStatus = (Status)Enum.Parse(typeof(Status), (string)bus.Element("BusStatus"), true),
                                            StartActivity = (DateTime)bus.Element("StartActivity"),
                                            TotalKm = (double)bus.Element("TotalKm")
                                        };
            return buses;
        }

        public void GetRefule(int lisence)
        {
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\Bus.xml");
            xmlDocumentBus.Descendants("Bus").Where(x => (int)x.Element("LicenseNum") == lisence)
                .FirstOrDefault().SetElementValue("FuelInKm", 1200);
            xmlDocumentBus.Save(@"..\bin\Bus.xml");
        }

        public void GetRepair(int lisence)
        {
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\Bus.xml");
            xmlDocumentBus.Descendants("Bus").Where(x => (int)x.Element("LicenseNum") == lisence)
                .FirstOrDefault().SetElementValue("FuelInKm", 1200);
            xmlDocumentBus.Descendants("Bus").Where(x => (int)x.Element("LicenseNum") == lisence)
               .Select(x => x.Element("TotalKm")).FirstOrDefault().SetValue(0);
            xmlDocumentBus.Save(@"..\bin\Bus.xml");
        }

        public IEnumerable<BusLine> GetAllLines()
        {
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\BusLine.xml");
            IEnumerable<DO.BusLine> buses = from bus in xmlDocumentBus.Descendants("BusLine")
                                            select new DO.BusLine
                                            {
                                                LineID = (int)bus.Element("LineID"),
                                                LineNumber = (int)bus.Element("LineNumber"),
                                                FirstStation = (int)bus.Element("FirstStation"),
                                                LastStation = (int)bus.Element("LastStation"),
                                                Place = (Area)Enum.Parse(typeof(Area), (string)bus.Element("Place"), true)
                                            };
            return buses;
        }

        public void RemoveBusLine(int ID)
        {
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\LineStation.xml");
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\BusLine.xml");
            xmlDocumentBus.Root.Elements().Where(x => (int)x.Element("LineID") == ID)
                .FirstOrDefault().Remove();
            xmlDocumentLineSta.Root.Elements().Where(x => (int)x.Element("LineID") == ID).Remove();
            xmlDocumentBus.Save(@"..\bin\BusLine.xml");
            xmlDocumentLineSta.Save(@"..\bin\LineStation.xml");
        }

        public IEnumerable<LineStation> GetAllLineStations(int LineID)
        {
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\LineStation.xml");
            IEnumerable<DO.LineStation> line = from bus in xmlDocumentLineSta.Descendants("LineStation")
                                               where (int)bus.Element("LineID") == LineID
                                               let l = new DO.LineStation
                                               {
                                                   LineID = (int)bus.Element("LineID"),
                                                   BusStationKey = (int)bus.Element("BusStationKey"),
                                                   LIneStationIndex = (int)bus.Element("LIneStationIndex")
                                               }
                                               select l;
            return from l in line
                   orderby l.LIneStationIndex
                   select l;
        }

        public void AddNewBusLine(BusLine bus)
        {
            bus.LineID = BusLine.ID++;
            XDocument xmlDocumentBusLine = XDocument.Load(@"..\bin\BusLine.xml");
            xmlDocumentBusLine.Element("BusLines").Add(
                      new XElement("BusLine",
                      new XElement("LineID", bus.LineID),
                      new XElement("Place", bus.Place),
                      new XElement("LineNumber", bus.LineNumber),
                      new XElement("FirstStation", bus.FirstStation),
                      new XElement("LastStation", bus.LastStation)));
            xmlDocumentBusLine.Save(@"..\bin\BusLine.xml");
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\LineStation.xml");
            xmlDocumentLineSta.Element("LineStations").Add(
                new XElement("LineStation",
                new XElement("LineID", bus.LineID),
                new XElement("BusStationKey", bus.FirstStation),
                new XElement("LIneStationIndex", 0)));
            xmlDocumentLineSta.Element("LineStations").Add(
               new XElement("LineStation",
               new XElement("LineID", bus.LineID),
               new XElement("BusStationKey", bus.LastStation),
               new XElement("LIneStationIndex", 1)));
            xmlDocumentLineSta.Save(@"..\bin\LineStation.xml");
            BusStation sta1 = GetBusStation(bus.FirstStation);
            BusStation sta2 = GetBusStation(bus.LastStation);
            updatePair(sta1, sta2);
        }

        public IEnumerable<BusStation> GetAllStation()
        {
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\BusStation.xml");
            IEnumerable<DO.BusStation> buses = from bus in xmlDocumentBus.Descendants("BusStation")
                                               select new DO.BusStation
                                               {
                                                   BusStationKey = (int)bus.Element("BusStationKey"),
                                                   Latitude = (double)bus.Element("Latitude"),
                                                   Longitude = (double)bus.Element("Longitude"),
                                                   Name = (string)bus.Element("Name")
                                               };
            return buses;
        }

        public void AddNewStop(int index, BusLine bus, BusStation station)
        {
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\LineStation.xml");
            foreach (var x in xmlDocumentLineSta.Descendants("LineStation"))
            {
                if ((int)x.Element("LineID") != bus.LineID) continue;
                if ((int)x.Element("LIneStationIndex") < index) continue;
                x.Element("LIneStationIndex").SetValue((int)x.Element("LIneStationIndex") + 1);
            }
            xmlDocumentLineSta.Element("LineStations").Add(
                new XElement("LineStation",
                new XElement("LineID", bus.LineID),
                new XElement("BusStationKey", station.BusStationKey),
                new XElement("LIneStationIndex", index)));
            xmlDocumentLineSta.Save(@"..\bin\LineStation.xml");
            if (index == 0)
            {
                LineStation nextLine = getLineSta(bus.LineID, index + 1);
                BusStation nextSta = GetBusStation(nextLine.BusStationKey);
                updatePair(station, nextSta);
                return;
            }
            LineStation line1 = getLineSta(bus.LineID, index - 1);
            LineStation line2 = getLineSta(bus.LineID, index);
            LineStation line3 = getLineSta(bus.LineID, index + 1);
            BusStation sta1 = GetBusStation(line1.BusStationKey);
            BusStation sta2 = GetBusStation(line2.BusStationKey);
            updatePair(sta1, sta2);
            if (line3 != null)
            {
                BusStation sta3 = GetBusStation(line3.BusStationKey);
                updatePair(sta2, sta3);
            }
        }
        BusStation GetBusStation(int BusStationKey)
        {
            XDocument xmlDocumentBusStation = XDocument.Load(@"..\bin\BusStation.xml");
            return xmlDocumentBusStation.Descendants("BusStation").Where(x => (int)x.Element("BusStationKey") == BusStationKey).Select(x =>
                         new DO.BusStation
                         {
                             BusStationKey = (int)x.Element("BusStationKey"),
                             Latitude = (double)x.Element("Latitude"),
                             Longitude = (double)x.Element("Longitude"),
                             Name = (string)x.Element("Name")
                         }).FirstOrDefault();
        }
        LineStation getLineSta(int lineID,int index)
        {
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\LineStation.xml");
            return xmlDocumentLineSta.Descendants("LineStation").Where
                    (l => (int)l.Element("LineID") == lineID && (int)l.Element("LIneStationIndex") == index).Select(x =>
                     new LineStation
                     {
                         LineID = lineID,
                         BusStationKey = (int)x.Element("BusStationKey"),
                         LIneStationIndex = index
                     }).FirstOrDefault();
        }

        public void AddBusStation(BusStation station)
        {
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\BusStation.xml");
            xmlDocumentLineSta.Element("BusStations").Add(
               new XElement("BusStation",
               new XElement("BusStationKey", station.BusStationKey),
               new XElement("Latitude", station.Latitude),
               new XElement("Longitude", station.Longitude),
               new XElement("Name", station.Name)));
            xmlDocumentLineSta.Save(@"..\bin\BusStation.xml");
        }

        public void RemoveSta(int ID)
        {
            BusStation sta = GetBusStation(ID);
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\LineStation.xml");
            XDocument xmlDocumentBusSta = XDocument.Load(@"..\bin\BusStation.xml");
            foreach (var lines in xmlDocumentLineSta.Descendants("LineStation")) // update lines index
            {
                if ((int)lines.Element("BusStationKey") == sta.BusStationKey)
                {
                    foreach (var line in xmlDocumentLineSta.Descendants("LineStation"))
                    {
                        if (line.Element("LineID") == lines.Element("LineID") && (int)line.Element("LIneStationIndex") > (int)lines.Element("LIneStationIndex"))
                        {
                            line.Element("LIneStationIndex").SetValue((int)line.Element("LIneStationIndex") - 1);
                        }
                    }
                }
            }
            xmlDocumentLineSta.Root.Elements().Where(x => (int)x.Element("BusStationKey") == ID).Remove();
            xmlDocumentBusSta.Root.Elements().Where(x => (int)x.Element("BusStationKey") == ID).FirstOrDefault().Remove();
            xmlDocumentLineSta.Save(@"..\bin\LineStation.xml");
            xmlDocumentBusSta.Save(@"..\bin\BusStation.xml");
        }

        public PairStations GetPair(int sta1, int sta2)
        {
            XDocument xmlDocumentPair = XDocument.Load(@"..\bin\PairStation.xml");
            return xmlDocumentPair.Descendants("PairStation").Where(x => (int)x.Element("FirstKey") == sta1
            && (int)x.Element("SecondKey") == sta2).Select(pair =>
            new PairStations
            {
                AverageTime = (TimeSpan)pair.Element("AverageTime"),
                Distance = (double)pair.Element("Distance"),
                FirstKey = sta1,
                SecondKey = sta2
            }).FirstOrDefault();
        }

        public void UpdatePairByUser(int sta1, int sta2, double distance, TimeSpan averge)
        {
            if (GetBusStation(sta1) == null)
            {
                throw new DO.BadStationException(sta1, "there is no such station");
            }
            if (GetBusStation(sta2) == null)
            {
                throw new DO.BadStationException(sta2, "there is no such station");
            }
            DO.PairStations pair = new PairStations
            {
                FirstKey = sta1,
                SecondKey = sta2,
                AverageTime = averge,
                Distance = distance
            };
            XDocument xmlDocumentPair = XDocument.Load(@"..\bin\PairStation.xml");
            xmlDocumentPair.Root.Elements().Where(x => (int)x.Element("FirstKey") == sta1 && (int)x.Element("SecondKey") == sta2).Remove();
            xmlDocumentPair.Element("PairStations").Add(
                  new XElement("PairStation",
                  new XElement("FirstKey", sta1),
                  new XElement("SecondKey", sta2),
                  new XElement("Distance", distance),
                  new XElement("AverageTime", averge)));
            xmlDocumentPair.Save(@"..\bin\PairStation.xml");
        }

        void updatePair(BusStation sta1, BusStation sta2)
        {
            if (GetPair(sta1.BusStationKey, sta2.BusStationKey) == null)
            {
                var sCoord = new GeoCoordinate(sta1.Latitude, sta1.Longitude);
                var eCoord = new GeoCoordinate(sta2.Latitude, sta2.Longitude);
                double cord = sCoord.GetDistanceTo(eCoord) / 1000;
                XDocument xmlDocumentPair = XDocument.Load(@"..\bin\PairStation.xml");
                xmlDocumentPair.Element("PairStations").Add(
                new XElement("PairStation",
                new XElement("FirstKey", sta1.BusStationKey),
                new XElement("SecondKey", sta2.BusStationKey),
                new XElement("Distance", cord),
                new XElement("AverageTime", TimeSpan.FromMinutes(cord * 2))));
                xmlDocumentPair.Save(@"..\bin\PairStation.xml");
            }
        }
        public bool CheckUser(string username, string password, bool manage)
        {
            XDocument xmlDocumentUser = XDocument.Load(@"..\bin\Users.xml");
            if (xmlDocumentUser.Descendants("User").Where(x => (string)x.Element("UserName") == username
             && (string)x.Element("Password") == password && (bool)x.Element("Management") == manage)
                  .FirstOrDefault() == null)
                return false;
            return true;
        }
    }
}