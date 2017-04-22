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

        public AutoMarketAgent(bool autoPilot)
        {
            this._autoPilot = autoPilot;
            this._marketClient = new MarketClient();
            this._userData = (MarketUserData)this._marketClient.SendQueryUserRequest();
        }

        public void autoPilot()
        {
            int numOfItems = _userData.commodities.Values.Sum();
            while (_autoPilot)
            {
                // Key - commodity type
                // Value - commodity amount
                foreach (var commodity in this._userData.commodities)
                {

                    double currFunds = this._userData.funds;
                    int commodityID = Convert.ToInt32(commodity.Key);
                    int commodityAmount = commodity.Value;
                    Console.WriteLine(commodityID);
                    MarketCommodityOffer offer = (MarketCommodityOffer)_marketClient.SendQueryMarketRequest(commodityID);
                    int diff = offer.ask - offer.bid;
                    double askToBidRatio = (double)offer.ask / (double)offer.bid;
                    if (askToBidRatio > 1)
                    {
                        int fixedAskToBitRation = (int)(Math.Ceiling(askToBidRatio));
                        if (commodityAmount >= Math.Pow(fixedAskToBitRation, 2))
                        {
                            int sellID = this._marketClient.SendSellRequest(offer.bid, commodityID, commodityAmount/fixedAskToBitRation);
                            this._userData = (MarketUserData)this._marketClient.SendQueryUserRequest();
                            if (this._userData.funds>currFunds && !_userData.requests.Contains(sellID))
                            {
                                int buyID = this._marketClient.SendBuyRequest(offer.ask, commodityID, commodityAmount / fixedAskToBitRation / offer.ask);
                            }
                            else
                            {
                                this._marketClient.SendCancelBuySellRequest(sellID);
                            }
                        }
                        if (commodityAmount > fixedAskToBitRation)
                        {
                            int sellID = this._marketClient.SendSellRequest(offer.bid, commodityID, fixedAskToBitRation);
                            this._userData = (MarketUserData)this._marketClient.SendQueryUserRequest();
                            if (this._userData.funds > currFunds && !_userData.requests.Contains(sellID))
                            {
                                int buyID = this._marketClient.SendBuyRequest(offer.ask, commodityID, commodityAmount /offer.ask);
                            }
                            else
                            {
                                this._marketClient.SendCancelBuySellRequest(sellID);
                            }
                        }
                        //int buyID = this._marketClient.SendBuyRequest(offer.ask, commodityID, 1);
                    }
                    else
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
                _autoPilot = false;
            }
        }

        private void noOneWantsToBuyMe()
        {

        }

        private void wayMuchCheap()
        {

        }
    }
}
