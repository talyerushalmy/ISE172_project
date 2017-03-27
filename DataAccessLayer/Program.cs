using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            MarketClient c = new MarketClient();
            Console.WriteLine(c.SendQueryUserRequest());
            Console.WriteLine("HEY");
        }
    }
}
