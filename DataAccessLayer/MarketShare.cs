using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class MarketShare
    {
        private int _numOfTrades { get; }
        private DateTime _startDate { get; }
        private DateTime _endDate { get; }
        private Commodity[] _tradedComms { get; }

        public MarketShare(Commodity[] comms, int n)
        {
            _numOfTrades = n;
            //_tradedComms = DatabaseSocket.getMarketShare(comms, n);
        }
    }
}
