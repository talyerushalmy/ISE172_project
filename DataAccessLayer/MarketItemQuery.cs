using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class MarketItemQuery : IMarketItemQuery
    {
        // variables relevant for holding the server's resonse
        public int price;
        public int amount;
        public string type;
        public string user;
        public int commodity;

        // override the ToString() method to print the data in an elegant way
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
