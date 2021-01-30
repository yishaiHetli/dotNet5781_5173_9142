using System;
using System.Collections.Generic;
using BO;

namespace BLApi
{
    public interface IBL
    {
        #region Bus
        IEnumerable<BO.Bus> GetAllBuss();
        BO.Bus GetBus(int l);
        void GetRefule(BO.Bus other);
        void GetRepair(BO.Bus other);
        void RemoveBus(int lisence);
        void AddNewBus(BO.Bus addedBus);
        #endregion
        #region Lines
        IEnumerable<BO.BusLine> GetAllLines();
        void AddNewBusLine(BO.BusLine bus);
        void RemoveLine(BO.BusLine line);
        #endregion
        #region Stations
        IEnumerable<BO.BusStation> GetAllStations();
        void AddStop(int index, BO.BusLine bus, BO.BusStation station);
        void AddBusStation(BO.BusStation station);
        void RemoveSta(BO.BusStation sta);
        void PairUpdate(int sta1, int sta2, double distance, TimeSpan average);
        List<StationDistance> Avarge(BO.BusStation station, TimeSpan time);
        #endregion
        #region Users
        bool userCheck(string name, string password, bool manage);
        void AddUser(string name, string password, bool manage);
        #endregion
    }
}
