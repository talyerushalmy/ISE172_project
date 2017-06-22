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
        //The main AMA loop
        public void autoPilot()
        {
            while (true)
            {
                trade();
                Console.WriteLine("Waiting 7 seconds for requests");
                System.Threading.Thread.Sleep(7000);
            }
            //Functions from milestone 2
            //n00bTrade();
            //raiseCommAvg();
            //sell();
        }


        #region Update Functions
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
        #endregion

        #region Trade Functions Using The DB
        //The main trade function which goes over the Market Share and trades each commodity
        private void trade()
        {
            int n = DatabaseSocket.GetHistoryOfLastHour(false).Rows.Count;
            int[,] marketShare = DatabaseSocket.getMarketShare(n);
            Console.WriteLine("Market share of the last " + n + " trades : ");
            printMatrix(marketShare);
            for (int i = marketShare.GetLength(0) - 1; i >= 0; i--)
            {
                TradeComm(this._commodities[marketShare[i, 0]], marketShare[i, 1]);
            }
        }

        // Trades the given commodity based on the last n trades
        private void TradeComm(Commodity comm, int n)
        {
            updateCommodities();
            double avgPrice = Statistics.CalcAvgCommPriceByLastNTrades(comm.id, n);
            Console.WriteLine("Avg price : " + avgPrice + " for commodity " + comm.id);
            if (avgPrice > 0)
            {
                Console.WriteLine("ask price : " + comm.info.ask);
                Console.WriteLine("bid price : " + comm.info.bid);
                if (avgPrice > comm.info.ask)
                {
                    int buyAmount = checkAmountToBuy(comm);
                    if (buyAmount > 0)
                    {
                        this._marketClient.SendBuyRequest(comm.info.ask, comm.id, buyAmount);
                        Console.WriteLine("Bought " + buyAmount + " from " + comm.id + " for the price of " + comm.info.ask + " each");
                    }
                }
                if (avgPrice < comm.info.bid || comm.getAskToBid() <= 1)
                {
                    int sellAmount = checkAmountToSell(comm);
                    if (sellAmount > 0)
                    {
                        this._marketClient.SendSellRequest(calcSellPrice(comm, sellAmount), comm.id, sellAmount);
                        Console.WriteLine("Sold " + sellAmount + " from " + comm.id + " for the price of " + comm.info.bid + " each");
                    }
                }
            }
        }

        // Trades the most traded commodity
        private void TradeMostTradedComm(int[,] marketShare)
        {
            int commID = Statistics.GetMostTradedComm(marketShare);
            TradeComm(this._commodities[commID], marketShare[marketShare.GetLength(0) - 1, 1]);
        }
        #endregion

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
            Commodity commodity = getBidLowerEqualToAsk().First();
            int toBuy = checkAmountToBuy(commodity);
            if (toBuy > 0)
                this._marketClient.SendBuyRequest(commodity.info.ask, commodity.id, checkAmountToBuy(commodity));
        }

        private void buyAskLowerThanBid()
        {
            updateCommodities();
            Commodity[] askLowerThanBid = _commodities.Where(x => x.getAskToBid() < 1).ToArray();
            //wait(askLowerThanBid.Length);
            foreach (Commodity comm in askLowerThanBid)
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
            int newAvg = (int)((double)avgAmount / (avgRatio > 1 ? avgRatio : 1 / avgRatio));
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

        //TODO tweak checkAmountToBuy
        private int checkAmountToBuy(Commodity comm)
        {
            double ratio = comm.getAskToBid();
            int avg = getAvgCommAmount();
            int diff = avg - comm.amount;
            int toBuy = (int)Math.Ceiling((diff > 0 ? diff : (comm.amount == 0 ? 1 : comm.amount) * ratio));
            while (toBuy > 1)
            {
                if (toBuy * comm.info.ask < this._userData.funds / (getTotalAmountOfComms()))
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
            int toSell = 1;
            if (comm.getAskToBid() < 1 && comm.amount > (int)Math.Ceiling((comm.amount * comm.getAskToBid())))
                toSell = (int)Math.Ceiling((comm.amount* comm.getAskToBid()));
            if (diff > 0)
            {
                toSell = diff;
                if (comm.amount > (int)(Math.Max((diff / comm.getAskToBid()), diff)))
                    toSell = (int)(Math.Max((diff / comm.getAskToBid()), diff));
            }
            else if (comm.amount > (int)Math.Ceiling((avg * comm.getAskToBid())))
            {
                toSell = (int)Math.Ceiling((avg * comm.getAskToBid()));
            }
            return toSell;
        }

        private int calcSellPrice(Commodity comm, int amountToSell)
        {
            int sellPrice = comm.info.bid;
            if (comm.getAskToBid() < 1)
            {
                sellPrice = (int)((comm.info.ask + comm.info.bid) / 2);
            }
            return sellPrice;
        }

        public void n00bTrade()
        {
            updateCommodities();
            Commodity[] askLowerThanBid = getAskLowerThanBid();
            foreach (Commodity comm in askLowerThanBid)
            {
                int toBuy = (int)Math.Min(checkAmountToBuy(comm), _userData.funds / comm.info.ask);
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

        private void printMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0} ", matrix[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
