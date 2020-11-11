using System;
using System.Collections;
using System.Collections.Generic;

namespace dotNet5781_02_5713_9142
{
    public class BusCompany : IEnumerable<Bus>
    {
        public int NumLine { get; set; }
        public DateTime StartYear { get; set; }

        public override string ToString()
        {
            return String.Format("Bus {0} was klita be {1}", NumLine, StartYear.Year.ToString());
        }

        private List<BusLine> busses = new List<BusLine>;
        public int NumLine { get; set; }
        public BusCompany()
        {

        }
        public void Add(int key)
        {
            int j = 0;
            for (int i = 0; i < busses.Count; i++)
            {
                if (busses[i].Number == key)
                    ++j;
            }
            if (j >= 2)
                throw new ArgumentException("the number is already exist");
            busses.Add();
        }
        public List<int> CheckLines(int key)
        {
            List<int> lines = new List<int>();
            foreach (var item in busses)
            {
                if (item.)
            }

            return lines;
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
