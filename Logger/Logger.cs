using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Program;


namespace Program
{
    public class Logger 
    {
        private string _path = @"../../../Logs.txt";
        private System.IO.StreamWriter _log;

        public Logger() {
            this._log = new System.IO.StreamWriter(_path, true);
        }
        public void logMessage(string input)
        {
            this._log.Write("Type: Message ,");
            this._log.Write(DateTime.Now+" , ");
            this._log.Write(input);
            this._log.WriteLine();
            this._log.Close();
            this._log= new System.IO.StreamWriter(_path, true); 
        }
        public void logError(string File,string Line)
        {
            
            this._log.Write("Type: Error ,");
            this._log.Write("Occured in "+DateTime.Now + " ,");
            this._log.Write(" In File " + File + " In Line " + Line);
            this._log.WriteLine();
        }

    }
}

