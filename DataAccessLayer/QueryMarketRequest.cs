using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class QueryMarketRequest
    {
        private string type;
        private int commodity;

        public QueryMarketRequest(int commodity)
        {
            this.type = "queryMarket";
            this.commodity = commodity;
        }

    }
}
