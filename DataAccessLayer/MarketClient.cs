﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{

    // an implementation of the IMarketClient interface
    public class MarketClient : IMarketClient
    {
        // a dictionary (or an array, to be precise) of errors not to print on occurrence
        static string[] errorsNotToPrint =
        {
            "No auth key",
            "No user or auth token",
            "Verification failure",
            "No type key"
        };

        private SimpleHTTPClient client; // will be used to communicate with the server
        private RequestBase req = new RequestBase(); // will be used to get the necessary data for every request

        // a constructor that resets the local variables
        public MarketClient()
        {

            this.client = new SimpleHTTPClient();
            this.req = new RequestBase();
        }

        private void printError(string error)
        // Prints the error string only if it's relevant to the user
        // The error variable contains a response from the server that was marked as error
        {
            if (!MarketClient.errorsNotToPrint.Contains(error))
                Console.WriteLine(error);
        }

        // a method to make the function call shorter (and thus more convenient for the programmer)
        private string SendRequest<T>(T data)
        {
            try
            {
                Random rand = new Random();
                int nonce = rand.Next(Int32.MinValue, Int32.MaxValue);
                string token = req.createToken(nonce);

                var resp = this.client.SendPostRequest<T>(this.req.getUrl(), this.req.getUser(), token, data, nonce);
                RequestTimer.wait();
                RequestTimer.addRequest();
                return resp;
            }
            catch
            {
                return null;
            }
        }

        // a method to make the function call shorter (and thus more convenient for the programmer)
        private T2 SendRequest<T1, T2>(T1 data) where T2 : class
        {
            try
            {
                Random rand = new Random();
                int nonce = rand.Next(Int32.MinValue, Int32.MaxValue);
                string token = req.createToken(nonce);
                RequestTimer.wait();
                var resp = this.client.SendPostRequest<T1, T2>(this.req.getUrl(), this.req.getUser(), token, data, nonce);
                RequestTimer.addRequest();
                return resp;
            }
            catch (Exception e)
            {
                // in case of an error, print the error message
                Console.WriteLine(e.Message);
                return null;
            }
        }

        // send a buy request using the MarketClient project API
        public int SendBuyRequest(int price, int commodity, int amount)
        {
            string response = "Unknown error";
            try
            {
                BuyRequest data = new BuyRequest(commodity, amount, price);
                response = SendRequest(data);
                int id = Convert.ToInt32(response);
                if (id >= 1)
                {
                    HistoryItem newItem = new HistoryItem(DateTime.Now, data.type, data.ToString(), id);
                    HistoryTable.Add(newItem);
                }
                return id;
            }
            catch
            {
                printError(response); // Print the error
                return -1;
            }
        }

        // send a sell request using the MarketClient project API
        public int SendSellRequest(int price, int commodity, int amount)
        {
            string response = "Unknown error";
            try
            {
                SellRequest data = new SellRequest(commodity, amount, price);
                response = SendRequest(data);
                int id = Convert.ToInt32(response);
                if (id >= 1)
                {
                    HistoryItem newItem = new HistoryItem(DateTime.Now, data.type, data.ToString(), id);
                    HistoryTable.Add(newItem);
                }
                return id;
            }
            catch
            {
                printError(response); // Print the error
                return -1;
            }
        }

        // send a query buy/sell request using the MarketClient project API
        public IMarketItemQuery SendQueryBuySellRequest(int id)
        {
            object obj = SendRequest<QueryBuySellRequest, MarketItemQuery>(new QueryBuySellRequest(id));
            return (MarketItemQuery)obj;
        }

        // send a query user request using the MarketClient project API
        public IMarketUserData SendQueryUserRequest()
        {
            object obj = SendRequest<QueryUserRequest, MarketUserData>(new QueryUserRequest());
            return (MarketUserData)obj;
        }

        // send a query market request using the MarketClient project API
        public IMarketCommodityOffer SendQueryMarketRequest(int commodity)
        {
            object obj = SendRequest<QueryMarketRequest, MarketCommodityOffer>(new QueryMarketRequest(commodity));
            if (obj == null)
                Console.WriteLine("Could not fetch commodity data");
            return (MarketCommodityOffer)obj;
        }

        // send a cancel buy/sell request using the MarketClient project API
        public bool SendCancelBuySellRequest(int id)
        {
            string data = SendRequest<CancelBuySellRequest>(new CancelBuySellRequest(id));
            if (data == null)
            {
                return false;
            }
            if (data.Equals("Ok"))
            {
                //In case when history table's size is 1
                if (HistoryTable.getHistoryList().Last() == HistoryTable.getHistoryList().First())
                {
                    HistoryTable.getHistoryList().First()._status = Status.cancelled;
                    Logger.InfoLog("Request " + HistoryTable.getHistoryList().Last()._id + " has cancelled");
                }
                else
                {
                    HistoryItem item = HistoryTable.getHistoryList().Where(x => x._id == id).First();
                    item._status = Status.cancelled;
                    Logger.InfoLog(item._id + " has cancelled");
                }
                return true;
            }
            return false;
        }

        public QueryUserRequest[] SendQueryUserRequestsRequest()
        {
            QueryUserRequest[] obj = SendRequest<QueryUserRequestsRequest, QueryUserRequest[]>(new QueryUserRequestsRequest());
            return obj;
        }

        public void allHistory()
        {
            HistoryTable.PrintHistory();
        }
        public Commodity[] SendQueryAllMarketRequest()
        {
            Commodity[] obj = SendRequest<QueryAllMarketRequest, Commodity[]>(new QueryAllMarketRequest());
            return obj;
        }
    }
}