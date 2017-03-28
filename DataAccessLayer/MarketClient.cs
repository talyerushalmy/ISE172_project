using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{

    class MarketClient : IMarketClient
    {
        private SimpleHTTPClient client;
        private RequestBase req = new RequestBase();

        public MarketClient()
        {
            this.client = new SimpleHTTPClient();
            this.req = new RequestBase();
        }

        private string SendRequest<T>(T data)
        {
            try
            {
                string st = this.client.SendPostRequest<T>(this.req.getUrl(), this.req.getUser(), this.req.getToken(), data);
                return st;
            }
            catch
            {
                return null;
            }
        }

        private T2 SendRequest<T1, T2>(T1 data) where T2 : class
        {
            try
            {
                return this.client.SendPostRequest<T1, T2>(this.req.getUrl(), this.req.getUser(), this.req.getToken(), data);
            }
            catch
            {
                return null;
            }
        }

        public int SendBuyRequest(int price, int commodity, int amount)
        {
            string response = "Unknown error";
            try
            {
                BuyRequest data = new BuyRequest(commodity, amount, price);
                response = SendRequest(data);
                return Convert.ToInt32(response);
            }
            catch
            {
                Console.WriteLine(response); // Print the error
                return -1;
            }
        }

        public int SendSellRequest(int price, int commodity, int amount)
        {
            string response = "Unknown error";
            try
            {
                SellRequest data = new SellRequest(commodity, amount, price);
                response = SendRequest(data);
                return Convert.ToInt32(response);
            }
            catch
            {
                Console.WriteLine(response); // Print the error
                return -1;
            }
        }

        public IMarketItemQuery SendQueryBuySellRequest(int id)
        {
            object obj = SendRequest<QueryBuySellRequest, MarketItemQuery>(new QueryBuySellRequest(id));
            if (obj == null)
                Console.WriteLine("Id not found");
            return (MarketItemQuery)obj;
        }

        public IMarketUserData SendQueryUserRequest()
        {
            object obj = SendRequest<QueryUserRequest, MarketUserData>(new QueryUserRequest());
            if (obj == null)
                Console.WriteLine("Could not fetch user data");
            return (MarketUserData)obj;
        }

        public IMarketCommodityOffer SendQueryMarketRequest(int commodity)
        {
            object obj = SendRequest<QueryMarketRequest, MarketCommodityOffer>(new QueryMarketRequest(commodity));
            if (obj == null)
                Console.WriteLine("Could not fetch commodity data");
            return (MarketCommodityOffer)obj;
        }

        public bool SendCancelBuySellRequest(int id)
        {
            string data = SendRequest<CancelBuySellRequest>(new CancelBuySellRequest(id));
            if (data == null)
                return false;
            return data.Equals("Ok");
        }
    }
}
