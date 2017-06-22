using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    // A Class which represents a row in the history table of the DB
    public class Transaction
    {
        //Object that represents a transaction and contains its details.
        private readonly DateTime _timestamp;
        private readonly int _price;
        private readonly int _amount;
        private readonly int _commodityID;

        public Transaction(DateTime timestamp,int commodityID,int price,int amount)
        {
            _timestamp = timestamp;
            _commodityID = commodityID;
            _price = price;
            _amount = amount;
        }
        public Transaction(DateTime timestamp,int price)
        {
            _timestamp = timestamp;
            _price = price;
        }
        public Transaction(int commodityID,int amount,int price)
        {
            _commodityID = commodityID;
            _price = price;
            _amount = amount;
        }

        public DateTime getTimestamp()
        {
            return _timestamp;
        }
        public int getPrice()
        {
            return _price;
        }
        public int getAmount()
        {
            return _amount;
        }
        public int getCommodityID()
        {
            return _commodityID;
        }

        public override string ToString()
        {
            return this._timestamp.ToString() + ", Commodity: " + this._commodityID + ", Amount: " + this._amount + ", Price: " + this._price;
        }
    }
}
