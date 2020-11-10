using System;
using System.Collections;
using System.Collections.Generic;

namespace dotNet5781_02_5713_9142
{
        public class BusCompany : IEnumerable<Bus>
        {
            private List<int> numbers = new List<int>();

            private List<Bus> busses;

            public BusCompany()
            {
                busses = new List<Bus>();
            }
            public void Add(Bus bus)
            {
                if (numbers.Count != 0 && numbers.Contains(bus.Mispar))
                {
                    throw new ArgumentException("mispar kvar kayam bachevra");
                }
                busses.Add(bus);
                numbers.Add(bus.Mispar);
            }

            public IEnumerator<Bus> GetEnumerator()
            {
                return busses.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            //private List<BusLine> buses = new List<BusLine>();

            //public List<BusLine> Busses
            //{
            //    get { return buses; }
            //}
        }
    }
