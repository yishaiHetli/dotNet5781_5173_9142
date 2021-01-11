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
        static readonly DalObject instance = new DalObject();
        static DalObject() { }// static ctor to ensure instance init is done just before first usage
        DalObject() { } // default => private
        public static DalObject Instance { get => instance; }// The public Instance property to use
        #endregion

        #region bus 
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
        public IEnumerable<DO.Bus> GetAllBuss()
        {
            return from bus in DataSource.buss
                   orderby bus.LicenseNum
                   select bus.Clone();
        }

        public Bus GetBus(int licenseNum)
        {
            DO.Bus num = DataSource.buss.Find(p => p.LicenseNum == licenseNum);
            if (num != null)
                return num.Clone();
            else
                throw new DO.BadLisenceException(licenseNum, $"bad lisence number: {licenseNum}");
        }

        public void GetRefule(int lisence)
        {
            Bus bus = DataSource.buss.FirstOrDefault(b => b.LicenseNum == lisence);
            if (bus == null)
                throw new DO.BadLisenceException(lisence, $"this bus dont exsit");
            bus.FuelInKm = 1200;
        }

        public void GetRepair(int lisence)
        {
            Bus bus = DataSource.buss.FirstOrDefault(b => b.LicenseNum == lisence);
            if (bus == null)
                throw new DO.BadLisenceException(lisence, $"this bus dont exsit");
            bus.FuelInKm = 1200;
            bus.TotalKm = 0;
        }

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
        public IEnumerable<DO.BusLine> GetAllLines()
        {
            return from bus in DataSource.busLine
                   orderby bus.LineID
                   select bus.Clone();

        }
        public BusLine GetBusLine(int id, BusStation first, BusStation last)
        {
            DO.BusLine num = DataSource.busLine.Find(p => p.LineID == id && p.FirstStation == first.BusStationKey
            && p.LastStation == last.BusStationKey);
            if (num != null)
                return num.Clone();
            else
                throw new DO.BadLisenceException(id, $"bad ID number: {id}");
        }

        public IEnumerable<BusStation> GetAllStation()
        {
            return from bus in DataSource.busSta
                   orderby bus.BusStationKey
                   select bus.Clone();
        }

        public IEnumerable<LineStation> GetAllLineStations(int LineID)
        {
            IEnumerable<LineStation> line = DataSource.lineSta.FindAll(p => p.LineID == LineID);
            if (line == null)
                throw new DO.BadLisenceException(LineID, $"Line ID {LineID} dont match");
            return from l in line
                   orderby l.LIneStationIndex
                   select l.Clone();
        }
        public BusStation GetBusStation(int busStationKey)
        {
            DO.BusStation num = DataSource.busSta.Find(p => p.BusStationKey == busStationKey);
            if (num != null)
                return num.Clone();
            else
                throw new DO.BadLisenceException(busStationKey, $"bad lisence number: {busStationKey}");
        }

        public void AddNewBusLine(BusLine bus)
        {
            bus.LineID = BusLine.ID++;
            DataSource.lineSta.Add(new LineStation
            {
                LineID = bus.LineID,
                LIneStationIndex = 0,
                BusStationKey = bus.FirstStation
            });
            DataSource.lineSta.Add(new LineStation
            {
                LineID = bus.LineID,
                LIneStationIndex = 1,
                BusStationKey = bus.LastStation
            });
            DataSource.busLine.Add(bus.Clone());
            BusStation sta1 = DataSource.busSta.Find(s => s.BusStationKey == bus.FirstStation);
            BusStation sta2 = DataSource.busSta.Find(s => s.BusStationKey == bus.LastStation);
            updatePair(sta1, sta2);
        }
        #endregion

        public bool CheckUser(string userName, string password, bool manage)
        {
            if (DataSource.userList.FirstOrDefault(u => u.UserName == userName
          && u.Password == password && u.Management == manage) == null)
                return false;
            return true;
        }
        public void AddNewStop(int index, BusLine bus, BusStation station)
        {

            foreach (var x in DataSource.lineSta)
            {
                if (x.LineID != bus.LineID) continue;
                if (x.LIneStationIndex < index) continue;
                ++x.LIneStationIndex;
            }
            DataSource.lineSta.Add(new LineStation
            {
                LineID = bus.LineID,
                LIneStationIndex = index,
                BusStationKey = station.BusStationKey
            });

            if (index == 0)
            {
                LineStation nextLine = DataSource.lineSta.Find(l => l.LineID == bus.LineID && l.LIneStationIndex == index + 1);
                BusStation nextSta = DataSource.busSta.Find(s => s.BusStationKey == nextLine.BusStationKey);
                updatePair(station, nextSta);
                return;
            }
            LineStation line1 = DataSource.lineSta.Find(l => l.LineID == bus.LineID && l.LIneStationIndex == index - 1);
            LineStation line2 = DataSource.lineSta.Find(l => l.LineID == bus.LineID && l.LIneStationIndex == index);
            LineStation line3 = DataSource.lineSta.Find(l => l.LineID == bus.LineID && l.LIneStationIndex == index + 1);
            BusStation sta1 = DataSource.busSta.Find(s => s.BusStationKey == line1.BusStationKey);
            BusStation sta2 = DataSource.busSta.Find(s => s.BusStationKey == line2.BusStationKey);
            updatePair(sta1, sta2);
            if (line3 != null)
            {
                BusStation sta3 = DataSource.busSta.Find(s => s.BusStationKey == line3.BusStationKey);
                updatePair(sta2, sta3);
            }
        }
        public PairStations GetPair(int sta1, int sta2)
        {
            DO.PairStations pair = DataSource.pairSta.Find(p => p.FirstKey == sta1
            && p.SecondKey == sta2);
            return pair;
        }
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
        public void AddBusStation(BusStation station)
        {
            DataSource.busSta.Add(station);
        }
        public void RemoveBusLine(int ID)
        {
            BusLine line = DataSource.busLine.Find(p => p.LineID == ID);
            if (line != null)
                DataSource.busLine.Remove(line);
        }
        public void RemoveSta(int ID)
        {
            DO.BusStation sta = DataSource.busSta.Find(p => p.BusStationKey == ID);
            if (sta != null)
                DataSource.busSta.Remove(sta);
        }
    }
}
