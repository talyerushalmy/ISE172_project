﻿using System;
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
            Logger.InfoLog("The user started to work");
            AsciiSilverTongue ast = new AsciiSilverTongue();
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
            Logger.InfoLog("The user finished to work in the system");
            ast.PrintGoodbye();
        }
    }
}
