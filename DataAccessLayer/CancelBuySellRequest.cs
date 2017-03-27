using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class CancelBuySellRequest
    {
        public string type;
        public int id;

        public CancelBuySellRequest(int id)
        {
            this.type = "cancelBuySell";
            this.id = id;
        }

    }
}
