using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class Program
    {

        static AsciiSilverTongue ast = new AsciiSilverTongue();

        static void Main(string[] args)
        {
            ast.PrintWelcome();
            ast.PrintMenu();

            string input = ast.ReadLine();

            while (!input.Equals("exit"))
            {
                Parser.parse(input);
                input = ast.ReadLine();
            }

            // "exit" was recieved as input - print Goodbye and exit program
            ast.PrintGoodbye();
        }
    }
}
