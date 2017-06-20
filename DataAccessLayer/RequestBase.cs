using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    // holds the data that is necessary for every request
    class RequestBase
    {
        // variables relevant for every request
        private string KEY_PATH = @"..\..\..\private_key";
        private string user;
        private string url;
        private string privateKey;

        public string createToken(int nonce)
        {
            try
            {
                return SimpleCtyptoLibrary.CreateToken(user, privateKey, nonce);
            }
            catch
            {
                StackFrame sf = new StackFrame(1, true);
                Logger.ErrorLog(sf.GetMethod(), sf.GetFileLineNumber(), "There's a problem with reading the private key");
                throw new Exception("Error reading the private key file");
            }
        }

        // a constructor that sets the variables' values
        public RequestBase()
        {
            //this.url = "http://localhost";
            this.url = @"http://ise172.ise.bgu.ac.il/";
            this.user = "user46";
            this.privateKey = System.IO.File.ReadAllText(KEY_PATH);
        }

        public string getUser() { return this.user; }

        public string getUrl() { return this.url; }
    }
}
