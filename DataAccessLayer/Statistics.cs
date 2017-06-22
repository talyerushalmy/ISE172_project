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
        // Gets the Kth most traded commodity in the market share (sorted in ascending order)
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

        // Gets the Kth most traded commodity in the last N trades made in the market
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

        // Gets the most traded commodity in the given market share
        public static int GetMostTradedComm(int[,] marketShare)
        {
            return marketShare[marketShare.GetLength(0)-1, 0];
        }

        // Gets the most traded commodity based on the last N trades made in the market
        public static int GetMostTradedComm(int numOfTrades, int k)
        {
            int[,] marketShare = DatabaseSocket.getMarketShare(numOfTrades);
            return GetMostTradedComm(marketShare);
        }

        // Gets the least traded commodity in the given market share
        public static int GetLeastTradedComm(int[,] marketShare)
        {
            return marketShare[0, 0];
        }

        // Gets the least traded commodity based on the last N trades made in the market
        public static int GetLeastTradedComm(int numOfTrades)
        {
            int[,] marketShare = DatabaseSocket.getMarketShare(numOfTrades);
            return GetLeastTradedComm(marketShare);
        }

        // Calculates the average price of a certain commodity based on a serias of trades
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

        // Calculates the average price of the given commodity based on its last N trades in the market
        public static double CalcAvgCommPriceByLastNTrades(int commID, int n)
        {
            Transaction[] lastNTransactions = DatabaseSocket.getPriceOfCommByLastNTrades(commID, n);
            return CalcAvgCommPrice(lastNTransactions);
        }
    }
}
