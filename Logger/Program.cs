using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            //Just For Experiments!!!!!!
            //Logger logger = new Logger();
            //String input = Console.ReadLine();
            //logger.logMessage(input);
            StackFrame st = new StackFrame(0, true);
            Console.WriteLine("we are at"+st.GetFileLineNumber() + "  " + st.GetFileName());
            Console.ReadLine();

        }
    }
}
