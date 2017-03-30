using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class QueryBuySellRequest
    {
        // variables relevant for the request
        public string type;
        public int id;

        // a constructor that sets the variables' values
        public QueryBuySellRequest(int id)
        {
            this.type = "queryBuySell";
            this.id = id;
        }

    }
}
