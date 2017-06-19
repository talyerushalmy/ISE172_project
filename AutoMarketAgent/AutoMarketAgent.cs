using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Program
{
    public class AutoMarketAgent
    {
        private MarketClient _marketClient;
        private MarketUserData _userData;
        private Commodity[] _commodities;

        public AutoMarketAgent()
        {
            this._marketClient = new MarketClient();
            updateCommodities();
            //autoPilot();
        }

        public void autoPilot()
        {
            //Functions to test
            n00bTrade();
            raiseCommAvg();
            sell();
        }



        private void updateCommodities()
        {
            //wait();
            this._commodities = this._marketClient.SendQueryAllMarketRequest();
            updateUserData();
        }

        private void updateUserData()
        {
            //wait();
            this._userData = (MarketUserData)this._marketClient.SendQueryUserRequest();
            updateAmount();
        }

        private void updateAmount()
        {
            foreach (var comm in this._userData.commodities)
            {
                int id = Convert.ToInt32(comm.Key);
                this._commodities[id].amount = comm.Value;
            }
        }

        private void buy()
        {
            updateCommodities();
            int avg = getAvgCommAmount();
            Commodity[] askLowerThanBid = getAskLowerThanBid();
            foreach (Commodity comm in askLowerThanBid)
            {
                if (comm.amount < avg)
                {
                    int amount = checkAmountToBuy(comm);
                    if (amount > 0)
                    {
                        this._marketClient.SendBuyRequest(comm.info.ask, comm.id, amount);
                    }
                }
            }
            Commodity commodity  = getBidLowerEqualToAsk().First();
            int toBuy = checkAmountToBuy(commodity);
            if (toBuy > 0)
                this._marketClient.SendBuyRequest(commodity.info.ask, commodity.id, checkAmountToBuy(commodity));
        }

        private void buyAskLowerThanBid()
        {
            updateCommodities();
            Commodity[] askLowerThanBid = _commodities.Where(x => x.getAskToBid() < 1).ToArray();
            //wait(askLowerThanBid.Length);
            foreach(Commodity comm in askLowerThanBid)
            {
                int toBuy = checkAmountToBuy(comm);
                _marketClient.SendBuyRequest(comm.info.ask, comm.id, toBuy);
            }
        }

        private void sellAskLowerThanBid()
        {
            updateCommodities();
            Commodity[] askLowerThanBid = _commodities.Where(x => x.getAskToBid() < 1).ToArray();
            //wait(askLowerThanBid.Length);
            foreach (Commodity comm in askLowerThanBid)
            {
                int toSell = checkAmountToSell(comm);
                _marketClient.SendBuyRequest(comm.info.ask, comm.id, toSell);
            }
        }
        private void buy(double budget)
        {
            updateCommodities();
            int avg = getAvgCommAmount();
            Commodity lowestAsk = getLowestAskComm();
            while (budget > 0 && budget > Math.Abs(lowestAsk.info.ask * (getAvgCommAmount() - lowestAsk.amount)))
            {
                Commodity[] askLowerThanBid = getAskLowerThanBid();
                foreach (Commodity comm in askLowerThanBid)
                {
                    if (budget <= 0 || budget < Math.Abs(lowestAsk.info.ask * (getAvgCommAmount() - lowestAsk.amount)))
                    {
                        break;
                    }
                    int amountToBuy = checkAmountToBuy(comm);
                    if (amountToBuy > 0 && budget >= amountToBuy * comm.info.ask)
                    {
                        //wait();
                        this._marketClient.SendBuyRequest(comm.info.ask, comm.id, amountToBuy);
                        budget -= amountToBuy * comm.info.ask;
                    }
         
                }
                updateCommodities();
                if (budget <= 0 || budget < Math.Abs(lowestAsk.info.ask * (getAvgCommAmount() - lowestAsk.amount)))
                    break;
                Commodity commodity = getBidLowerEqualToAsk().First();
                int amount = checkAmountToBuy(commodity);
                if (budget >= amount * commodity.info.ask)
                {
                    //wait();
                    this._marketClient.SendBuyRequest(commodity.info.ask, commodity.id, amount);
                    budget -= amount * commodity.info.ask;
                }
                updateCommodities();
                avg = getAvgCommAmount();
                lowestAsk = getLowestAskComm();
            }
        }

        private void sell()
        {
            updateCommodities();
            int avgAmount = getAvgCommAmount();
            double avgRatio = getAvgAskToBid();
            int newAvg = (int)((double) avgAmount / (avgRatio > 1 ? avgRatio : 1 / avgRatio));
            while (avgAmount >= newAvg)
            {
                Commodity[] sortedByRatio = getSortedByDescending(x => x.getAskToBid());
                foreach (Commodity comm in sortedByRatio)
                {
                    this._marketClient.SendSellRequest(comm.info.bid, comm.id, checkAmountToSell(comm));
                }
                updateCommodities();
                avgAmount = getAvgCommAmount();
                avgRatio = getAvgAskToBid();
            }
        }

        private void raiseCommAvg()
        {
            updateCommodities();
            double avgRatio = getAvgAskToBid();
            int commAmount = getTotalAmountOfComms();
            double budget = 0;
            if (avgRatio > 1)
            {
                budget = this._userData.funds / (avgRatio);
            }
            else
            {
                budget = this._userData.funds / (commAmount * avgRatio);
            }
            buy(budget);
        }

        private double getAvgAskToBid()
        {
            double sum = _commodities.Sum(x => x.getAskToBid());
            return sum / this._commodities.Length;
        }

        private int checkAmountToBuy(Commodity comm)
        {
            double ratio = comm.getAskToBid();
            int avg = getAvgCommAmount();
            int toBuy = (int)Math.Ceiling((comm.amount == 0 ? 1 : comm.amount) / (ratio));
            while (toBuy > 1)
            {
                if (toBuy * comm.info.ask <= this._userData.funds / (ratio >= 1 ? ratio : 1.0 / ratio))
                    return toBuy;
                else
                    toBuy--;
            }
            return toBuy;
        }


        private int checkAmountToSell(Commodity comm)
        {
            int avg = getAvgCommAmount();
            if (comm.amount == 0)
                return 0;
            int diff = comm.amount - avg;
            if (diff > 0)
            {
                return diff;
            }
            return 1;
        }

        public void n00bTrade()
        {
            updateCommodities();
            Commodity[] askLowerThanBid = getAskLowerThanBid();
            foreach (Commodity comm in askLowerThanBid)
            {
                int toBuy = (int) Math.Min(checkAmountToBuy(comm), _userData.funds / comm.info.ask);
                if (toBuy > 0)
                {
                    int buyID = this._marketClient.SendBuyRequest(comm.info.ask, comm.id, toBuy);
                    int toSell = checkAmountToSell(comm);
                    if (toSell > 0)
                    {
                        int sellID = this._marketClient.SendSellRequest(comm.info.bid, comm.id, toSell);
                    }
                }
            }
        }

        private int getTotalAmountOfComms()
        {
            int sum = (this._commodities.Sum(x => x.amount));
            return sum;
        }

        private int getAvgCommAmount()
        {
            return getTotalAmountOfComms() / this._commodities.Length;
        }

        private bool isRequestSuccessful(int id)
        {
            updateUserData();
            return (!(this._userData.requests.Contains(id)));
        }

        private Commodity getLowestAskComm()
        {
            return _commodities.OrderBy(x => x.info.ask).First();
        }

        private Commodity getHighestAskComm()
        {
            return _commodities.OrderBy(x => x.info.ask).Last();
        }

        private Commodity getLowestBidComm()
        {
            return _commodities.OrderBy(x => x.info.bid).First();
        }

        private Commodity getHighestBidComm()
        {
            return _commodities.OrderBy(x => x.info.bid).Last();
        }

        private Commodity getLowestAskToBidRatioComm()
        {
            return _commodities.OrderBy(x => x.getAskToBid()).First();
        }

        private Commodity getHighestAskToBidRatioComm()
        {
            return _commodities.OrderBy(x => x.getAskToBid()).Last();
        }

        private Commodity[] getSortedBy(Func<Commodity, double> selector)
        {
            return _commodities.OrderBy(selector).ToArray();
        }

        private Commodity[] getSortedByDescending(Func<Commodity, double> selector)
        {
            return _commodities.OrderByDescending(selector).ToArray();
        }

        private Commodity[] getAskLowerThanBid()
        {
            return _commodities.Where(x => x.getAskToBid() < 1).OrderBy(x => x.getAskToBid()).ToArray();
        }
        
        private Commodity[] getBidLowerThanAsk()
        {
            return _commodities.Where(x => x.getAskToBid() > 1).OrderBy(x => x.getAskToBid()).ToArray();
        }

        private Commodity[] getBidLowerEqualToAsk()
        {
            return _commodities.Where(x => x.getAskToBid() >= 1).OrderBy(x => x.getAskToBid()).ToArray();
        }
    }
}
