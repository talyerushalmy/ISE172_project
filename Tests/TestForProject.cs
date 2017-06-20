using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;

namespace Program
{
    [TestFixture]
    public class TestForProject
    {
        public static void Main(string[] args)
        {
            TestStatistics();
        }

        
        public static void TestStatistics()
        {
            TestGetMostTradedComm();
            TestGetLeastMostTradedComm();
            TestCalcAvgPrice();
        }
        [Test]
        public static void TestGetMostTradedComm()
        {
            int[,] a = { { 0, 12 }, { 1, 10 }, { 2, 15 }, { 3, 14 }, { 4, 11 } };
            int mostTradedComm=Statistics.GetMostTradedComm(a);
            Assert.AreEqual(0, mostTradedComm); //Because the sql sort the array before Statitistics gets array.
        }
        [Test]
        public static void TestGetLeastMostTradedComm()
        {
            int[,] a = { { 0, 12 }, { 1, 10 }, { 2, 15 }, { 3, 14 }, { 4, 11 } };
            int leastTradedComm = Statistics.GetLeastTradedComm(a);
            Assert.AreEqual(4, leastTradedComm); //Because the sql sort the array before Statitistics gets array.
        }
        [Test]
        public static void TestCalcAvgPrice()
        {
            Random rnd = new Random();
            Transaction first = new Transaction(0, 12, rnd.Next(0,50));
            Transaction second = new Transaction(1, 10, rnd.Next(0, 50));
            Transaction third = new Transaction(2, 15, rnd.Next(0, 50));
            Transaction fourth = new Transaction(3, 14, rnd.Next(0, 50));
            Transaction fifth = new Transaction(4, 11, rnd.Next(0, 50));
            Transaction[] transactions = { first, second, third, fourth, fifth };
            double excepted = 0;
            for(int i = 0; i < transactions.Length; i++)
            {
                excepted += transactions[i].getPrice();
            }
            excepted = excepted / (double)transactions.Length;
            double leastTradedComm = Statistics.CalcAvgPrice(transactions);
            Assert.AreEqual(excepted, leastTradedComm);
        }


    }
}
