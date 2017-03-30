using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class Socket
    {
        private MarketClient marketClient;

        public Socket()
        {
            this.marketClient = new MarketClient();
        }

        public void printNoValidCommandError()
        {
            Console.WriteLine("No valid command was found. Please try again");
        }

        public int generalStringToInt(string str, int errorVal, string errorMsg)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch
            {
                Console.WriteLine(errorMsg);
                return errorVal;
            }
        }

        public int idStringToInt(string str, int errorVal, string errorMsg)
        {
            try
            {
                int id = Convert.ToInt32(str);
                if (id >= 0)
                    return id;
                else
                {
                    Console.WriteLine("The " + errorMsg + " ID should be a non-negative number");
                    return errorVal;
                }
            }
            catch
            {
                Console.WriteLine("The commodity ID should be a non-negative number");
                return errorVal;
            }
        }

        public void buy(String str)
        {
            String[] words = str.Split(' ');
            if (words.Length == 3)
            {
                int commodity = idStringToInt(words[0], -1, "commodity");
                int amount = generalStringToInt(words[1], 0, "The amount should be a number different then 0");
                int price = generalStringToInt(words[2], 0, "The price should be a number different then 0");
                //goes to buy request
                if (commodity >= 0 && amount != 0 && price != 0)
                {
                    int resp = marketClient.SendBuyRequest(price, commodity, amount);
                    if (resp >=0)
                        Console.WriteLine("Success! Trade id: " + resp);
                }
            }
            else
                printNoValidCommandError();

        }

        public  void sell(String str)
        {
            String[] words = str.Split(' ');
            if (words.Length == 3)
            {
                int commodity = idStringToInt(words[0], -1, "commodity");
                int amount = generalStringToInt(words[1], 0, "The amount should be a number different then 0");
                int price = generalStringToInt(words[2], 0, "The price should be a number different then 0");
                //goes to sell request
                if (commodity >= 0 && amount != 0 && price != 0)
                {
                    int resp = this.marketClient.SendSellRequest(price, commodity, amount);
                    if (resp >=0)
                        Console.WriteLine("Success! Trade id: " + resp);
                }
            }
            else
                printNoValidCommandError();
        }
        public void cancel(String str)
        {
            int id = idStringToInt(str, -1, "cancel request");
            if (id > -1)
            {
                //goes to cancel request
                if (this.marketClient.SendCancelBuySellRequest(id))
                    Console.WriteLine("Cancelled successfully");
                else
                    Console.WriteLine("Cannot cancel trade number " + id);
            }
        }
        public void findInfo(String str)
        {
            //3 types of queries - buy request, sell request, commodity request
            string[] words = str.ToLower().Split();
            if (words.Length == 2)
            {
                string type = words[0];
                if (type.Equals("commodity"))
                {
                    //goes to query market request
                    int id = idStringToInt(words[1], -1, "commodity");
                    if (id > -1)
                        Console.WriteLine(this.marketClient.SendQueryMarketRequest(id));
                }
                else if (type.Equals("sell") || type.Equals("buy"))
                {
                    //goes to query buy/sell request
                    int id = idStringToInt(words[1], -1, "" + type + " request");
                    if (id > -1)
                        Console.WriteLine(this.marketClient.SendQueryBuySellRequest(id));
                }
                else
                    printNoValidCommandError();
            }
            else
                printNoValidCommandError();
        }
        public void userInfo()
        {
            Console.WriteLine(this.marketClient.SendQueryUserRequest());
        }
    }
}
