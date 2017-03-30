using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class QueryUserRequest
    {
        // variables relevant for the request
        public string type;

        // a constructor that sets the variable's value
        public QueryUserRequest()
        {
            this.type = "queryUser";
        }
    }
}
