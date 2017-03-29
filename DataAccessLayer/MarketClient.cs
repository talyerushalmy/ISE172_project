using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{

    public class MarketClient : IMarketClient
    {
        static string[] errorsNotToPrint = {
            "No auth key",
            "No user or auth token",
            "Verification failure",
            "No type key"
        };

        private SimpleHTTPClient client;
        private RequestBase req = new RequestBase();

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

        private string SendRequest<T>(T data)
        {
            try
            {
                return this.client.SendPostRequest<T>(this.req.getUrl(), this.req.getUser(), this.req.getToken(), data);
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                printError(response); // Print the error
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
                printError(response); // Print the error
                return -1;
            }
        }

        public IMarketItemQuery SendQueryBuySellRequest(int id)
        {
            object obj = SendRequest<QueryBuySellRequest, MarketItemQuery>(new QueryBuySellRequest(id));
            return (MarketItemQuery)obj;
        }

        public IMarketUserData SendQueryUserRequest()
        {
            object obj = SendRequest<QueryUserRequest, MarketUserData>(new QueryUserRequest());
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
