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
                bool isTimeToClean = (DateTime.Now.Hour == 19 && DateTime.Now.Minute == 00);
                System.IO.StreamWriter log = new System.IO.StreamWriter(path, !isTimeToClean);
                log.Write("Type: Message ,");
                log.Write(DateTime.Now + " , ");
                log.Write(input);
                log.WriteLine();
                log.Close();
        }
        
        public static void logError(string File,string Line, string message)
        {
                string path = @"../../../Logs.txt";
                bool isTimeToClean = (DateTime.Now.Hour == 19 && DateTime.Now.Minute == 00);
                System.IO.StreamWriter log = new System.IO.StreamWriter(path, !isTimeToClean);
                log.Write("Type: Error");
                log.Write("Occured in " + DateTime.Now + " ,");
                log.Write(" in File " + File + " in Line " + Line);
                log.WriteLine();
                
                log.WriteLine();
                log.Close();
        }

    }
}

