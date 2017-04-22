using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class Presentation
    {
        public static void MainLoop()  //Print the main communication loop with the user.
        {
            AsciiSilverTongue ast = new AsciiSilverTongue();
           // Log lg = new Log();
            //lg.WriteAndRead("Groiser the king");
            //Log- the user enters to the system.
            ast.PrintWelcome();
            ast.PrintMenu();

            string input = ast.ReadLine().ToLower();

            while (!input.Equals("exit"))
            {
                if (input.Equals("menu"))
                    ast.PrintMenu();
                else if (input.Equals("clear"))
                {
                    Console.Clear();
                    ast.PrintWelcome();
                    ast.PrintMenu();
                }
                else
                    Parser.parse(input); // Parse and handle the input using the BusinessLayer
                input = ast.ReadLine();

            }

            // "exit" was recieved as input - print Goodbye and exit program
            //Log- the user exits to the system.
            ast.PrintGoodbye();
        }
    }
}
