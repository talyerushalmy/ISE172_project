using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{

    // an implementation of the IMarketClient interface
    public class MarketClient : IMarketClient
    {
        // a dictionary (or an array, to be precise) of errors not to print on occurrence
        static string[] errorsNotToPrint = {
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
                return this.client.SendPostRequest<T>(this.req.getUrl(), this.req.getUser(), this.req.getToken(), data);
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
                return this.client.SendPostRequest<T1, T2>(this.req.getUrl(), this.req.getUser(), this.req.getToken(), data);
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
                //Log- Buy Request is sent to the server.
                BuyRequest data = new BuyRequest(commodity, amount, price);
                response = SendRequest(data);
                return Convert.ToInt32(response);
            }
            catch
            {
                StackFrame st = new StackFrame(0, true);
                String file = st.GetFileName();
                String line = Convert.ToString(st.GetFileLineNumber());
                Logger.logError(file, line, "The server can't perform buy request");
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
                //Log- Sell Request is sent to the server.
                SellRequest data = new SellRequest(commodity, amount, price);
                response = SendRequest(data);
                return Convert.ToInt32(response);
            }
            catch
            {
                StackFrame st = new StackFrame(0, true);
                String file = st.GetFileName();
                String line = Convert.ToString(st.GetFileLineNumber());
                Logger.logError(file, line, "The server can't perform sell");
                printError(response); // Print the error
                return -1;
            }
        }

        // send a query buy/sell request using the MarketClient project API
        public IMarketItemQuery SendQueryBuySellRequest(int id)
        {
            //Log BuySell Request is sent to the server
            object obj = SendRequest<QueryBuySellRequest, MarketItemQuery>(new QueryBuySellRequest(id));
            return (MarketItemQuery)obj;
        }

        // send a query user request using the MarketClient project API
        public IMarketUserData SendQueryUserRequest()
        {
            //Log- User Query Request is sent to the server
            object obj = SendRequest<QueryUserRequest, MarketUserData>(new QueryUserRequest());
            return (MarketUserData)obj;
        }

        // send a query market request using the MarketClient project API
        public IMarketCommodityOffer SendQueryMarketRequest(int commodity)
        {
            //Log- Market Request is sent to the server
            object obj = SendRequest<QueryMarketRequest, MarketCommodityOffer>(new QueryMarketRequest(commodity));
            if (obj == null)
            {
                Console.WriteLine("Could not fetch commodity data");
                StackFrame st = new StackFrame(0, true);
                String file = st.GetFileName();
                String line = Convert.ToString(st.GetFileLineNumber());
                Logger.logError(file, line, "The server can't find of commodity/buy/sell");
            }
            return (MarketCommodityOffer)obj;
        }

        // send a cancel buy/sell request using the MarketClient project API
        public bool SendCancelBuySellRequest(int id)
        {
            //Log-Cancel request is sent to the server.
            string data = SendRequest<CancelBuySellRequest>(new CancelBuySellRequest(id));
            if (data == null)
            {
                StackFrame st = new StackFrame(0, true);
                String file = st.GetFileName();
                String line = Convert.ToString(st.GetFileLineNumber());
                Logger.logError(file, line, "The server can't perform cancel of request with id " + id);
                return false;
            }
                
            return data.Equals("Ok");
        }
    }
}
