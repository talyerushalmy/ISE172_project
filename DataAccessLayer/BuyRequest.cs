using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class BuyRequest
    {
        // variables relevant for the request
        public string type;
        public int commodity;
        public int amount;
        public int price;

        // a constructor that sets the variables' values
        public BuyRequest(int commodity, int amount, int price)
        {
            this.type = "buy";
            this.commodity = commodity;
            this.amount = amount;
            this.price = price;
        }
    }
}
