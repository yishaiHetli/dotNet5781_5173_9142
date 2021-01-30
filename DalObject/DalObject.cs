using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using DS;

namespace Dal
{
    sealed class DalObject : IDal
    {
        #region singelton
        static readonly IDal instance = new DalObject();
        static DalObject() { }// static ctor to ensure instance init is done just before first usage
        DalObject() { } // default => private
        public static IDal Instance { get => instance; }// The public Instance property to use
        #endregion

        #region bus 
        /// <summary>
        /// Add a new bus to data source list of busus 
        /// </summary>
        /// <param name="other">new bus to add</param>
        public void AddNewBus(Bus other)
        {
            int longs = other.LicenseNum.ToString().Length;
            if (longs < 7 || longs > 8)
                throw new DO.BadLisenceException(other.LicenseNum, "bus lisence nember is illegal");
            if ((longs == 8 && other.StartActivity.Year < 2018) ||
                (longs == 7 && other.StartActivity.Year >= 2018))
                throw new DO.BadLisenceException(other.LicenseNum, "bus lisence nember dont match the date of manfacture");
            if (DataSource.buss.FirstOrDefault(p => p.LicenseNum == other.LicenseNum) != null)
                throw new DO.BadLisenceException(other.LicenseNum, "Duplicate of lisence number");
            if (other.FuelInKm > 1200)
                other.FuelInKm = 1200;
            if (other.FuelInKm < 0)
                other.FuelInKm = 0;
            DataSource.buss.Add(other.Clone());
        }
        /// <summary>
        /// Returns all existing buses in the system
        /// </summary>
        /// <returns>all existing buses in the system</returns>
        public IEnumerable<DO.Bus> GetAllBuss()
        {
            return from bus in DataSource.buss
                   orderby bus.LicenseNum
                   select bus.Clone();
        }
        /// <summary>
        /// Gets bus by his license number
        /// </summary>
        /// <param name="licenseNum">Bus ID</param>
        /// <returns>The requested bus or throw an error if dont exsit</returns>
        public Bus GetBus(int licenseNum)
        {
            DO.Bus num = DataSource.buss.Find(p => p.LicenseNum == licenseNum);
            if (num != null)
                return num.Clone();
            else
                throw new DO.BadLisenceException(licenseNum, $"bad lisence number: {licenseNum}");
        }
        /// <summary>
        /// Remove bus by his license number
        /// </summary>
        /// <param name="license">Bus ID</param>
        public void RemoveBus(int license)
        {
            DO.Bus bus = DataSource.buss.Find(p => p.LicenseNum == license);

            if (bus != null)
            {
                DataSource.buss.Remove(bus);
            }
            else
                throw new DO.BadLisenceException(license, $"bad lisence number: {license}");
        }
        /// <summary>
        /// Updateing Bus by removing the exsit bus and add the sender bus
        /// </summary>
        /// <param name="bus">bus to replace</param>
        public void UpdateBus(Bus bus)
        {
            DO.Bus myBus = DataSource.buss.Find(p => p.LicenseNum == bus.LicenseNum);

            if (myBus != null)
            {
                DataSource.buss.Remove(myBus);
                DataSource.buss.Add(bus.Clone());
            }
            else
                throw new DO.BadLisenceException(bus.LicenseNum, $"bad lisence number: {bus.LicenseNum}");
        }
        #endregion

        #region BusLine
        /// <summary>
        /// returns IEnumerable of all the exsiting lines in the system
        /// </summary>
        /// <returns>IEnumerable of all the exsiting lines in the system</returns>
        public IEnumerable<DO.BusLine> GetAllLines()
        {
            return from bus in DataSource.busLine
                   select bus.Clone();
        }
        /// <summary>
        ///Delete selected bus line
        /// </summary>
        /// <param name="ID">ID of the bus line to remove</param>
        public void RemoveBusLine(int ID)
        {
            BusLine line = DataSource.busLine.Find(p => p.LineID == ID);
            if (line != null) // if the line exsit
            {
                DataSource.lineSta.RemoveAll(P => P.LineID == line.LineID); // remove all line stations
                DataSource.lineTrips.RemoveAll(P => P.LineID == line.LineID);// remove all line trips
                DataSource.busLine.Remove(line); // remove bus line

                foreach (var x in DataSource.busLine) // update the id for each bus line
                {
                    if (x.LineID < ID) continue;
                    --x.LineID;
                }
                foreach (var x in DataSource.lineSta) // update the id for each line station
                {
                    if (x.LineID < ID) continue;
                    --x.LineID;
                }
                foreach (var x in DataSource.lineTrips) // update the id for each line trip 
                {
                    if (x.LineID < ID) continue;
                    --x.LineID;
                }
                --BusLine.ID; // update the static bus line id
            }
        }
      
