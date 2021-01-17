using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BLApi;
using DO;
using BO;

namespace BL
{
    class BlImp : IBL
    {
        IDal dl = DLFactory.GetDL();
        #region singelton
        static readonly IBL instance = new BlImp();
        static BlImp() { }// static ctor to ensure instance init is done just before first usage
        BlImp() { } // default => private
        public static IBL Instance { get => instance; }// The public Instance property to use
        #endregion
        #region Buses CUR 
        BO.Bus BusDoBoAdapter(DO.Bus busDO)
        {
            BO.Bus busBO = new BO.Bus();
            busDO.CopyPropertiesTo(busBO);
            return busBO;
        }
        DO.Bus BusBoDoAdapter(BO.Bus busBO)
        {
            DO.Bus busDO = new DO.Bus();
            busBO.CopyPropertiesTo(busDO);
            return busDO;
        }
        public void GetRefule(BO.Bus other)
        {
            other.FuelInKm = 1200;
            DO.Bus bus = BusBoDoAdapter(other);
            dl.UpdateBus(bus);
        }
        public void GetRepair(BO.Bus other)
        {
            other.FuelInKm = 1200;
            other.TotalKm = 0;
            DO.Bus bus = BusBoDoAdapter(other);
            dl.UpdateBus(bus);
        }

        public IEnumerable<BO.Bus> GetAllBuss()
        {
            return from item in dl.GetAllBuss()
                   orderby item.LicenseNum
                   select BusDoBoAdapter(item);
        }

        public BO.Bus GetBus(int lisence)
        {
            try { return BusDoBoAdapter(dl.GetBus(lisence)); }
            catch (DO.BadLisenceException ex)
            { throw new BO.BadLisenceException($"bus number {lisence} does not exist", ex); }
        }

        public void RemoveBus(int lisence)
        {
            try
            {
                dl.RemoveBus(lisence);
            }
            catch (DO.BadLisenceException ex)
            { throw new BO.BadLisenceException($"bus number {lisence} does not exist", ex); }
        }
        public void AddNewBus(BO.Bus addedBus)
        {
            try
            {
                dl.AddNewBus(BusBoDoAdapter(addedBus));
            }
            catch (DO.BadLisenceException ex)
            {
                throw new BO.BadLisenceException($"{ex.Message}", ex);
            }
        }
        #endregion
        #region Lines CUR
        BO.LineStation LineStaDoBoAdapter(DO.LineStation busDO)
        {
            BO.LineStation busBO = new BO.LineStation();
            busDO.CopyPropertiesTo(busBO);
            return busBO;
        }
        BO.BusLine BusLineDoBoAdapter(DO.BusLine busDO)
        {
            BO.BusLine busBO = new BO.BusLine();
            busDO.CopyPropertiesTo(busBO);
            busBO.LinesSta = from line in dl.GetAllLineStations(busBO.LineID)
                             orderby line.LIneStationIndex
                             select LineStaDoBoAdapter(line);
            bool notFirstSta = false;
            int first = 0;
            List<BO.LineStation> linesSta = busBO.LinesSta.ToList();
            foreach (var item in linesSta)
            {
                if (notFirstSta)
                {
                    DO.PairStations pair = dl.GetPair(first, item.BusStationKey);
                    if (pair != null)
                    {
                        item.Distance = pair.Distance;
                        item.AverageTime = pair.AverageTime;
                    }
                }

                first = item.BusStationKey;
                notFirstSta = true;
            }
            busBO.LinesSta = linesSta;
            return busBO;
        }
        

        DO.BusLine BusLineBoDoAdapter(BO.BusLine busBO)
        {
            DO.BusLine busDO = new DO.BusLine();
            busBO.CopyPropertiesTo(busDO);
            return busDO;
        }
     
        public IEnumerable<BO.BusLine> GetAllLines()
        {
            return from item in dl.GetAllLines()
                   orderby item.LineID
                   select BusLineDoBoAdapter(item);
        }
        public void AddNewBusLine(BO.BusLine bus)
        {
            try
            {
                dl.AddNewBusLine(BusLineBoDoAdapter(bus));
            }
            catch (DO.BadLisenceException ex)
            {
                throw new BO.BadLisenceException($"{ex.Message}", ex);
            }

        }
        public void RemoveLine(BO.BusLine line)
        {
            dl.RemoveBusLine(line.LineID);
        }
       
        #endregion
        #region user
        public bool userCheck(string name, string password, bool manage)
        {
            if (dl.CheckUser(name, password, manage))
                return true;
            return false;
        }
        #endregion
        #region Stations
        DO.BusStation BusStationBoDoAdapter(BO.BusStation busBO)
        {
            DO.BusStation busDO = new DO.BusStation();
            busBO.CopyPropertiesTo(busDO);
            return busDO;
        }

        public void AddStop(int index, BO.BusLine bus, BO.BusStation station)
        {
            if (index > bus.LinesSta.ToList().Count())
                throw new IndexOutOfRangeException("index too big");
            var linesta = bus.LinesSta.FirstOrDefault(P => P.BusStationKey == station.BusStationKey);
            if (linesta != null)
            {
                throw new DuplicateWaitObjectException("this station already exist in this bus route");
            }
            dl.AddNewStop(index, BusLineBoDoAdapter(bus), BusStationBoDoAdapter(station));
        }
        public void AddBusStation(BO.BusStation station)
        {
            List<BO.BusStation> busStations = GetAllStations().ToList();
            foreach (var item in busStations)
            {
                if (item.Latitude == station.Latitude && item.Longitude == station.Longitude)
                {
                    throw new DuplicateWaitObjectException("there is another station at this location");
                }
                if (item.BusStationKey == station.BusStationKey)
                {
                    throw new DuplicateWaitObjectException("this code already exist");
                }
                if (item.Name == station.Name)
                {
                    throw new DuplicateWaitObjectException("this name already exist");
                }
            }
            dl.AddBusStation(BusStationBoDoAdapter(station));

        }
        BO.BusStation BusStationDoBoAdapter(DO.BusStation busDO)
        {
            BO.BusStation busBO = new BO.BusStation();
            busDO.CopyPropertiesTo(busBO);
            busBO.LineInStation = from line in GetAllLines()
                                  from lines in line.LinesSta
                                  where lines.BusStationKey == busBO.BusStationKey
                                  orderby line.LineID
                                  select line;
            return busBO;
        }
        public IEnumerable<BO.BusStation> GetAllStations()
        {
            return from item in dl.GetAllStation()
                   orderby item.BusStationKey
                   select BusStationDoBoAdapter(item);
        }

        public void RemoveSta(BO.BusStation sta)
        {
            dl.RemoveSta(sta.BusStationKey);
        }

        public void PairUpdate(int sta1,int sta2 , double distance, TimeSpan average)
        {
            if (sta1 == sta2)
            {
                throw new DuplicateWaitObjectException("you cannot select the same station twice");
            }    
            try
            {
                dl.UpdatePairByUser(sta1, sta2, distance, average);
            }
            catch (DO.BadStationException ex)
            {
                throw new BO.BadStationException(ex.Message,ex);
            }
        }
        #endregion
    }
}
