﻿using System;
using System.Collections.Generic;
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
                LineID =bus.LineID,
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
        }
    }
}
