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
        public Bus GetBus(int licenseNum)
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
