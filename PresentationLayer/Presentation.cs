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
                Console.WriteLine(input);
                // delete the line above and use the Business Layer project to handle the input
                input = ast.ReadLine();
            }

            // "exit" was recieved as input - print Goodbye and exit program
            ast.PrintGoodbye();
        }
    }
}
