using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal
{
    class DalXml : IDal
    {
        public void AddNewBus(Bus other)
        {
            throw new NotImplementedException();
        }

        public void AddNewBusLine(BusLine bus)
        {
            throw new NotImplementedException();
        }

        public void AddNewStop(int index, BusLine bus, BusStation station)
        {
            throw new NotImplementedException();
        }

        public bool CheckUser(string username, string password, bool manage)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusLine> GetAllLines()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineStation> GetAllLineStations(int LineID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusStation> GetAllStation()
        {
            throw new NotImplementedException();
        }

        public Bus GetBus(int licenseNum)
        {
            throw new NotImplementedException();
        }

        public BusLine GetBusLine(int id, BusStation first, BusStation last)
        {
            throw new NotImplementedException();
        }

        public BusStation GetBusStation(int busStationKey)
        {
            throw new NotImplementedException();
        }

        public void GetRefule(int lisence)
        {
            throw new NotImplementedException();
        }

        public void GetRepair(int lisence)
        {
            throw new NotImplementedException();
        }

        public void RemoveBus(Bus other)
        {
            throw new NotImplementedException();
        }

        public void RemoveBus(int license)
        {
            throw new NotImplementedException();
        }

        public void UpdateBus(Bus bus)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Bus> IDal.GetAllBuss()
        {
            throw new NotImplementedException();
        }
    }
}
