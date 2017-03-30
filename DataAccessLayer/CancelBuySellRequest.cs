using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class CancelBuySellRequest
    {
        // variables relevant for the request
        public string type;
        public int id;

        // a constructor that sets the variables' values
        public CancelBuySellRequest(int id)
        {
            this.type = "cancelBuySell";
            this.id = id;
        }

    }
}
