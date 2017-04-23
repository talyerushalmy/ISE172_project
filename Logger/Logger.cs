using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Program;


namespace Program
{
    public static class Logger 
    {
        public static void logMessage(string input)
        {
            string path = @"../../../Logs.txt";
            System.IO.StreamWriter log = new System.IO.StreamWriter(path, true);
            log.Write("Type: Message ,");
            log.Write(DateTime.Now+" , ");
            log.Write(input);
            log.WriteLine();
            log.Close();  
        }
        public static void logError(string File,string Line)
        {
            string path = @"../../../Logs.txt";
            System.IO.StreamWriter log = new System.IO.StreamWriter(path, true);
            log.Write("Type: Error ,");
            log.Write("Occured in "+DateTime.Now + " ,");
            log.Write(" In File " + File + " In Line " + Line);
            log.WriteLine();
            log.Close();
        }

    }
}

