using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class QueryMarketRequest
    {
        // variables relevant for the request
        public string type;
        public int commodity;

        // a constructor that sets the variables' values
        public QueryMarketRequest(int commodity)
        {
            this.type = "queryMarket";
            this.commodity = commodity;
        }

    }
}
