using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class Presentation
    {
        public static void MainLoop()
        {
            AsciiSilverTongue ast = new AsciiSilverTongue();

            ast.PrintWelcome();
            ast.PrintMenu();

            string input = ast.ReadLine();

            while (!input.Equals("exit"))
            {
                Parser.parse(input); // Parse and handle the input using the BusinessLayer
                input = ast.ReadLine();

            }

            // "exit" was recieved as input - print Goodbye and exit program
            ast.PrintGoodbye();
        }
    }
}
