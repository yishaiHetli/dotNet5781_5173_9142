using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BLApi
{
    public interface IBL
    {
        IEnumerable<BO.Bus> GetAllBuss();
        BO.Bus GetBus(int l);
        void GetRefule(BO.Bus other);
        void GetRepair(BO.Bus other);
        void RemoveBus(int lisence);
        void AddNewBus(BO.Bus addedBus);
        IEnumerable<BO.BusLine> GetAllLines();
        IEnumerable<BO.BusStation> GetAllStations();
        void AddNewBusLine(BO.BusLine bus);
        void AddStop(int index, BO.BusLine bus, BO.BusStation station);
        bool userCheck(string name, string password, bool manage);
        void AddBusStation(BO.BusStation station);
        void RemoveLine(BO.BusLine line);
        void RemoveSta(BO.BusStation sta);
    }
}
