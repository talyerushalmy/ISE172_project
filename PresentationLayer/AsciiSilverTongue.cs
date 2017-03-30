using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class AsciiSilverTongue
    {

        public AsciiSilverTongue()
        {
            // a default constructor
        }

        public void PrintWelcome()
        {
            string data = @"    ___    __                        ______               __   
   /   |  / /___ _____              /_  __/________ _____/ /__ 
  / /| | / / __ `/ __ \   ______     / / / ___/ __ `/ __  / _ \
 / ___ |/ / /_/ / /_/ /  /_____/    / / / /  / /_/ / /_/ /  __/
/_/  |_/_/\__, /\____/             /_/ /_/   \__,_/\__,_/\___/ 
         /____/                                                ";

            Console.WriteLine(data);
            Console.WriteLine("\nBy Tom Marzea, Roee Groiser and Tal Yerushalmi\n");
        }

        public void PrintGoodbye()
        {
            string data = @"   ______                ____               __
  / ____/___  ____  ____/ / /_  __  _____  / /
 / / __/ __ \/ __ \/ __  / __ \/ / / / _ \/ / 
/ /_/ / /_/ / /_/ / /_/ / /_/ / /_/ /  __/_/  
\____/\____/\____/\__,_/_.___/\__, /\___(_)   
                             /____/           ";

            Console.WriteLine(data);
            Console.WriteLine("\nThank you for using Algo-Trade\n");
        }

        public void PrintMenu()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("\tmenu - Print this menu");
            Console.WriteLine("\tbuy [COMMODITY ID (number)] [QUANTITY (number)] [PRICE(number)] - Buy [COMMODITY] by the number of [QUANTITY] for the price of [PRICE] each");
            Console.WriteLine("\tsell [COMMODITY ID (number)] [QUANTITY (number)] [PRICE(number)] - Sell [COMMODITY] by the number of [QUANTITY] for the price of [PRICE] each");
            Console.WriteLine("\tcancel [TRADE ID (number)] - Cancel trade identified by [TRADE ID]");
            Console.WriteLine("\tinfo - Query the server for information about the user and show the results");
            Console.WriteLine("\tfind sell/buy/commodity [REQUEST/COMMODITY ID (number)] - Query the server for [REQUEST/COMMODITY ID] and show the results");
            Console.WriteLine("\tclear - Clear the user activity history");
            Console.WriteLine("\texit - Exit the program");
        }

        public static void printServerData(Object obj) //The method Prints information that the server sends.
        {
            Console.WriteLine(obj);
        }

        public string ReadLine()  //The method reads the user's input.
        {
            Console.Write("\n> ");
            return Console.ReadLine();
        }

    }
}
