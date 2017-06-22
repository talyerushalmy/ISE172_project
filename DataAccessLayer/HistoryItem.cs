using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public enum Status { pending, completed, cancelled };

    public class HistoryItem
    {
        public DateTime _date { get; }
        public string _requestType { get; }
        public Object _info { get; }
        public Status _status { get; set; }
        public int _id { get; }

        //Default Constructor
        public HistoryItem(DateTime date,string requestType, Object info, int id)
        {
            _id = id;
            _date = date;
            _requestType = requestType;
            _info = info;
            _status = Status.pending;
        }

    }
}
