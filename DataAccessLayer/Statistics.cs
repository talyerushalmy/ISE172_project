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
        public static int GetKthTradedComm(int [,] marketShare,int k)
        {
            return marketShare[k, 0];
        }

        public static int GetKthTradedComm(int numOfTrades, int numOfComms, int k)
        {
            int[,] marketShare = DatabaseSocket.marketShare(numOfTrades, numOfComms);
            return GetKthTradedComm(marketShare, k);
        }
        public static int GetMostTradedComm(int[,] marketShare)
        {
            return marketShare[0, 0];
        }
        public static int GetLeastTradedComm(int[,] marketShare)
        {
            return marketShare[marketShare.GetLength(0)-1, 0];
        }

        public static double CalcAvgPrice(Transaction[] transactions)
        {
            double sum = 0;
            for(int i = 0; i < transactions.Length; i++)
            {
                sum += transactions[i].getPrice();
            }
            return sum/ (double)transactions.Length;
        }

    }
}
