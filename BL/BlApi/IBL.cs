﻿using System;
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
    }
}