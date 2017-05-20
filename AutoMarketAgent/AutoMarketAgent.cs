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
        private System.Timers.Timer _timer;

        public AutoMarketAgent(bool autoPilot)
        {
            this._autoPilot = autoPilot;
            this._marketClient = new MarketClient();
            this._timer = new Timer(10000);
            updateUserData();
            this._commodities = initializeCommodities();
        }

        private Commodity[] initializeCommodities()
        {
            int length = this._userData.commodities.Keys.Count;
            Commodity[] commodities = new Commodity[length];
            return commodities;
        }

        private void updateUserData()
        {
            this._userData = (MarketUserData)this._marketClient.SendQueryUserRequest();
        }

        private void updateCommodities()
        {
            updateUserData();
            foreach (var commodity in this._userData.commodities)
            {
                int commID = Convert.ToInt32(commodity.Key);
            }
        }

        public void autoPilot()
        {
            int numOfItems = _userData.commodities.Values.Sum();
            while (_autoPilot)
            {
                #region Terminology
                // Key - commodity type
                // Value - commodity amount
                #endregion

                test();

                /*int commID = -1;
                int commAmount = -1;
                double askToBidRatio = Double.PositiveInfinity;
                MarketCommodityOffer bestOffer = null;
                foreach (var commodity in this._userData.commodities)
                {
                    MarketCommodityOffer offer = (MarketCommodityOffer)this._marketClient.SendQueryMarketRequest(Convert.ToInt32(commodity.Key));
                    double bestRatio = ((double)offer.ask / (double)offer.bid);
                    if (bestRatio < askToBidRatio && bestRatio != 1)
                    {
                        bestOffer = offer;
                        askToBidRatio = bestRatio;
                        commID = Convert.ToInt32(commodity.Key);
                        commAmount = commodity.Value;
                    }

                }
                int fixedAskToBitRatio = (int)(Math.Ceiling(askToBidRatio));
                int amountToAskToBidRatio = commAmount / fixedAskToBitRatio;
                if (askToBidRatio > 1)
                {
                    Console.WriteLine("ask is bigger");
                    if (amountToAskToBidRatio > 1)
                    {
                        int sellID = this._marketClient.SendSellRequest(bestOffer.bid, commID, fixedAskToBitRatio);
                        this._userData = (MarketUserData)this._marketClient.SendQueryUserRequest();
                        if (!_userData.requests.Contains(sellID))
                        {
                            int buyID = this._marketClient.SendBuyRequest(bestOffer.ask, commID, amountToAskToBidRatio);
                        }
                        else
                        {
                            this._marketClient.SendCancelBuySellRequest(sellID);
                        }
                    }
                }
                else if (askToBidRatio < 1)
                {
                    Console.WriteLine("Tom");
                    bool success = false;
                    int count = 1;
                    int maxAmountToBuy = checkAmountToBuy((int)this._userData.funds, bestOffer.ask, commAmount);
                    Console.WriteLine("maxAmountToBuy: " + maxAmountToBuy);
                    while (!success && count <= maxAmountToBuy)
                    {
                        Console.WriteLine("bid is bigger");
                        Console.WriteLine(commID);
                        int buyID = this._marketClient.SendBuyRequest(bestOffer.ask, commID, count);
                        if (isRequestSuccessful(buyID))
                        {
                            int buyCount = count;
                            Console.WriteLine("did buy, buyCount:" + buyCount);
                            count = 1;
                            int sellID = this._marketClient.SendSellRequest(bestOffer.bid, commID, count);
                            while (!isRequestSuccessful(sellID))
                            {
                                Console.WriteLine("Trying to sell");
                                this._marketClient.SendCancelBuySellRequest(sellID);
                                System.Threading.Thread.Sleep(3000);
                                count++;
                                sellID = this._marketClient.SendSellRequest(bestOffer.bid, commID, count);
                            }
                            Console.WriteLine("sold successfully");
                            success = true;
                        }
                        else
                        {
                            this._marketClient.SendCancelBuySellRequest(buyID);
                            count++;
                            System.Threading.Thread.Sleep(3000);
                        }
                    }
                }
                Console.WriteLine("autoPilotOff");*/
                _autoPilot = false;
            }

        }

        private double getAskToBidRatio(Commodity comm)
        {
            double ratio = ((double)comm.info.ask / (double)comm.info.bid);
            if (ratio <= 1)
                return ratio;
            else
                return Math.Ceiling(ratio);
        }

        private double getAmountToAskToBidRatio(Commodity comm)
        {
            double ratio = 1;// (comm.amount) / (getAskToBidRatio(comm));
            if (ratio <= 1)
                return ratio;
            else
                return Math.Ceiling(ratio);
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
                /*if (!isRequestSuccessful(buyID))
                {
                    this._marketClient.SendCancelBuySellRequest(buyID);
                }*/
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
                double askToBidRatio = getAskToBidRatio(comm);
                Console.WriteLine("commID : " + comm.id);
                //Console.WriteLine("ammount : " + comm.amount);
                Console.WriteLine("ask: " + comm.info.ask);
                Console.WriteLine("bid: " + comm.info.bid);
                Console.WriteLine("ratio: " + askToBidRatio);
                if (askToBidRatio <= 1 && askToBidRatio < minAmount * bestRatio)
                {
                    double specialRatio = getAmountToAskToBidRatio(comm);
                    Console.WriteLine("special ratio :" + specialRatio);
                    /*if (comm.amount <= minAmount && specialRatio <= bestSpecialRatio)
                    {
                        bestRatio = askToBidRatio;
                        //minAmount = comm.amount;
                        bestSpecialRatio = specialRatio;
                        index = comm.id;
                    }*/
                }
                Console.WriteLine();
            }
            return index;
        }

        public void makeBestSellDeal()
        {
            updateCommodities();
            //Console.WriteLine(string.Join(",", this._commodities));
            int index = getIdealCommToSell();
            Commodity comm = this._commodities[index];
            Console.WriteLine("indexOfCommToSell:" + index);
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
            int avg = getAvgCommAmount();
            double ratio = getAskToBidRatio(comm);
            /*while (diff > 0)
            {
                if (diff * comm.ask <= this._userData.funds * ratio)
                    return diff;
                else
                    diff--;
            }
            return diff;*/
            return 0;
        }

        private int getIdealCommToSell()
        {
            int maxAmount = Int32.MinValue;
            int index = this._commodities.Length - 1;
            double bestRatio = Double.PositiveInfinity;
            double bestSpecialRatio = Math.Ceiling((maxAmount / bestRatio));
            foreach (Commodity comm in this._commodities)
            {
                double askToBidRatio = getAskToBidRatio(comm);
     
           
                if (askToBidRatio <= 1 && askToBidRatio > maxAmount * bestRatio)
                {
                    double specialRatio = getAmountToAskToBidRatio(comm);
                    /*Console.WriteLine("special ratio :" + specialRatio);
                    if (comm.amount >= maxAmount && specialRatio >= bestSpecialRatio)
                    {
                        bestRatio = askToBidRatio;
                        maxAmount = comm.amount;
                        bestSpecialRatio = specialRatio;
                        index = comm.id;
                    }*/
                }
                Console.WriteLine();
            }
            return index;
        }
        /*
        private int checkAmountToSell(Commodity comm)
        {
            int avg = getAvgCommAmount();
            int diff = comm.amount - avg;
            Console.WriteLine("ammount of comm num " + comm.id + " : " + comm.amount);
            Console.WriteLine("average comms : " + avg);
            Console.WriteLine("diff " + diff);
            int ratio = (int)Math.Ceiling(getAskToBidRatio(comm));
            if (diff > 0)
            {
                return diff;
            }
            return (comm.amount / ratio);
        }
        */
        private int getTotalAmmountOfComms()
        {
            int sum = 0;
            foreach(Commodity comm in this._commodities)
            {
                //sum += comm.amount;
            }
            return sum;
        }

        private int getAvgCommAmount()
        {
            return getTotalAmmountOfComms()/ this._commodities.Length;
        }

        /*private int indexOfMaxComm()
        {
            int max = Int32.MinValue;
            int index = 0;
            foreach (Commodity comm in this._commodities)
            {
                if (comm.amount > max)
                {
                    max = comm.amount;
                    index = comm.id;
                }
            }
            return index;
        }

        private int indexOfMinComm()
        {
            int min = Int32.MaxValue;
            int index = 0;
            foreach (Commodity comm in this._commodities)
            {
                if (comm.amount > min)
                {
                    min = comm.amount;
                    index = comm.id;
                }
            }
            return index;
        }*/

        private bool isRequestSuccessful(int id)
        {
            System.Threading.Thread.Sleep(3000);
            updateUserData();
            return (!(this._userData.requests.Contains(id)));
        }

        private void checkCommodityOffer(Commodity comm)
        {
            MarketCommodityOffer offer = (MarketCommodityOffer)this._marketClient.SendQueryMarketRequest(comm.id);
            int bid = offer.bid, ask = offer.ask;
            int diff = offer.ask - offer.bid;
            double askToBidRatio = (double)ask / (double)bid;
            int fixedRatio = (int)Math.Ceiling(askToBidRatio);
            //int amountToRatio = comm.amount / fixedRatio;
            double currFunds = this._userData.funds;
            System.Threading.Thread.Sleep(2000);
            if (askToBidRatio > 1)
            {
                /*if (amountToRatio > 1)
                {
                    int sellID = this._marketClient.SendSellRequest(bid, comm.id, amountToRatio);
                    if (isRequestSuccessful(sellID))
                    {
                        double gain = this._userData.funds - currFunds;
                        int amountToBuy = checkAmountToBuy(bid * amountToRatio, ask, amountToRatio);
                        if (amountToBuy > 0)
                        {
                            int buyID = this._marketClient.SendBuyRequest(ask, comm.id, amountToBuy);
                            if (!isRequestSuccessful(buyID))
                            {
                                this._marketClient.SendCancelBuySellRequest(buyID);
                            }
                        }
                    }
                    else
                        this._marketClient.SendCancelBuySellRequest(sellID);
                }*/
            }
            else if (askToBidRatio < 1)
            {

            }

        }

        private void test()
        {

        }

        /*while (offer.ask < offer.bid)
    {
    Console.WriteLine(offer);
    if (this._userData.funds >= offer.ask)
    {
        bool buy = true;
        bool sell = true;
        foreach (int request in this._userData.requests)
        {
            MarketItemQuery marketItem = (MarketItemQuery)this._marketClient.SendQueryBuySellRequest(request);
            if (marketItem.type.Equals("sell") && marketItem.commodity == commodityID && marketItem.price == offer.ask)
            {
                sell = false;
                break;
            }
            else if (marketItem.type.Equals("buy") && marketItem.commodity == commodityID && marketItem.price == offer.bid)
            {
                buy = false;
                break;
            }
        }
        if (buy)
        {
            int buyID = this._marketClient.SendBuyRequest(offer.ask, commodityID, 1);
            if (this._userData.requests.Contains(buyID))
            {
                sell = false;
            }
        }
        if (sell)
        {
            int sellID = this._marketClient.SendSellRequest(offer.bid, commodityID, 1);
        }
        Console.WriteLine(this._userData.requests);

    }
    offer = (MarketCommodityOffer)_marketClient.SendQueryMarketRequest(commodityID);
    }*/
    }
}
