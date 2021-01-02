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
        public IEnumerable<BO.Bus> GetAllBuss()
        {
            return from item in dl.GetAllBuss()
                   select BusDoBoAdapter(item);
        }

        public void GetRefule(BO.Bus other)
        {
            try
            {
                dl.GetRefule(other.LicenseNum);
            }
            catch (DO.BadLisenceException ex)
            {
                throw new BO.BadLisenceException($"bus number {other.LicenseNum} does not exist", ex);
            }
        }

        public void GetRepair(BO.Bus other)
        {
            try
            {
                dl.GetRepair(other.LicenseNum);
            }
            catch (DO.BadLisenceException ex)
            {
                throw new BO.BadLisenceException($"bus number {other.LicenseNum} does not exist", ex);
            }
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
    }
}
