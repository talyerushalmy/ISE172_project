using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketClient.DataEntries;

namespace Program
{

    class Request : IMarketClient
    {
        private SimpleHTTPClient client;
        private string KEY_PATH = @"..\..\..\private_key";
        private string user;
        private string token;
        private string url;

        public void setToken(string KEY_PATH = @"..\..\..\private_key")
        {
            try
            {
                string privateKey = System.IO.File.ReadAllText(KEY_PATH);
                this.token = SimpleCtyptoLibrary.CreateToken(this.user, privateKey);
            }
            catch
            {
                throw new Exception("Error reading the private key file");
            }
        }

        public Request()
        {
            this.client = new SimpleHTTPClient();
            this.url = @"http://ise172.ise.bgu.ac.il/";
            this.user = "user46";
            setToken(KEY_PATH);
        }

        public string getUser() { return this.user; }

        public string getToken() { return this.token; }

        private string SendRequest<T>(T request)
        {
            try
            {
                return this.client.SendPostRequest<T>(this.url, this.user, this.token, request);
            }
            catch
            {
                return null;
            }
        }

        private T2 SendRequest<T1, T2>(T1 request) where T2 : class
        {
            try
            {
                return this.client.SendPostRequest<T1, T2>(this.url, this.user, this.token, request);
            }
            catch
            {
                return null;
            }
        }

        public int SendBuyRequest(int price, int commodity, int amount)
        {
            try
            {
                SendRequest(new BuyRequest(commodity, amount, price));
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public int SendSellRequest(int price, int commodity, int amount)
        {
            try
            {
                SendRequest(new SellRequest(commodity, amount, price));
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public IMarketItemQuery SendQueryBuySellRequest(int id)
        {
            return SendRequest<QueryBuySellRequest, IMarketItemQuery>(new QueryBuySellRequest(id));
        }

        public IMarketUserData SendQueryUserRequest()
        {
            return SendRequest<QueryUserRequest, IMarketUserData>(new QueryUserRequest());
        }

        public IMarketCommodityOffer SendQueryMarketRequest(int commodity)
        {
            return SendRequest<QueryMarketRequest, IMarketCommodityOffer>(new QueryMarketRequest(commodity));
        }

        public bool SendCancelBuySellRequest(int id)
        {
            return SendRequest<CancelBuySellRequest>(new CancelBuySellRequest(id)).Equals("Ok");
        }
    }
}
