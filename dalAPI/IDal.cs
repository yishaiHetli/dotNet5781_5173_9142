using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDal
    {
        #region Bus        
        void AddNewBus(Bus other);
        void RemoveBus(int license);
        Bus GetBus(int licenseNum);
        void UpdateBus(Bus bus);
        IEnumerable<Bus> GetAllBuss();
        #endregion

        #region Lines
        IEnumerable<BusLine> GetAllLines();
        void RemoveBusLine(int ID);
        IEnumerable<LineStation> GetAllLineStations(int LineID);
        void AddNewBusLine(BusLine bus);
        IEnumerable<LineTrip> GetAllLineTrip(int lineID);
        #endregion

        #region Stations
        IEnumerable<BusStation> GetAllStation();
        void AddNewStop(int index, BusLine bus, BusStation station);
        void AddBusStation(BusStation station);
        void RemoveSta(int ID);
        #endregion

        #region pair
        PairStations GetPair(int sta1, int sta2);
        void UpdatePairByUser(int sta1, int sta2, double distance, TimeSpan averge);
        #endregion

        #region user
        bool CheckUser(string username, string password, bool manage);
        void AddUser(string username, string password, bool manage);
        #endregion
    }
}
