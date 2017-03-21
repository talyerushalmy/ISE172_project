using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class Presentation
    {
        static AsciiSilverTongue ast = new AsciiSilverTongue();

        private static void HandleInput(string input)
        {
            switch (input)
            {
                case "menu":
                    ast.PrintMenu();
                    break;
                default:
                    if (input.Length > 6 && input.IndexOf("hello ") == 0)
                        Console.WriteLine("Hello " + input.Substring(6));
                    else
                        Console.WriteLine("Wrong format - please check your input");
                    break;
            }

        }

        public static void MainLoop()
        {
            ast.PrintWelcome();
            ast.PrintMenu();

            string input = ast.ReadLine();

            while (!input.Equals("exit"))
            {
                HandleInput(input);
                input = ast.ReadLine();
            }

            // "exit" was recieved as input - print Goodbye and exit program
            ast.PrintGoodbye();
        }
    }
}
