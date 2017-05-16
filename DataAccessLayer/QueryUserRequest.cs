using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class QueryUserRequest
    {
        // variables relevant for the request
        public string type;
        public MarketItemQuery request;
        public int id;

        public override string ToString()
        {
            string s = "";
            s += "id: " + id + "\tRequest: " + "[ Price: " + request.price + " , Amount: " + request.amount + " , Type: " + request.type + " , User: " + request.user + " , Commodity: " + request.commodity + " ]";
            return s;
        }

        // a constructor that sets the variable's value
        public QueryUserRequest()
        {
            this.type = "queryUser";
        }
    }
}
