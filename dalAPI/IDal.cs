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
        void GetRefule(int lisence);
        void GetRepair(int lisence);
        #endregion

        IEnumerable<BusStation> GetAllStation();
        BusStation GetBusStation(int busStationKey);

        #region Lines
        IEnumerable<BusLine> GetAllLines();
        BusLine GetBusLine(int id, BusStation first, BusStation last);
        IEnumerable<LineStation> GetAllLineStations(int LineID);
        void AddNewBusLine(BusLine bus);
        #endregion
        void AddNewStop(int index, BusLine bus, BusStation station);
        bool CheckUser(string username, string password, bool manage);
    }
}
