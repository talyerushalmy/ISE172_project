using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class Commodity
    {
        public MarketCommodityOffer info;
        public int id;

        public override string ToString()
        {
            string s = "id: " + id + "\tinfo: " + "[ ask: " + info.ask + ", bid " + info.bid + " ]";
            return s;
        }
    }
}
