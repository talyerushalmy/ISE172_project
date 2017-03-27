using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class MarketCommodityOffer : IMarketCommodityOffer
    {
        public int ask;
        public int bid;

        public override string ToString()
        {
            string toPrint = "";

            toPrint += "Ask: " + ask + "\n";
            toPrint += "Bid: " + bid + "\n";

            return toPrint;
        }

    }
}
