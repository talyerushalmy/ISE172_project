using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class MarketCommodityOffer : IMarketCommodityOffer
    {
        // variables relevant for holding the server's resonse
        public int ask;
        public int bid;

        // override the ToString() method to print the data in an elegant way
        public override string ToString()
        {
            string toPrint = "";

            toPrint += "Ask: " + ask + "\n";
            toPrint += "Bid: " + bid + "\n";

            return toPrint;
        }

    }
}
