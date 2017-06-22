using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public static class Statistics
    {
        //This class's purpose is to analyze informaion that stored in a given array/table and prevent it to AMA or GUI.
        public static int GetKthTradedComm(int[,] marketShare, int k)
        {
            try
            {
                return marketShare[k, 0];
            }
            catch
            {
                return -1;
            }
        }

        public static int GetKthTradedComm(int numOfTrades, int k)
        {
            try
            {
                int[,] marketShare = DatabaseSocket.getMarketShare(numOfTrades);
                return GetKthTradedComm(marketShare, k);
            }
            catch
            {
                return -1;
            }
        }

        public static int GetMostTradedComm(int[,] marketShare)
        {
            return marketShare[marketShare.GetLength(0)-1, 0];
        }

        public static int GetMostTradedComm(int numOfTrades, int k)
        {
            int[,] marketShare = DatabaseSocket.getMarketShare(numOfTrades);
            return GetMostTradedComm(marketShare);
        }

        public static int GetLeastTradedComm(int[,] marketShare)
        {
            return marketShare[0, 0];
        }

        public static int GetLeastTradedComm(int numOfTrades)
        {
            int[,] marketShare = DatabaseSocket.getMarketShare(numOfTrades);
            return GetLeastTradedComm(marketShare);
        }

        public static double CalcAvgCommPrice(Transaction[] transactions)
        {
            double sum = 0;
            for (int i = 0; i < transactions.Length; i++)
            {
                sum += transactions[i].getPrice();
            }
            if (transactions.Length == 0)
                return -1;
            return sum / (double)transactions.Length;
        }

        public static double CalcAvgCommPriceByLastNTrades(int commID, int n)
        {
            Transaction[] lastNTransactions = DatabaseSocket.getPriceOfCommByLastNTrades(commID, n);
            return CalcAvgCommPrice(lastNTransactions);
        }
    }
}
