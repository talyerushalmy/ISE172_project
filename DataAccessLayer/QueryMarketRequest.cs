using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class QueryMarketRequest
    {
        public string type;
        public int commodity;

        public QueryMarketRequest(int commodity)
        {
            this.type = "queryMarket";
            this.commodity = commodity;
        }

    }
}
