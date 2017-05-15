using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class UserRequest
    {
        public MarketItemQuery request;
        public int id;

        public override string ToString()
        {
            string s = "";
            s += "request:" + "[Price: " + request.price + ", Amount: " + request.amount + ", Type: " + request.type + ", User: " + request.user + ", Commodity: " + request.commodity + " ], id: " + id;
            return s;
        }
    }
}
