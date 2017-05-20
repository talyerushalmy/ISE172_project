using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class QueryUserRequestsRequest
    {
        public string type;

        // a constructor that sets the variable's value
        public QueryUserRequestsRequest()
        {
            this.type = "queryUserRequests";
        }
    }
}