        /// <summary>
        /// returns IEnumerable of all Line Stations by line ID
        /// </summary>
        /// <param name="LineID">line ID</param>
        /// <returns>IEnumerable of all Line Stations by line ID</returns>
        public IEnumerable<LineStation> GetAllLineStations(int LineID)
        {
            IEnumerable<LineStation> line = DataSource.lineSta.FindAll(p => p.LineID == LineID);
            if (line == null)
                throw new DO.BadStationException(LineID, $"Line ID {LineID} dont match");
            return from l in line
                   orderby l.LineStationIndex
                   select l.Clone();
        }

        /// <summary>
        /// return IEnumerable of all line trip
        /// </summary>
        /// <param name="lineID">line ID</param>
        /// <returns>IEnumerable of all line trip</returns>
        public IEnumerable<LineTrip> GetAllLineTrip(int lineID)
        {
            return from lines in DataSource.lineTrips
                   where lines.LineID == lineID
                   select lines;
        }
        /// <summary>
        /// Add a new bus line to date source lists of BusLine
        /// </summary>
        /// <param name="bus">Bus line to add</param>
        public void AddNewBusLine(BusLine bus)
        {
            bus.LineID = BusLine.ID++;
            //update line station list with first and last station of this bus
            DataSource.lineSta.Add(new LineStation
            {
                LineID = bus.LineID,
                LineStationIndex = 0,
                BusStationKey = bus.FirstStation
            });
            DataSource.lineSta.Add(new LineStation
            {
                LineID = bus.LineID,
                LineStationIndex = 1,
                BusStationKey = bus.LastStation
            });
            DataSource.busLine.Add(bus.Clone());
            BusStation sta1 = DataSource.busSta.Find(s => s.BusStationKey == bus.FirstStation);
            BusStation sta2 = DataSource.busSta.Find(s => s.BusStationKey == bus.LastStation);
            updatePair(sta1, sta2); // update pair station list with this two stations
        }
        #endregion

        #region station
        /// <summary>
        /// returns IEnumerable of all the exsiting stations in the system
        /// </summary>
        /// <returns>IEnumerable of all the exsiting stations in the system</returns>
        public IEnumerable<BusStation> GetAllStation()
        {
            return from bus in DataSource.busSta
                   orderby bus.BusStationKey
                   select bus.Clone();
        }
        /// <summary>
        /// Add new stop to sender bus line 
        /// </summary>
        /// <param name="index">the index of this station</param>
        /// <param name="bus">bus line to add this stop</param>
        /// <param name="station">station to add</param>
        public void AddNewStop(int index, BusLine bus, BusStation station)
        {
            foreach (var x in DataSource.lineSta) // update all stations index that greater from this index
            {
                if (x.LineID != bus.LineID) continue;
                if (x.LineStationIndex < index) continue;
                ++x.LineStationIndex;
            }        
            DataSource.lineSta.Add(new LineStation
            {
                LineID = bus.LineID,
                LineStationIndex = index,
                BusStationKey = station.BusStationKey
            });
            if (DataSource.lineSta.FirstOrDefault(x => x.LineID == bus.LineID && x.LineStationIndex > index) == null) //If the index of the added station is last
            {
                DataSource.busLine.FirstOrDefault(x => x.LineID == bus.LineID).LastStation = station.BusStationKey; // update bus line last station 
            }
            if (index == 0) //If the index of the added station is first
            {
                DataSource.busLine.FirstOrDefault(x => x.LineID == bus.LineID).FirstStation = station.BusStationKey;
                LineStation nextLine = DataSource.lineSta.Find(l => l.LineID == bus.LineID && l.LineStationIndex == index + 1); // find the second station index
                BusStation nextSta = DataSource.busSta.Find(s => s.BusStationKey == nextLine.BusStationKey); // find the station of second station index
                updatePair(station, nextSta); // update pair station
                return;
            }
            // find the line stations of the index and before and after it
            LineStation line1 = DataSource.lineSta.Find(l => l.LineID == bus.LineID && l.LineStationIndex == index - 1);
            LineStation line2 = DataSource.lineSta.Find(l => l.LineID == bus.LineID && l.LineStationIndex == index);
            LineStation line3 = DataSource.lineSta.Find(l => l.LineID == bus.LineID && l.LineStationIndex == index + 1);
            BusStation sta1 = DataSource.busSta.Find(s => s.BusStationKey == line1.BusStationKey);
            BusStation sta2 = DataSource.busSta.Find(s => s.BusStationKey == line2.BusStationKey);
            updatePair(sta1, sta2);
            if (line3 != null) // if is not the last index 
            {
                BusStation sta3 = DataSource.busSta.Find(s => s.BusStationKey == line3.BusStationKey);
                updatePair(sta2, sta3);
            }
        }
        /// <summary>
        /// remove bus station by its key
        /// </summary>
        /// <param name="key">bus station key</param>
        public void RemoveSta(int key)
        {
            DO.BusStation sta = DataSource.busSta.Find(p => p.BusStationKey == key);// find the station to remove
            if (sta != null)
            {
                foreach (var lines in DataSource.busLine)// update first and last station for each line
                {
                    if (lines.FirstStation == key) // update first station
                    {
                        try
                        {
                            lines.FirstStation = DataSource.lineSta.FirstOrDefault(x => x.LineID == lines.LineID && x.LineStationIndex == 1).BusStationKey;
                        }
                        catch (Exception) { lines.FirstStation = 0; } //if there are no other line station to this line
                    }
                    if (lines.LastStation == key) // update last station
                    {
                        try
                        {
                            int index = DataSource.lineSta.FirstOrDefault(x => x.LineID == lines.LineID && x.BusStationKey == key).LineStationIndex - 1;
                            lines.LastStation = DataSource.lineSta.FirstOrDefault(x => x.LineID == lines.LineID && x.LineStationIndex == index).BusStationKey;
                        }
                        catch (Exception) { lines.LastStation = 0; } //if there are no other line station to this line
                    }
                }
                foreach (var lines in DataSource.lineSta) // update lines index
                {
                    if (lines.BusStationKey == sta.BusStationKey)
                    {
                        foreach (var line in DataSource.lineSta)
                        {
                            if (line.LineID == lines.LineID && line.LineStationIndex > lines.LineStationIndex)
                            {
                                --line.LineStationIndex;
                            }
                        }
                    }
                }
                DataSource.lineSta.RemoveAll(P => P.BusStationKey == sta.BusStationKey); // remove all line station that have this station
                DataSource.busSta.Remove(sta); // remove station
            }
        }

