using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class SellRequest
    {
        private string type;
        private int commodity;
        private int amount;
        private int price;

        public SellRequest(int commodity, int amount, int price)
        {
            this.type = "sell";
            this.commodity = commodity;
            this.amount = amount;
            this.price = price;
        }
    }
}
