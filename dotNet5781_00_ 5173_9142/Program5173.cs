using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00__5173_9142
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome5173();
            Welcome9142();
        }
        
        private static void Welcome5173()
        {
            Console.Write("Enter your name: ");
            string a = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", a);
        }

        static partial void Welcome9142();
    }
}
