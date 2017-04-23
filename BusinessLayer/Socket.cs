using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                StackFrame st = new StackFrame(0, true);
                String file = st.GetFileName();
                String line = Convert.ToString(st.GetFileLineNumber());
                Logger.logError(file, line);
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
                    StackFrame st = new StackFrame(0, true);
                    String file = st.GetFileName();
                    String line = Convert.ToString(st.GetFileLineNumber());
                    Logger.logError(file, line);
                    return errorVal;
                }
            }
            catch
            {
                Console.WriteLine("The " + errorMsg + " ID should be a non-negative number");
                StackFrame st = new StackFrame(0, true);
                String file = st.GetFileName();
                String line = Convert.ToString(st.GetFileLineNumber());
                Logger.logError(file, line);
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
                    Logger.logMessage("Buy request is sent to MarketClient ");
                    int resp = marketClient.SendBuyRequest(price, commodity, amount);
                    if (resp >= 0)
                    {
                        Logger.logMessage("Success of buy request");
                        //if resp=0 Log- Problem with communication with the server.
                        Console.WriteLine("Success! Trade id: " + resp);
                    }
                }
            }
            else
            {
                StackFrame st = new StackFrame(0, true);
                String file = st.GetFileName();
                String line = Convert.ToString(st.GetFileLineNumber());
                Logger.logError(file, line);
                printNoValidCommandError();
            }

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
                    Logger.logMessage("Sell request is sent to MarketClient ");
                    int resp = this.marketClient.SendSellRequest(price, commodity, amount);
                    if (resp >= 0)
                    {
                        //if resp>0 Log- success.
                        //if resp=0 Log- Problem with communication with the server.
                        Console.WriteLine("Success! Trade id: " + resp);
                    }
                }
            }
            else
            {
                StackFrame st = new StackFrame(0, true);
                String file = st.GetFileName();
                String line = Convert.ToString(st.GetFileLineNumber());
                Logger.logError(file, line);
                printNoValidCommandError(); //Log- fail- the user entered invalid values.
            }
        }

        //Cancel Request
        public void cancel(String str)
        {
            int id = idStringToInt(str, -1, "cancel request");
            if (id > -1)
            {
                //goes to cancel request
                Logger.logMessage("Cancel request is sent to MarketClient ");
                if (this.marketClient.SendCancelBuySellRequest(id))
                {
                    Console.WriteLine("Cancelled successfully");
                    Logger.logMessage("Cancelled successfully");
                }
                else
                {
                    Console.WriteLine("Cannot cancel trade number " + id);
                    StackFrame st = new StackFrame(0, true);
                    String file = st.GetFileName();
                    String line = Convert.ToString(st.GetFileLineNumber());
                    Logger.logError(file, line);
                }
            }
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
                    {
                        Logger.logMessage("Find info request is sent to MarketClient ");
                        Console.WriteLine(this.marketClient.SendQueryBuySellRequest(id));
                    }
                }
                else
                {
                    StackFrame st = new StackFrame(0, true);
                    String file = st.GetFileName();
                    String line = Convert.ToString(st.GetFileLineNumber());
                    Logger.logError(file, line);
                    printNoValidCommandError();
                }
            }
            else
            {
                StackFrame st = new StackFrame(0, true);
                String file = st.GetFileName();
                String line = Convert.ToString(st.GetFileLineNumber());
                Logger.logError(file, line);
                printNoValidCommandError();
            }
        }

        //Query User Request
        public void userInfo()
        {
            Logger.logMessage("User's information request is sent to MarketClient ");
            Console.WriteLine(this.marketClient.SendQueryUserRequest());
            Logger.logMessage("The user got his information");
        }
    }
}
