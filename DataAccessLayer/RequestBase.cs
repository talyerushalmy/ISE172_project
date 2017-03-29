using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class RequestBase
    {
        private string KEY_PATH = @"..\..\..\private_key";
        private string user;
        private string token;
        private string url;

        public void setToken(string KEY_PATH = @"..\..\..\private_key")
        {
            try
            {
                string privateKey = System.IO.File.ReadAllText(KEY_PATH);
                this.token = SimpleCtyptoLibrary.CreateToken(this.user, privateKey);
            }
            catch
            {
                throw new Exception("Error reading the private key file");
            }
        }

        public RequestBase()
        {
            //this.url = "http://localhost";
            this.url = @"http://ise172.ise.bgu.ac.il/";
            this.user = "user46";
            setToken(KEY_PATH);
        }

        public string getUser() { return this.user; }

        public string getToken() { return this.token; }

        public string getUrl() { return this.url; }

    }
}
