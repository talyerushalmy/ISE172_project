using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public static class Parser
    {
        public static void parser(String str)
        {
            String[] words = str.Split(' ');
            switch (words[0].ToLower())
            {
                case "buy":
                    {
                        if (words.Length == 3)
                            buy(str.Substring(4));
                        else
                            printNoValidCommandError();
                    }
                    break;
                case "sell":
                    {
                        if (words.Length == 3)
                            sell(str.Substring(5));
                        else
                            printNoValidCommandError();
                    }
                    break;
                case "cancel":
                    {
                        if (words.Length == 2)
                        {
                            //stringToInt(words[1]);
                            cancel(str.Substring(7));
                        }
                        else
                            printNoValidCommandError();
                    }
                    break;
                case "info":
                    {
                        if (words.Length == 2)
                            info(str.Substring(7));
                        else
                            printNoValidCommandError();
                    }
                    break;
                default:
                    {
                        // no command was identified
                        printNoValidCommandError();
                    }
                    break;
            }
        }

        public static void printNoValidCommandError()
        {
            Console.WriteLine("No valid command was found. Please try again");
        }

        public static int stringToInt(String str)
        {
            int id;
            try
            {
                id = Convert.ToInt32(str);
                return id;
            }
            catch (Exception e)
            {
                Console.WriteLine("You haven't entered a number");
                return -1;
            }
        }

        public static void buy(String str)
        {
            String[] words = str.Split(' ');

        }
        public static void sell(String str)
        {

        }
        public static void cancel(String str)
        {
            int id = stringToInt(str);
            //Goes to bool SendCancelBuySellRequest(int id) in the IMarketClient Interface
        }
        public static void info(String str)
        {
            int id = stringToInt(str);

        }
        public static bool isInStock(String str)
        {
            return false;
        }
    }
}
