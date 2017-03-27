using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class SellRequest
    {
        public string type;
        public int commodity;
        public int amount;
        public int price;

        public SellRequest(int commodity, int amount, int price)
        {
            this.type = "sell";
            this.commodity = commodity;
            this.amount = amount;
            this.price = price;
        }
    }
}