        /// <summary>
        /// return a pair of consecutive stations 
        /// </summary>
        /// <param name="sta1">first station</param>
        /// <param name="sta2">second station</param>
        /// <returns>a pair of consecutive stations</returns>
        public PairStations GetPair(int sta1, int sta2)
        {
            DO.PairStations pair = DataSource.pairSta.Find(p => p.FirstKey == sta1
            && p.SecondKey == sta2);
            if (pair != null)
                return pair.Clone();
            return null;
        }
        /// <summary>
        /// Update pair station by coordinates
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
                DataSource.pairSta.Add(new PairStations
                {
                    FirstKey = sta1.BusStationKey,
                    SecondKey = sta2.BusStationKey,
                    Distance = cord,
                    AverageTime = TimeSpan.FromMinutes(cord * 2)
                });
            }
        }
        /// <summary>
        /// Update pair of station by distance and time that the user send
        /// </summary>
        /// <param name="sta1">first station</param>
        /// <param name="sta2">second station</param>
        /// <param name="distance">distance between send stations</param>
        /// <param name="averge">average time between sended stations</param>
        public void UpdatePairByUser(int sta1, int sta2, double distance , TimeSpan average)
        {
            if (DataSource.busSta.FirstOrDefault(x => x.BusStationKey == sta1) == null ||
                DataSource.busSta.FirstOrDefault(x => x.BusStationKey == sta2) == null) // if one of the stations was not found
            {
                throw new DO.BadStationException(sta1, "there is no such station");
            }

            DO.PairStations pair = DataSource.pairSta.Find(p => p.FirstKey == sta1
              && p.SecondKey == sta2);
            if (pair != null) // if this two stations alredy update as pair station 
            {
                pair.AverageTime = average;
                pair.Distance = distance;
            }
            else 
            {
                DataSource.pairSta.Add(new PairStations
                {
                    FirstKey = sta1,
                    SecondKey = sta2,
                    AverageTime = average,
                    Distance = distance
                });
            }
        }
        /// <summary>
        /// Add a new bus station
        /// </summary>
        /// <param name="station">station to add</param>
        public void AddBusStation(BusStation station)
        {
            DataSource.busSta.Add(station);
        }
        #endregion

        #region User
        /// <summary>
        /// bool founction that check if this user and password exsit or not
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="manage"></param>
        /// <returns>true if the user exsit and false if not</returns>
        public bool CheckUser(string userName, string password, bool manage)
        {
            if (DataSource.userList.FirstOrDefault(u => u.UserName == userName
          && u.Password == password && u.Management == manage) == null)
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
            if (DataSource.userList.FirstOrDefault(x => x.UserName == username) != null)
                throw new DuplicateWaitObjectException("this username alredy exsit");
            DataSource.userList.Add(new Users
            {
                UserName = username,
                Password = password,
                Management = manage
            });
        }
        #endregion
    }
}
