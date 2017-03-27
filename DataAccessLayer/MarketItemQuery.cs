using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class MarketItemQuery : IMarketItemQuery
    {
        public int price;
        public int amount;
        public string type;
        public string user;
        public int commodity;

        public override string ToString()
        {
            string toPrint = "";

            toPrint += "Price: " + this.price + "\n";
            toPrint += "Amount: " + this.amount + "\n";
            toPrint += "Type: " + this.type + "\n";
            toPrint += "User: " + this.user + "\n";
            toPrint += "Commodity: " + this.commodity + "\n";

            return toPrint;
        }

    }
}
