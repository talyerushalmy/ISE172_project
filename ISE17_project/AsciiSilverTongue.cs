using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class AsciiSilverTongue
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
            Console.WriteLine("menu - print this menu");
            Console.WriteLine("hello [NAME] - say Hello to {name}!");
            Console.WriteLine("exit - Exit the program");
        }

        public string ReadLine()
        {
            Console.Write("\n> ");
            return Console.ReadLine();
        }

    }
}
