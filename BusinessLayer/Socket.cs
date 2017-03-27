using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class Socket
    {
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
            catch (Exception e)
            {
                Console.WriteLine("You haven't entered a number");
                return -1;
            }
        }

        public void buy(String str)
        {
            String[] words = str.Split(' ');
            if (words.Length == 2)
            {
                if (isInStock(words[0]))
                {

                }
            }

        }
        public  void sell(String str)
        {
            
        }
        public void cancel(String str)
        {
            int id = stringToInt(str);
            //Goes to bool SendCancelBuySellRequest(int id) in the IMarketClient Interface
        }
        public void info(String str)
        {
            //3 types of queries - request, user, market
            if (str.ToLower().Equals("market"))
            {

            }
            else if (str.ToLower().Equals("sell"))
            {

            }
            else if (str.ToLower().Equals("buy"))
            {

            }
        }
        public static bool isInStock(String str)
        {
            return true;
        }
    }
}
