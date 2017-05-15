using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class QueryAllMarketRequest
    {
        public string type;

        public QueryAllMarketRequest()
        {
            this.type = "queryAllMarket";
        }

        /*public override string ToString()
        {
            string s = "";
            s += " ask: " + info.ask + ", bid: " + info.bid + " id: " + id;
            return s;
        }*/
    }
}
