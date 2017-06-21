using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Program
{
    // This socket class connects between the BL and the DAL by sending the user input string recieved from the parser to the market client
    public class Socket
    {
        private MarketClient marketClient;

        //Construcor which initializes the market client which is in the DAL
        public Socket()
        {
            this.marketClient = new MarketClient();
        }

        public void printNoValidCommandError()
        {
            Console.WriteLine("No valid command was found. Please try again");
        }

        //String to int convertors
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
                Console.WriteLine("The " + errorMsg + " ID should be a non-negative number");
                return errorVal;
            }
        }

        //Those functions Prepare the string recieved from the Parser and send it to the relevant function in market client

        //Buy Request
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

        //Sell Request
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

        //Cancel Request
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

        public void cancelAll()
        {
            MarketUserData userData = (MarketUserData) this.marketClient.SendQueryUserRequest();
            int[] userRequests = userData.requests;
            if (userRequests.Length != 0)
            {
                String cancelledRequests = "";
                foreach (int id in userRequests)
                {
                    if (!(this.marketClient.SendCancelBuySellRequest(id)))
                        cancelledRequests += "Cannot cancel trade number " + id + "\n";
                }
                if (cancelledRequests.Length == 0)
                    cancelledRequests = "All requests cancelled successfully";
                Console.WriteLine(cancelledRequests);
            }
            else
                Console.WriteLine("No requests to cancel were found");

        }

        public void runAutoMarketAgent()
        {
            AutoMarketAgent autoMarketAgent = new AutoMarketAgent();
            autoMarketAgent.autoPilot();
        }

        //Query Buy/Sell/Market Request
        public void findInfo(String str)
        {
            //3 types of queries - buy request, sell request, commodity(market) request
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

        public void allMarketRequest()
        {
            Commodity[] commodities = this.marketClient.SendQueryAllMarketRequest();
            Console.WriteLine(string.Join<Commodity>("\n", commodities));
        }

        public void userRequestsInfo()
        {
            QueryUserRequest[] requests = this.marketClient.SendQueryUserRequestsRequest();
            if (requests.Length ==0)
                Console.WriteLine("No active requests were found");
            else
                Console.WriteLine(string.Join<QueryUserRequest>("\n", requests));
        }

        //Query User Request
        public void userInfo()
        {
            Console.WriteLine(this.marketClient.SendQueryUserRequest());
        }

        public void allHistory()
        {
            HistoryTable.PrintHistory();
        }

        public void marketShare()
        {
            /*Transaction[] t = DatabaseSocket.getOurLastHistory();
            foreach (Transaction item in t)
            {
                Console.WriteLine(item.ToString());
            }*/
            int[,] marketShare = DatabaseSocket.getMarketShare(1000);
            printMatrix(marketShare);
            
            //Console.WriteLine("Least Traded: " + Statistics.GetLeastTradedStock(marketShare));
            //Console.WriteLine("Most Traded: " + Statistics.GetMostTradedStock(marketShare));
        }

        public void printMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0} ", matrix[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
