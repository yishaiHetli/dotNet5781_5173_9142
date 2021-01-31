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
        /// <summary>
        /// convert bus in type of DO to BO
        /// </summary>
        /// <param name="busDO">bus to convert</param>
        /// <returns>bus in typ of BO</returns>
        BO.Bus BusDoBoAdapter(DO.Bus busDO)
        {
            BO.Bus busBO = new BO.Bus();
            busDO.CopyPropertiesTo(busBO);
            return busBO;
        }
        /// <summary>
        ///  convert bus in type of BO to DO
        /// </summary>
        /// <param name="busBO">bus to convert</param>
        /// <returns>bus in typ of DO</returns>
        DO.Bus BusBoDoAdapter(BO.Bus busBO)
        {
            DO.Bus busDO = new DO.Bus();
            busBO.CopyPropertiesTo(busDO);
            return busDO;
        }
        /// <summary>
        /// refule the sended bus
        /// </summary>
        /// <param name="other">bus that send to refule</param>
        public void GetRefule(BO.Bus other)
        {
            other.FuelInKm = 1200;
            DO.Bus bus = BusBoDoAdapter(other); // convert the type to DO
            dl.UpdateBus(bus);
        }
        /// <summary>
        /// repair the sended bus
        /// </summary>
        /// <param name="other">bus that send to repair</param>
        public void GetRepair(BO.Bus other)
        {
            other.FuelInKm = 1200;
            other.TotalKm = 0;
            DO.Bus bus = BusBoDoAdapter(other); // convert the type to DO
            dl.UpdateBus(bus);
        }
        /// <summary>
        /// get all the buses from dl and return it
        /// </summary>
        /// <returns>IEnumerable that contain all the buses in the system</returns>
        public IEnumerable<BO.Bus> GetAllBuss()
        {
            return from item in dl.GetAllBuss()
                   orderby item.LicenseNum
                   select BusDoBoAdapter(item);
        }
        /// <summary>
        /// return BO bus by the lisence number
        /// </summary>
        /// <param name="lisence">bus id</param>
        /// <returns>BO bus by the lisence number</returns>
        public BO.Bus GetBus(int lisence)
        {
            try { return BusDoBoAdapter(dl.GetBus(lisence)); }
            catch (DO.BadLisenceException ex)
            { throw new BO.BadLisenceException($"bus number {lisence} does not exist", ex); }
        }
        /// <summary>
        /// remove bus with this license number
        /// </summary>
        /// <param name="lisence">bus id</param>
        public void RemoveBus(int lisence)
        {
            try
            {
                dl.RemoveBus(lisence);
            }
            catch (DO.BadLisenceException ex)
            { throw new BO.BadLisenceException($"bus number {lisence} does not exist", ex); }
        }
        /// <summary>
        /// add a new bus that sended 
        /// </summary>
        /// <param name="addedBus">new bus to add</param>
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
        /// <summary>
        /// convert DO line station to BO
        /// </summary>
        /// <param name="busDO">line station DO to convert</param> 
        /// <returns>line station in type of BO</returns>
        BO.LineStation LineStaDoBoAdapter(DO.LineStation busDO)
        {
            BO.LineStation busBO = new BO.LineStation();
            busDO.CopyPropertiesTo(busBO);
            return busBO;
        }
        /// <summary>
        /// convert DO line trip to BO
        /// </summary>
        /// <param name="busDO">line trip DO to convert</param>
        /// <returns>line trip in type of BO</returns>
        BO.LineTrip LineTripDOBOAdapter(DO.LineTrip busDO)
        {
            BO.LineTrip busBO = new BO.LineTrip();
            busDO.CopyPropertiesTo(busBO);
            return busBO;
        }
        /// <summary>
        /// convert DO bus line to BO 
        /// </summary>
        /// <param name="busDO">bus line DO to convert</param>
        /// <returns>bus line in type of BO</returns>
        BO.BusLine BusLineDoBoAdapter(DO.BusLine busDO)
        {
            BO.BusLine busBO = new BO.BusLine();
            busDO.CopyPropertiesTo(busBO);
            //get all lines station from dl
            busBO.LinesSta = from line in dl.GetAllLineStations(busBO.LineID)
                             orderby line.LineStationIndex
                             select LineStaDoBoAdapter(line);
            bool notFirstSta = false;
            int first = 0;
            List<BO.LineStation> linesSta = busBO.LinesSta.ToList(); // get the ienumerable into list
            foreach (var item in linesSta) // update foreach line station his distance and time from last station
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
            busBO.LinesExit = from line in dl.GetAllLineTrip(busBO.LineID) // get all lines trip from dl
                              orderby line.StartAt
                              select LineTripDOBOAdapter(line);

            TimeSpan time = TimeSpan.Zero;
            foreach (var line in busBO.LinesSta) //collect the average time of travel
            {
                time += line.AverageTime;
            }
            List<BO.LineTrip> lines = busBO.LinesExit.ToList(); // get the ienumerable into list
            foreach (var line in lines) // add the travel time + start to each line exsit at its finish
            {
                line.FinishAt = line.StartAt + time; 
            }
            busBO.LinesExit = lines; 
            return busBO;
        }
        /// <summary>
        /// convert BO bus line to DO
        /// </summary>
        /// <param name="busBO">bus line BO to convert</param>
        /// <returns>bus line in type of DO</returns>
        DO.BusLine BusLineBoDoAdapter(BO.BusLine busBO)
        {
            DO.BusLine busDO = new DO.BusLine();
            busBO.CopyPropertiesTo(busDO);
            return busDO;
        }
        /// <summary>
        /// gets all bus lines from dl
        /// </summary>
        /// <returns>all the bus lines there are in the system</returns>
        public IEnumerable<BO.BusLine> GetAllLines()
        {
            return from item in dl.GetAllLines()
                   orderby item.LineID
                   select BusLineDoBoAdapter(item);
        }
        /// <summary>
        /// add a new sended bus line 
        /// </summary>
        /// <param name="bus">bus line to add</param>
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
        /// <summary>
        /// remove sended line
        /// </summary>
        /// <param name="line">line to remove</param>
        public void RemoveLine(BO.BusLine line)
        {
            dl.RemoveBusLine(line.LineID);
        }
        #endregion

        #region User
        /// <summary>
        /// check if the user is registered in the system
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="manage">if he have access</param>
        /// <returns></returns>
        public bool userCheck(string username, string password, bool manage)
        {
            if (dl.CheckUser(username, password, manage))
                return true;
            return false;
        }
        /// <summary>
        /// add a new user that can have access  
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="manage">if he have access</param>
        public void AddUser(string username, string password, bool manage)
        {
            try
            {
                dl.AddUser(username, password, manage);
            }
            catch (Exception ex)
            {
                throw new DuplicateWaitObjectException($"{ex.Message}");
            }
        }
        #endregion
        #region Stations
        DO.BusStation BusStationBoDoAdapter(BO.BusStation busBO)
        {
            DO.BusStation busDO = new DO.BusStation();
            busBO.CopyPropertiesTo(busDO);
            return busDO;
        }
        /// <summary>
        /// Add a station to bus line in a specific position
        /// </summary>
        /// <param name="index">station location</param>
        /// <param name="bus">bus line to add this stop</param>
        /// <param name="station">new stop</param>
        public void AddStop(int index, BO.BusLine bus, BO.BusStation station)
        {
            if (index > bus.LinesSta.ToList().Count())
                throw new IndexOutOfRangeException("index too big");
            if (bus.LinesSta.FirstOrDefault(P => P.BusStationKey == station.BusStationKey) != null)
            {
                throw new DuplicateWaitObjectException("this station already exist in this bus route");
            }
            dl.AddNewStop(index, BusLineBoDoAdapter(bus), BusStationBoDoAdapter(station));
        }
        /// <summary>
        /// add a new station
        /// </summary>
        /// <param name="station">bus station to add</param>
        public void AddBusStation(BO.BusStation station)
        {
            List<BO.BusStation> busStations = GetAllStations().ToList();
            foreach (var item in busStations)//check if the station alredy exsit
            {
                if (item.Latitude == station.Latitude && item.Longitude == station.Longitude) // if alredy have station in this location
                {
                    throw new DuplicateWaitObjectException("there is another station at this location");
                }
                if (item.BusStationKey == station.BusStationKey)  // if the bus station key alredy exist
                {
                    throw new DuplicateWaitObjectException("this code already exist");
                }
                if (item.Name == station.Name) // if the station name alredy exsit
                {
                    throw new DuplicateWaitObjectException("this name already exist");
                }
            }
            dl.AddBusStation(BusStationBoDoAdapter(station));
        }
        /// <summary>
        /// convert DO bus station to BO
        /// </summary>
        /// <param name="busDO">bus station DO to convert</param>
        /// <returns>bus station in type of BO</returns>
        BO.BusStation BusStationDoBoAdapter(DO.BusStation busDO)
        {
            BO.BusStation busBO = new BO.BusStation();
            busDO.CopyPropertiesTo(busBO);
            busBO.LineInStation = from line in GetAllLines() // gets all lines that passing through this station
                                  from lines in line.LinesSta
                                  where lines.BusStationKey == busBO.BusStationKey
                                  orderby line.LineID
                                  select line;
            return busBO;
        }
        /// <summary>
        /// get all the station from dl
        /// </summary>
        /// <returns>all the buses stations there are in the system</returns>
        public IEnumerable<BO.BusStation> GetAllStations()
        {
            return from item in dl.GetAllStation()
                   orderby item.BusStationKey
                   select BusStationDoBoAdapter(item);
        }
        /// <summary>
        /// remove bus station from the system
        /// </summary>
        /// <param name="sta">bus station to remove</param>
        public void RemoveSta(BO.BusStation sta)
        {
            dl.RemoveSta(sta.BusStationKey);
        }
        /// <summary>
        /// Update time and distance between two station
        /// </summary>
        /// <param name="sta1">first station</param>
        /// <param name="sta2">second station</param>
        /// <param name="distance">distance between</param>
        /// <param name="average">average time between</param>
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
        /// <summary>
        ///gets a time and station and check the 5 first bus lines that 
        ///came to this bus station at this time 
        ///and one line that already past there
        /// </summary>
        /// <param name="station">bus station to check</param>
        /// <param name="time">time that the user wait in the station</param>
        /// <returns></returns>
        public List<StationDistance> Avarge(BO.BusStation station, TimeSpan time)
        {
            List<StationDistance> list = new List<StationDistance>(); // list of all lines that past in this station and its time
            foreach (var x in station.LineInStation) // loop on all lines in this station
            {
                TimeSpan average = TimeSpan.Zero;
                foreach (var y in x.LinesSta) // collect time until it came to this station 
                {
                    average += y.AverageTime;
                    if (station.BusStationKey == y.BusStationKey)
                    {
                        foreach (var item in x.LinesExit) // loop of avrey line exsit foreach bus line and add this line and time to a new list
                        {
                            TimeSpan _average = average;
                            _average += item.StartAt;
                            list.Add(new StationDistance
                            {
                                Average = _average,
                                LineID = x.LineID,
                                StartTime = item.StartAt,
                                FirstStation = x.FirstStation,
                                LineNumber = x.LineNumber,
                                LastStation = x.LastStation
                            });
                        }
                        break;
                    }
                }
            }
            list = list.OrderBy(x => x.Average).ToList(); // sort the list of station distance by the time
            for (int i = 0; i < list.Count(); i++) // loop until it came to time
            {
                if (list[i].Average >= time)
                {
                    List<StationDistance> list2 = new List<StationDistance>(); // list of only 5 first that came in this station and 1 before them
                    if (i != 0)
                        list2.Add(list[i - 1]);// add the last line that past in this station
                    for (int j = 0; i < list.Count() && j < 5; j++) // add the first 5 lines that arrive to this station
                    {
                        list2.Add(new StationDistance
                        {
                            Average = list[i].Average - time,
                            LineID = list[i].LineID,
                            StartTime = list[i].StartTime,
                            FirstStation = list[i].FirstStation,
                            LineNumber = list[i].LineNumber,
                            LastStation = list[i].LastStation

                        });
                        i++;
                    }
                    return list2;
                }
            }
            return null;
        }
        #endregion
    }
}
