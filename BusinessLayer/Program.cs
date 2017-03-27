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
            string input = Console.ReadLine();
            while (!input.ToLower().Equals("quit"))
            {
                Parser.parser(input);
                input = Console.ReadLine();
            }
        }

    }
}
