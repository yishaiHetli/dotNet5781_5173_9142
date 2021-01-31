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
            // xmlDocumentBus.Save(@"..\bin\Bus.xml");

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
            // xmlDocumentBusLine.Save(@"..\bin\BusLine.xml");

            // XDocument xmlDocumentBusStation = new XDocument(
            //     new XDeclaration("1.0", "utf-8", "yes"),
            // new XElement("BusStations",
            //         from bus in DataSource.busSta
            //         select new XElement("BusStation",
            //                new XElement("BusStationKey", bus.BusStationKey),
            //                new XElement("Latitude", bus.Latitude),
            //                new XElement("Longitude", bus.Longitude),
            //                new XElement("Name", bus.Name))));
            // xmlDocumentBusStation.Save(@"..\bin\BusStation.xml");

            // XDocument xmlDocumentLineStation = new XDocument(
            //    new XDeclaration("1.0", "utf-8", "yes"),
            //new XElement("LineStations",
            //         from bus in DataSource.lineSta
            //         select new XElement("LineStation",
            //                new XElement("LineID", bus.LineID),
            //               new XElement("BusStationKey", bus.BusStationKey),
            //               new XElement("LineStationIndex", bus.LineStationIndex))));
            // xmlDocumentLineStation.Save(@"..\bin\LineStation.xml");

            // XDocument xmlDocumentPairStation = new XDocument(
            //   new XDeclaration("1.0", "utf-8", "yes"),
            //new XElement("PairStations",
            //         from bus in DataSource.pairSta
            //         select new XElement("PairStation",
            //                new XElement("FirstKey", bus.FirstKey),
            //                new XElement("SecondKey", bus.SecondKey),
            //                new XElement("Distance", bus.Distance),
            //                new XElement("AverageTime", bus.AverageTime))));
            // xmlDocumentPairStation.Save(@"..\bin\PairStation.xml");

            // XDocument xmlDocumentUsers = new XDocument(
            //  new XDeclaration("1.0", "utf-8", "yes"),
            //new XElement("Users",
            //         from bus in DataSource.userList
            //         select new XElement("User",
            //                new XElement("UserName", bus.UserName),
            //                new XElement("Password", bus.Password),
            //                new XElement("Management", bus.Management))));
            // xmlDocumentUsers.Save(@"..\bin\Users.xml");
            // XDocument xmlDocumentLineTrip = new XDocument(
            //  new XDeclaration("1.0", "utf-8", "yes"),
            //new XElement("LineTrips",
            //         from line in DataSource.lineTrips
            //         select new XElement("LineTrip",
            //                new XElement("LineID", line.LineID),
            //                new XElement("StartAt", line.StartAt))));
            // xmlDocumentLineTrip.Save(@"..\bin\LineTrip.xml");
            //x.Element("LineID").SetValue((int)x.Element("LineID") - 1);
            XDocument xmlDocumentBuses = XDocument.Load(@"..\bin\Bus.xml");
            foreach (var x in xmlDocumentBuses.Descendants("Bus"))
            {
                x.Element("BusStatus").SetValue(Status.READY);
            }
            xmlDocumentBuses.Save(@"..\bin\Bus.xml");
            XDocument xmlDocumentBusLines = XDocument.Load(@"..\bin\BusLine.xml");
            int id = 0;
            foreach (var bus in xmlDocumentBusLines.Descendants("BusLine"))
            {
                if ((int)bus.Element("LineID") > id)
                    id = (int)bus.Element("LineID");
            }
            BusLine.ID = ++id;
        }
        public static IDal Instance { get => instance; }// The public Instance property to use
        #endregion
        /// <summary>
        /// the function gets a bus and if the parameters are correct 
        /// adding it to the list of buses
        /// </summary>
        /// <param name="other">the bus we want to add</param>
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
        /// <summary>
        /// the function gets a license plate and 
        /// removes the bus to which it belongs
        /// </summary>
        /// <param name="license">the license of the bus we want to remove</param>
        public void RemoveBus(int license)
        {
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\Bus.xml");
            xmlDocumentBus.Root.Elements().Where(x => (int)x.Element("LicenseNum") == license)
                .FirstOrDefault().Remove();
            xmlDocumentBus.Save(@"..\bin\Bus.xml");
        }
        /// <summary>
        /// the function gets a license plate and take
        /// the bus to which it belongs and copy the data to a DO.Bus
        /// </summary>
        /// <param name="licenseNum">the licenseNum of the bus ew want</param>
        /// <returns></returns>
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
        /// <summary>
        /// the function updating the bus the bus by remoning it and recrating it
        /// </summary>
        /// <param name="bus">the bus we want to update</param>
        public void UpdateBus(Bus bus)
        {
            RemoveBus(bus.LicenseNum);
            AddNewBus(bus);
        }
        /// <summary>
        /// the function copy all of the buses the to IEnumerable
        /// </summary>
        /// <returns> IEnumerable<DO.Bus></returns>
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

        /// <summary>
        /// the function copy all of the lines to IEnumerable
        /// </summary>
        /// <returns>IEnumerable<DO.BusLine></returns>
        public IEnumerable<BusLine> GetAllLines()
        {
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\BusLine.xml");
            IEnumerable<DO.BusLine> lines = from bus in xmlDocumentBus.Descendants("BusLine")
                                            select new DO.BusLine
                                            {
                                                LineID = (int)bus.Element("LineID"),
                                                LineNumber = (int)bus.Element("LineNumber"),
                                                FirstStation = (int)bus.Element("FirstStation"),
                                                LastStation = (int)bus.Element("LastStation"),
                                                Place = (Area)Enum.Parse(typeof(Area), (string)bus.Element("Place"), true)
                                            };
            return lines;
        }
        /// <summary>
        /// the function gets an id and remove the line to which it belongs
        /// </summary>
        /// <param name="ID">th id of the line we want to remove</param>
        public void RemoveBusLine(int ID)
        {
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\LineStation.xml");
            XDocument xmlDocumentBus = XDocument.Load(@"..\bin\BusLine.xml");
            XDocument xmlDocumentLineTrip = XDocument.Load(@"..\bin\LineTrip.xml");
            xmlDocumentBus.Root.Elements().Where(x => (int)x.Element("LineID") == ID)
                .FirstOrDefault().Remove();
            xmlDocumentLineSta.Root.Elements().Where(x => (int)x.Element("LineID") == ID).Remove();
            xmlDocumentLineTrip.Root.Elements().Where(x => (int)x.Element("LineID") == ID).Remove();
            foreach (var x in xmlDocumentBus.Descendants("BusLine"))
            {
                if ((int)x.Element("LineID") < ID) continue;
                x.Element("LineID").SetValue((int)x.Element("LineID") - 1);
            }
            foreach (var x in xmlDocumentLineSta.Descendants("LineStation"))
            {
                if ((int)x.Element("LineID") < ID) continue;
                x.Element("LineID").SetValue((int)x.Element("LineID") - 1);
            }
            foreach (var x in xmlDocumentLineTrip.Descendants("LineTrip"))
            {
                if ((int)x.Element("LineID") < ID) continue;
                x.Element("LineID").SetValue((int)x.Element("LineID") - 1);
            }
            --BusLine.ID;
            xmlDocumentBus.Save(@"..\bin\BusLine.xml");
            xmlDocumentLineSta.Save(@"..\bin\LineStation.xml");
            xmlDocumentLineTrip.Save(@"..\bin\LineTrip.xml");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public IEnumerable<LineStation> GetAllLineStations(int LineID)
        {
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\LineStation.xml");
            return from bus in xmlDocumentLineSta.Descendants("LineStation")
                   where (int)bus.Element("LineID") == LineID
                   let l = new DO.LineStation
                   {
                       LineID = (int)bus.Element("LineID"),
                       BusStationKey = (int)bus.Element("BusStationKey"),
                       LineStationIndex = (int)bus.Element("LineStationIndex")
                   }
                   orderby l.LineStationIndex
                   select l;
        }
        /// <summary>
        /// the function gets a line and if the parameters are correct 
        /// adding it to the list of lines
        /// </summary>
        /// <param name="other">the line we want to add</param>
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
                new XElement("LineStationIndex", 0)));
            xmlDocumentLineSta.Element("LineStations").Add(
               new XElement("LineStation",
               new XElement("LineID", bus.LineID),
               new XElement("BusStationKey", bus.LastStation),
               new XElement("LineStationIndex", 1)));
            xmlDocumentLineSta.Save(@"..\bin\LineStation.xml");
            BusStation sta1 = GetBusStation(bus.FirstStation);
            BusStation sta2 = GetBusStation(bus.LastStation);
            updatePair(sta1, sta2); // update time between first and last stations
        }
        /// <summary>
        /// the function copy all of the bus stations to IEnumerable
        /// </summary>
        /// <returns> IEnumerable<DO.BusStation></returns>
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
        /// <summary>
        /// the function gets bus station data and line data
        /// and if the parameters of the bus station are correct 
        /// adding it to the route of the line
        /// </summary>
        /// <param name="index">the place we want to add the bus station 
        /// in the route of the line </param>
        /// <param name="bus">the line of which we want to add the bus station</param>
        /// <param name="station">the station we want to add</param>
        public void AddNewStop(int index, BusLine bus, BusStation station)
        {
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\LineStation.xml");
            XDocument xmlDocumentBusLine = XDocument.Load(@"..\bin\BusLine.xml");

            foreach (var x in xmlDocumentLineSta.Descendants("LineStation")) // update line station index
            {
                if ((int)x.Element("LineID") != bus.LineID) continue;
                if ((int)x.Element("LineStationIndex") < index) continue;
                x.Element("LineStationIndex").SetValue((int)x.Element("LineStationIndex") + 1);
            }
            xmlDocumentLineSta.Element("LineStations").Add( // add this stop 
                new XElement("LineStation",
                new XElement("LineID", bus.LineID),
                new XElement("BusStationKey", station.BusStationKey),
                new XElement("LineStationIndex", index)));
            xmlDocumentLineSta.Save(@"..\bin\LineStation.xml");

            if (xmlDocumentLineSta.Descendants("LineStation").FirstOrDefault(x =>
             (int)x.Element("LineID") == bus.LineID && (int)x.Element("LineStationIndex") > index) == null) // if it's the last station
            {
                // update bus line last station
                xmlDocumentBusLine.Descendants("BusLine").FirstOrDefault(x => (int)x.Element("LineID") == bus.LineID) 
                    .Element("LastStation").SetValue(station.BusStationKey);
                xmlDocumentBusLine.Save(@"..\bin\BusLine.xml");
            }

            if (index == 0)// if it's the first station
            {
                xmlDocumentBusLine.Descendants("BusLine").FirstOrDefault(x => (int)x.Element("LineID") == bus.LineID)
                    .Element("FirstStation").SetValue(station.BusStationKey); // update bus line first station
                xmlDocumentBusLine.Save(@"..\bin\BusLine.xml");
                LineStation nextLine = getLineSta(bus.LineID, index + 1);//get the second line station 
                BusStation nextSta = GetBusStation(nextLine.BusStationKey);  //get the second station
                updatePair(station, nextSta); // update their time and distance between
                return;
            }
            LineStation line1 = getLineSta(bus.LineID, index - 1);
            LineStation line2 = getLineSta(bus.LineID, index);
            LineStation line3 = getLineSta(bus.LineID, index + 1);
            BusStation sta1 = GetBusStation(line1.BusStationKey);
            BusStation sta2 = GetBusStation(line2.BusStationKey);
            updatePair(sta1, sta2); // update the station before the index and the index station time and distance
            if (line3 != null)
            {
                BusStation sta3 = GetBusStation(line3.BusStationKey); 
                updatePair(sta2, sta3); // update the index station and station after the index time and distance
            }
        }
        /// <summary>
        /// the function gets a bus station key plate and take
        /// the bus to which it belongs and copy the data to a  DO.BusStation
        /// </summary>
        /// <param name="BusStationKey">the bus station key of the station we want</param>
        /// <returns>DO.BusStation</returns>
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
        /// <summary>
        /// the function get a line id and  an index
        /// and looking for the bus station in the index in the line route
        /// </summary>
        /// <param name="lineID"></param>
        /// <param name="index"></param>
        /// <returns>the station in the wanted index in the line route</returns>
        LineStation getLineSta(int lineID, int index)
        {
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\LineStation.xml");
            return xmlDocumentLineSta.Descendants("LineStation").Where
                    (l => (int)l.Element("LineID") == lineID && (int)l.Element("LineStationIndex") == index).Select(x =>
                     new LineStation
                     {
                         LineID = lineID,
                         BusStationKey = (int)x.Element("BusStationKey"),
                         LineStationIndex = index
                     }).FirstOrDefault();
        }
        /// <summary>
        /// the function gets a bus station and if the parameters are correct 
        /// adding it to the list of bus stations
        /// </summary>
        /// <param name="station">the bus station we want to add</param>
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
        /// <summary>
        /// the function gets a key and 
        /// removes the station to which it belongs
        /// </summary>
        /// <param name="key">the key of the station we want to remove</param>
        public void RemoveSta(int key)
        {
            BusStation sta = GetBusStation(key);
            XDocument xmlDocumentLineSta = XDocument.Load(@"..\bin\LineStation.xml");
            XDocument xmlDocumentBusSta = XDocument.Load(@"..\bin\BusStation.xml");
            XDocument xmlDocumentBusLine = XDocument.Load(@"..\bin\BusLine.xml");
            foreach (var lines in xmlDocumentBusLine.Descendants("BusLine"))// update first and last station for each line
            {
                if ((int)lines.Element("FirstStation") == key) // update first station
                {
                    var val = xmlDocumentLineSta.Descendants("LineStation").
                         FirstOrDefault(x => (int)x.Element("LineID") == (int)lines.Element("LineID") && (int)x.Element("LineStationIndex") == 1);
                    if (val == null) //if there are no other line station to this line
                        lines.Element("FirstStation").SetValue(0);
                    else
                        lines.Element("FirstStation").SetValue((int)val.Element("BusStationKey"));
                }
                if ((int)lines.Element("LastStation") == key) //update last station
                {
                    var val = xmlDocumentLineSta.Descendants("LineStation").FirstOrDefault(x => (int)x.Element("LineID") == (int)lines.Element("LineID") &&
                                         (int)x.Element("BusStationKey") == key);
                    if (val == null) //if there are no other line station to this line
                        lines.Element("LastStation").SetValue(0);
                    else
                    {
                        int index = (int)val.Element("LineStationIndex") - 1;
                        var val2 = xmlDocumentLineSta.Descendants("LineStation").FirstOrDefault(x =>
                               (int)x.Element("LineID") == (int)lines.Element("LineID") && (int)x.Element("LineStationIndex") == index);
                        if (val2 != null)
                            lines.Element("LastStation").SetValue((int)val2.Element("BusStationKey"));
                    }
                }
            }
            foreach (var lines in xmlDocumentLineSta.Descendants("LineStation")) // update lines index
            {
                if ((int)lines.Element("BusStationKey") == sta.BusStationKey)
                {
                    foreach (var line in xmlDocumentLineSta.Descendants("LineStation"))
                    {
                        if ((int)line.Element("LineID") == (int)lines.Element("LineID") && (int)line.Element("LineStationIndex") > (int)lines.Element("LineStationIndex"))
                        {
                            line.Element("LineStationIndex").SetValue((int)line.Element("LineStationIndex") - 1);
                        }
                    }
                }
            }
            xmlDocumentLineSta.Root.Elements().Where(x => (int)x.Element("BusStationKey") == key).Remove(); // remove all line station with this station
            xmlDocumentBusSta.Root.Elements().Where(x => (int)x.Element("BusStationKey") == key).FirstOrDefault().Remove();// remove this bus station
            xmlDocumentLineSta.Save(@"..\bin\LineStation.xml");
            xmlDocumentBusSta.Save(@"..\bin\BusStation.xml");
            xmlDocumentBusLine.Save(@"..\bin\BusLine.xml");
        }

        /// <summary>
        /// gets two stations and if they exist returns their pair station
        /// </summary>
        /// <param name="sta1">first station</param>
        /// <param name="sta2">second station</param>
        /// <returns></returns>
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

        /// <summary>
        /// gets two station from the user and update their time and distance 
        /// </summary>
        /// <param name="sta1">first station</param>
        /// <param name="sta2">second station</param>
        /// <param name="distance">distance between</param>
        /// <param name="averge">average time between</param>
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
            //remove if the pair of station alredy exist
            xmlDocumentPair.Root.Elements().Where(x => (int)x.Element("FirstKey") == sta1 && (int)x.Element("SecondKey") == sta2).Remove();
            xmlDocumentPair.Element("PairStations").Add( // add the new update pair by the user 
                  new XElement("PairStation",
                  new XElement("FirstKey", sta1),
                  new XElement("SecondKey", sta2),
                  new XElement("Distance", distance),
                  new XElement("AverageTime", averge)));
            xmlDocumentPair.Save(@"..\bin\PairStation.xml");
        }
        /// <summary>
        /// gets two stations and calculate their time and distance by coordinates and update the xml file
        /// </summary>
        /// <param name="sta1">first station</param>
        /// <param name="sta2">second station</param>
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
        /// <summary>
        /// the function get a line id
        /// and copy all of his trips to a IEnumerable
        /// </summary>
        /// <param name="lineID">the line id of which we want his trips</param>
        /// <returns>IEnumerable of all the trips of the wanted line</returns>
        public IEnumerable<LineTrip> GetAllLineTrip(int lineID)
        {
            XDocument xmlDocumentLine = XDocument.Load(@"..\bin\LineTrip.xml");
            return from line in xmlDocumentLine.Descendants("LineTrip")
                   where (int)line.Element("LineID") == lineID
                   select new DO.LineTrip
                   {
                       LineID = lineID,
                       StartAt = (TimeSpan)line.Element("StartAt")
                   };
        }

        #region User
        /// <summary>
        /// the function get a username and password 
        /// and check if thay are matching a manager
        /// </summary>
        /// <param name="username">The username who used to log to the program</param>
        /// <param name="password">The password who used to log to the program</param>
        /// <param name="manage"></param>
        /// <returns>true: if there a match. false:if there is'nt a match</returns>
        public bool CheckUser(string username, string password, bool manage)
        {
            XDocument xmlDocumentUser = XDocument.Load(@"..\bin\Users.xml");
            if (xmlDocumentUser.Descendants("User").Where(x => (string)x.Element("UserName") == username
             && (string)x.Element("Password") == password && (bool)x.Element("Management") == manage)
                  .FirstOrDefault() == null)
                return false;
            return true;
        }
        /// <summary>
        /// Add a new user 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="manage">if he have access</param>
        public void AddUser(string username, string password, bool manage)
        {
            XDocument xmlDocumentUser = XDocument.Load(@"..\bin\Users.xml");
            if (xmlDocumentUser.Descendants("User").FirstOrDefault(x => (string)x.Element("UserName") == username) != null)
                throw new DuplicateWaitObjectException("this username alredy exsit");
            xmlDocumentUser.Element("Users").Add
                (new XElement("User",
                new XElement("UserName", username),
                new XElement("Password", password),
                new XElement("Management", manage)));
            xmlDocumentUser.Save(@"..\bin\Users.xml");
        }
        #endregion
    }
}