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
        private bool _autoPilot;
        private MarketClient _marketClient;
        private MarketUserData _userData;
        private Commodity[] _commodities;

        public AutoMarketAgent(bool autoPilot)
        {
            this._autoPilot = autoPilot;
            this._marketClient = new MarketClient();
            updateUserData();
            updateCommodities();
        }

        public void autoPilot()
        {
            // Functions to test
            n00bTrade();
            raiseCommAvg();
            sell();
        }

        private void updateCommodities()
        {
            this._commodities = this._marketClient.sendQueryAllMarketRequest();
            wait();
            updateAmount();
        }

        private void updateUserData()
        {
            this._userData = (MarketUserData)this._marketClient.SendQueryUserRequest();
        }

        private void updateAmount()
        {
            wait();
            updateUserData();
            foreach (var comm in this._userData.commodities)
            {
                int id = Convert.ToInt32(comm.Key);
                this._commodities[id].amount = comm.Value;
            }
        }

        public void buy()
        {
            updateCommodities();
            updateAmount();
            double bestRatio = Double.MaxValue;
            int avg = getAvgCommAmount();
            bool bought = false;
            int id = 0;
            foreach (Commodity comm in this._commodities)
            {
                double askToBid = comm.getAskToBid();
                if (askToBid < 1 && comm.amount < avg)
                {
                    int amount = checkAmountToBuy(comm);
                    if (amount > 0)
                    {
                        this._marketClient.SendBuyRequest(comm.info.ask, comm.id, amount);
                        bought = true;
                    }
                }
                else if (askToBid < bestRatio)
                {
                    bestRatio = askToBid;
                    id = comm.id;
                }
            }
            Commodity commodity = this._commodities[id];
            if (!bought && commodity.amount < avg)
                this._marketClient.SendBuyRequest(commodity.info.ask, commodity.id, checkAmountToBuy(commodity));
        }

        public void buy(double budget)
        {
            while (budget > 0)
            {
                wait();
                updateCommodities();
                double bestRatio = Double.MaxValue;
                int avg = getAvgCommAmount();
                int id = 0;
                Commodity lowestAsk = getLowestAskComm();
                foreach (Commodity comm in this._commodities)
                {
                    if (budget <= 0 || budget < Math.Abs(lowestAsk.info.ask * (getAvgCommAmount() - lowestAsk.amount)))
                    {
                        break;
                    }
                    double askToBid = comm.getAskToBid();
                    if (askToBid < 1)
                    {
                        int amountToBuy = checkAmountToBuy(comm);
                        if (amountToBuy > 0 && budget >= amountToBuy * comm.info.ask)
                        {
                            wait();
                            this._marketClient.SendBuyRequest(comm.info.ask, comm.id, amountToBuy);
                            budget -= amountToBuy * comm.info.ask;
                        }
                    }
                    else if (askToBid < bestRatio)
                    {
                        bestRatio = askToBid;
                        id = comm.id;
                    }
                }
                if (budget <= 0 || budget < Math.Abs(lowestAsk.info.ask * (getAvgCommAmount() - lowestAsk.amount)))
                    break;
                Commodity commodity = this._commodities[id];
                int amount = checkAmountToBuy(commodity);
                if (amount > 0 && budget >= amount * commodity.info.ask)
                {
                    wait();
                    this._marketClient.SendBuyRequest(commodity.info.ask, commodity.id, amount);
                    budget -= amount * commodity.info.ask;
                }
            }
        }

        public void sell()
        {
            wait();
            updateCommodities();
            int avgAmount = getAvgCommAmount();
            double avgRatio = getAvgAskToBid();
            int newAvg = (int)((double) avgAmount / (avgRatio > 1 ? avgRatio : 1 / avgRatio));
            while (avgAmount >= newAvg)
            {
                foreach (Commodity comm in _commodities)
                {
                    if (comm.amount >= avgAmount)
                    {
                        wait(4);
                        if (comm.getAskToBid() < 1)
                        { // test order
                            int amount = checkAmountToSell(comm);
                            if (amount > 0)
                                this._marketClient.SendSellRequest(comm.info.bid, comm.id, amount);
                            updateCommodities();
                            amount = checkAmountToBuy(comm);
                            if (amount > 0)
                                this._marketClient.SendBuyRequest(comm.info.ask, comm.id, amount);
                        }
                        else
                        {
                            int amount = checkAmountToSell(comm);
                            if (amount > 0)
                                this._marketClient.SendSellRequest(comm.info.bid, comm.id, amount);
                        }
                    }
                }
                wait();
                updateCommodities();
                avgAmount = getAvgCommAmount();
                avgRatio = getAvgAskToBid();
            }
        }

        public void sell2()
        {
            wait();
            updateCommodities();
            double bestRatio = Double.MaxValue;
            bool sold = false;
            int avg = getAvgCommAmount();
            int id = 0;
            foreach (Commodity comm in this._commodities)
            {
                double askToBid = comm.getAskToBid();
                if (askToBid < 1 && comm.amount > avg)
                {
                    int amount = checkAmountToSell(comm);
                    if (amount > 0)
                    {
                        this._marketClient.SendSellRequest(comm.info.bid, comm.id, amount);
                        sold = true;
                        break;
                    }
                }
                else if (askToBid < bestRatio)
                {
                    bestRatio = askToBid;
                    id = comm.id;
                }
            }
            Commodity commodity = this._commodities[id];
            if (!sold && commodity.amount > avg)
                this._marketClient.SendSellRequest(commodity.info.ask, commodity.id, checkAmountToSell(commodity));
        }

        public void raiseCommAvg()
        {
            wait();
            updateCommodities();
            double avgRatio = getAvgAskToBid();
            Console.WriteLine(avgRatio);
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
            Console.WriteLine(budget);
            buy(budget);
        }

        public double getAvgAskToBid()
        {
            double sum = 0;
            foreach (Commodity comm in this._commodities)
            {
                sum += comm.getAskToBid();
            }
            return sum / this._commodities.Length;
        }
        public void makeBestBuyDeal()
        {
            updateCommodities();
            //Console.WriteLine(string.Join(",", this._commodities));
            int index = getIdealCommToBuy();
            Commodity comm = this._commodities[index];
            Console.WriteLine("indexOfCommToBuy:" + index);
            int buyAmount = checkAmountToBuy(comm);
            Console.WriteLine("we need to buy " + buyAmount + " of comm num " + index);
            MarketCommodityOffer offer = (MarketCommodityOffer)this._marketClient.SendQueryMarketRequest(comm.id);
            if (offer.ask <= comm.info.ask)
            {
                comm.info.ask = offer.ask;
                int buyID = this._marketClient.SendBuyRequest(offer.ask, comm.id, buyAmount);
                if (!isRequestSuccessful(buyID))
                {
                    this._marketClient.SendCancelBuySellRequest(buyID);
                }
                Console.WriteLine(buyID);
            }
        }

        private int getIdealCommToBuy()
        {
            int minAmount = Int32.MaxValue;
            int index = 0;
            double bestRatio = 1;
            double bestSpecialRatio = Math.Ceiling((minAmount / bestRatio));
            foreach (Commodity comm in this._commodities)
            {
                double askToBidRatio = comm.getAskToBid();
                if (askToBidRatio <= 1 && askToBidRatio < minAmount * bestRatio)
                {
                    double specialRatio = comm.getAmountToAskToBid();
                    Console.WriteLine("special ratio :" + specialRatio);
                    if (comm.amount <= minAmount && specialRatio <= bestSpecialRatio)
                    {
                        bestRatio = askToBidRatio;
                        minAmount = comm.amount;
                        bestSpecialRatio = specialRatio;
                        index = comm.id;
                    }
                }
                Console.WriteLine();
            }
            return index;
        }

        public void makeBestSellDeal()
        {
            updateCommodities();
            int index = getIdealCommToSell();
            Commodity comm = this._commodities[index];
            //int sellAmount = checkAmountToSell(comm);
            MarketCommodityOffer offer = (MarketCommodityOffer)this._marketClient.SendQueryMarketRequest(comm.id);
            if (offer.bid >= comm.info.bid)
            {
                comm.info.bid = offer.bid;
                //int sellID = this._marketClient.SendSellRequest(comm.info.bid, comm.id, sellAmount);
            }

        }

        private int checkAmountToBuy(int gain, int ask, int amount)
        {
            Console.WriteLine("checking amount to buy");
            int price = amount * ask;
            if (2 * price <= gain || amount == 0)
                return amount;
            return checkAmountToBuy(gain, ask, amount - 1);
        }

        private int checkAmountToBuy(Commodity comm)
        {
            double ratio = comm.getAskToBid();
            int toBuy = (int)Math.Ceiling((comm.amount == 0 ? 1 : comm.amount) / (ratio));
            while (toBuy > 2)
            {
                if (toBuy * comm.info.ask <= this._userData.funds / (ratio >= 1 ? ratio : 1.0 / ratio))
                    return toBuy;
                else
                    toBuy--;
            }
            return toBuy;
        }

        private int getIdealCommToSell()
        {
            int maxAmount = Int32.MinValue;
            int index = this._commodities.Length - 1;
            double bestRatio = Double.PositiveInfinity;
            double bestSpecialRatio = Math.Ceiling((maxAmount / bestRatio));
            foreach (Commodity comm in this._commodities)
            {
                double askToBidRatio = comm.getAskToBid();
                if (askToBidRatio <= 1 && askToBidRatio > maxAmount * bestRatio)
                {
                    double specialRatio = comm.getAmountToAskToBid();
                    if (comm.amount >= maxAmount && specialRatio >= bestSpecialRatio)
                    {
                        bestRatio = askToBidRatio;
                        maxAmount = comm.amount;
                        bestSpecialRatio = specialRatio;
                        index = comm.id;
                    }
                }
            }
            return index;
        }

        private int checkAmountToSell(Commodity comm)
        {
            int avg = getAvgCommAmount();
            int diff = comm.amount - avg;
            if (diff > 0)
            {
                return diff;
            }
            return 0;
        }

        public void n00bTrade()
        {
            int successCount = 0;
            waitAll();
            foreach (Commodity comm in this._commodities)
            {
                wait(1);
                updateCommodities();
                if (comm.getAskToBid() < 1)
                {
                    int toBuy = (int) Math.Min(checkAmountToBuy(comm), _userData.funds / comm.info.ask);
                    if (toBuy > 0)
                    {
                        int buyID = this._marketClient.SendBuyRequest(comm.info.ask, comm.id, toBuy);
                        int toSell = checkAmountToSell(comm);
                        if (toSell > 0)
                        {
                            int sellID = this._marketClient.SendSellRequest(comm.info.bid, comm.id, toSell);
                            if (isRequestSuccessful(sellID))
                                successCount++;
                        }
                        if (isRequestSuccessful(buyID))
                            successCount++;
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

        private void wait()
        {
            //Console.WriteLine("Tom");
            if (RequestTimer.availableRequests() <= 0)
            {
                Console.WriteLine("wait time :" + RequestTimer.getWaitTime());
                System.Threading.Thread.Sleep(RequestTimer.getWaitTime() * 1000);
            }
        }

        private void wait(int n)
        {
            //Console.WriteLine("Groiser");
            if (RequestTimer.availableRequests() <= 0)
            {
                Console.WriteLine("wait time :" + RequestTimer.getWaitTime());
                System.Threading.Thread.Sleep(RequestTimer.getWaitTime(n) * 1000);
            }
        }

        private void waitAll()
        {
            wait(RequestTimer.getLastRequests().Length);
        }

        public Commodity getLowestAskComm()
        {
            return _commodities.OrderBy(x => x.info.ask).First();
        }

        public Commodity getHighestAskComm()
        {
            return _commodities.OrderBy(x => x.info.ask).Last();
        }

        public Commodity getLowestBidComm()
        {
            return _commodities.OrderBy(x => x.info.bid).First();
        }

        public Commodity getHighestBidComm()
        {
            return _commodities.OrderBy(x => x.info.bid).Last();
        }
        private void test()
        {

        }
    }
}
