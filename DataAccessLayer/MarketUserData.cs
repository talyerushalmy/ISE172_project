using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class MarketUserData : IMarketUserData
    {
        // variables relevant for holding the server's resonse
        public Dictionary<string, int> commodities;
        public double funds;
        public int[] requests;

        // override the ToString() method to print the data in an elegant way
        public override string ToString()
        {
            string toPrint = "Commodities:\t";
            foreach (var key in commodities.Keys)
            {
                toPrint += String.Format("{0} - {1}\n\t\t", key, commodities[key]);
            }
            toPrint += String.Format("\nFunds:\t\t{0}\n", this.funds);
            toPrint += String.Format("\nRequests:\t{0}\n", string.Join(", ", requests));

            toPrint += "\n";
            return toPrint;
        }

    }
}
