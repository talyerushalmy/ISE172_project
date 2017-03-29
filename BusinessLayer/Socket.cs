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
                return Math.Abs(Convert.ToInt32(str));
            }
            catch
            {
                Console.WriteLine(errorMsg);
                return errorVal;
            }
        }

        public void buy(String str)
        {
            String[] words = str.Split(' ');
            if (words.Length == 3)
            {
                int commodity = generalStringToInt(words[0], -1, "The commodity should be a positive integer");
                int amount = generalStringToInt(words[1], 0, "The amount should be a non-negative number");
                int price = generalStringToInt(words[2], 0, "The price should be a non-negative number");
                //goes to buy request
                if (commodity >= 0 && amount != 0 && price != 0)
                {
                    int resp = marketClient.SendBuyRequest(price, commodity, amount);
                    if (resp != -1)
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
                int commodity = generalStringToInt(words[0], -1, "The commodity should be a positive integer");
                int amount = generalStringToInt(words[1], 0, "The amount should be a non-negative number");
                int price = generalStringToInt(words[2], 0, "The price should be a non-negative number");
                //goes to sell request
                if (commodity >= 0 && amount != 0 && price != 0)
                {
                    int resp = this.marketClient.SendSellRequest(price, commodity, amount);
                    if (resp != -1)
                        Console.WriteLine("Success! Trade id: " + resp);
                }
            }
            else
                printNoValidCommandError();
        }
        public void cancel(String str)
        {
            int id = generalStringToInt(str, -1, "The Id should be a non-negative number");
            if (id > -1)
            {
                //goes to cancel request
                if (this.marketClient.SendCancelBuySellRequest(id))
                    Console.WriteLine("Cancelled successfully");
                else
                    Console.WriteLine("Cannot cancel trade number " + id);
            }
            else
            {
                Console.WriteLine();
            }
        }
        public void findInfo(String str)
        {
            //3 types of queries - buy request, sell request, market request
            string[] words = str.ToLower().Split();
            if (words.Length == 2)
            {
                string type = words[0];
                if (type.Equals("commodity"))
                {
                    //goes to query market request
                    int id = generalStringToInt(words[1], -1, "The commodity should be a positive integer");
                    if (id > -1)
                        Console.WriteLine(this.marketClient.SendQueryMarketRequest(id));
                }
                else if (type.Equals("sell") || type.Equals("buy"))
                {
                    //goes to query buy/sell request
                    int id = generalStringToInt(words[1], 0, "The Id should be a positive number");
                    if (id > 0)
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
