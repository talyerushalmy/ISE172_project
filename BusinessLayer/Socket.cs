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

        public int stringToInt(String str)
        {
            int id;
            try
            {
                id = Convert.ToInt32(str);            
                return id;
            }
            catch
            {
                Console.WriteLine("Your argument is not a number");
                return -1;
            }
        }

        public void buy(String str)
        {
            String[] words = str.Split(' ');
            if (words.Length == 3)
            {
                int commodity = stringToInt(words[0]);
                int amount = stringToInt(words[1]);
                int price = stringToInt(words[2]);
                //goes to buy request
                marketClient.SendBuyRequest(price, commodity, amount);
            }
            else
                printNoValidCommandError();

        }

        public  void sell(String str)
        {
            String[] words = str.Split(' ');
            if (words.Length == 3)
            {
                int commodity = stringToInt(words[0]);
                int amount = stringToInt(words[1]);
                int price = stringToInt(words[2]);
                //goes to sell request
                marketClient.SendSellRequest(price, commodity, amount);
            }
            else
                printNoValidCommandError();
        }
        public void cancel(String str)
        {
            int id = stringToInt(str);
            //goes to cancel request
            marketClient.SendCancelBuySellRequest(id);
        }
        public void info(String str)
        {
            //3 types of queries - buy request, sell request, market request
            string[] words = str.ToLower().Split();
            if (words.Length == 2)
            {
                string type = words[0];
                int id = stringToInt(words[1]);
                if (type.Equals("commodity"))
                {
                    //goes to query market request
                    marketClient.SendQueryMarketRequest(id);
                }
                else if (type.Equals("sell") || type.Equals("buy") || type.Equals("trade"))
                {
                    //goes to query buy/sell request
                    marketClient.SendQueryBuySellRequest(id);
                }
            }
            else
                printNoValidCommandError();
        }
        public void userInfo()
        {
            this.marketClient.SendQueryUserRequest();
        }
    }
}
