using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using System.Diagnostics;

namespace Program
{
    [TestFixture]
    public class TestForProject
    {
        public static void Main(string[] args)
        {

        }

        [PreTest] //Change directory to default.
        public static void initializeCWD()
        {
            Directory.SetCurrentDirectory(@"./ISE172_project/GUI/bin/Debug");
        }

        //Unit 1- checking Statistic class
        [Test]
        public static void TestGetMostTradedComm()
        {
            int[,] a = { { 0, 12 }, { 1, 10 }, { 2, 15 }, { 3, 14 }, { 4, 11 } };
            int mostTradedComm = Statistics.GetMostTradedComm(a);
            Assert.AreEqual(4, mostTradedComm); //Because the sql sort the array before Statitistics gets array.
        }
        [Test]
        public static void TestGetLeastMostTradedComm()
        {
            int[,] a = { { 0, 12 }, { 1, 10 }, { 2, 15 }, { 3, 14 }, { 4, 11 } };
            int leastTradedComm = Statistics.GetLeastTradedComm(a);
            Assert.AreEqual(0, leastTradedComm); //Because the sql sort the array before Statitistics gets array.
        }
        [Test]
        public static void TestCalcAvgPrice()
        {
            Random rnd = new Random();
            Transaction first = new Transaction(0, 12, rnd.Next(0, 50));
            Transaction second = new Transaction(1, 10, rnd.Next(0, 50));
            Transaction third = new Transaction(2, 15, rnd.Next(0, 50));
            Transaction fourth = new Transaction(3, 14, rnd.Next(0, 50));
            Transaction fifth = new Transaction(4, 11, rnd.Next(0, 50));
            Transaction[] transactions = { first, second, third, fourth, fifth };
            double excepted = 0;
            for (int i = 0; i < transactions.Length; i++)
            {
                excepted += transactions[i].getPrice();
            }
            excepted = excepted / (double)transactions.Length;
            double leastTradedComm = Statistics.CalcAvgCommPrice(transactions);
            Assert.AreEqual(excepted, leastTradedComm);
        }
        [Test]
        public static void TestCalcAvgCommPriceByLastNTrades()
        {
            Random rnd = new Random();
            double excepted = 0;
            int n = rnd.Next(20, 100);
            int id = rnd.Next(0, 10);
            Transaction[] transactions = new Transaction[100];
            for (int i = 0; i < transactions.Length; i++)
            {
                transactions[i] = new Transaction(DateTime.Now, id, rnd.Next(10, 40), rnd.Next(1, 40));
            }
            Transaction[] someTransactions = new Transaction[n];
            for (int i = 0; i < someTransactions.Length; i++)
            {
                someTransactions[i] = transactions[transactions.Length - n + i];
            }
            for (int i = 0; i < someTransactions.Length; i++)
            {
                excepted += (double)someTransactions[i].getPrice();
            }
            excepted = excepted / (double)n;
            double result = Statistics.CalcAvgCommPrice(someTransactions);
            Assert.AreEqual(excepted, result);
        }

        //Unit 2- checking reaction of the logger to 3 types of messages
        [Test]
        public static void TestInfoLog()
        {
            string path = @"../../../Logger/LogFiles/" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
            string input = "Test Info Log " + DateTime.Now.ToString() + ".";
            Logger.DebugLog(input);
            System.IO.StreamReader sr = new StreamReader(path);
            string curr = sr.ReadLine();
            bool found = false;
            while (!found & curr != null)
            {
                if (curr.Contains(input))
                    found = true;
                curr = sr.ReadLine();
            }
            sr.Close();
            Assert.AreEqual(true, found);
        }
        [Test]
        public static void TestDebugLog()
        {
            if (!Directory.GetCurrentDirectory().Equals(@"./ISE172_project/GUI/bin/Debug"))
                Directory.SetCurrentDirectory(@"./ISE172_project/GUI/bin/Debug");
            string path = @"../../../Logger/LogFiles/" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
            string input = "Test Debug Log " + DateTime.Now.ToString() + ".";
            Logger.DebugLog(input);
            System.IO.StreamReader sr = new StreamReader(path);
            string curr = sr.ReadLine();
            bool found = false;
            while (!found & curr != null)
            {
                if (curr.Contains(input))
                    found = true;
                curr = sr.ReadLine();
            }
            sr.Close();
            Assert.AreEqual(true, found);
        }
        [Test]
        public static void TestErrorLog()
        {

            string path = @"../../../Logger/LogFiles/" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
            StackFrame sf = new StackFrame(1);
            string input = "Test Error Log " + DateTime.Now.ToString() + ".";
            Logger.ErrorLog(sf.GetMethod(), sf.GetFileLineNumber(), input);
            System.IO.StreamReader sr = new StreamReader(path);
            string curr = sr.ReadLine();
            bool found = false;
            while (!found & curr != null)
            {
                if (curr.Contains(input))
                    found = true;
                curr = sr.ReadLine();
            }
            sr.Close();
            Assert.AreEqual(true, found);
        }
        //Unit 3- Testing parser,socket and their reactions to variety of inputs.
        [Test]
        public static void TestParser()
        {
            Assert.AreEqual(false, Parser.parse("sfgfsgfg"));
            Assert.AreEqual(false, Parser.parse("cancel"));
            Assert.AreEqual(false, Parser.parse("MENU"));
            Assert.AreEqual(false, Parser.parse("ClEAr"));
            Assert.AreEqual(true, Parser.parse("buy 0 0 0"));
            Assert.AreEqual(false, Parser.parse("buy 0 0 0 9"));
            Assert.AreEqual(false, Parser.parse("sell 0 0"));
            Assert.AreEqual(false, Parser.parse("cancell"));
        }
        [Test]
        public static void TestSocket()
        {
            Socket socket = new Socket();
            Assert.AreEqual(-1, socket.generalStringToInt("-1", -1, "The number should be a number different then 0"));
            Assert.AreEqual(-1, socket.generalStringToInt("0.3", -1, "The number should be a number different then 0"));
            Assert.AreEqual(-1, socket.idStringToInt("-999", -1, "Bad ID"));
            Assert.AreEqual(-1, socket.idStringToInt("-44", -1, "Bad ID"));
        }
        //Unit 4- testing history session, adding to history list, update list and insure that cancelled request can not be completed.
        [Test]
        public static void TestSessionHistory()
        {
            Random rnd = new Random();
            BuyRequest b1 = new BuyRequest(rnd.Next(1,10), rnd.Next(1,100),rnd.Next(30,200));
            HistoryItem item1 = new HistoryItem(DateTime.Now, "BuyRequest", b1, rnd.Next(10000, 99999));
            HistoryTable.Add(item1);
            bool found = false;
            foreach (HistoryItem item in HistoryTable.getHistoryList())
                if (item.Equals(item1))
                    found = true;
            Assert.AreEqual(true, found);
            Assert.AreEqual(Status.pending, item1._status);
            HistoryTable.update();
            Assert.AreEqual(Status.completed, item1._status);
            BuyRequest b2=new BuyRequest(rnd.Next(1, 10), rnd.Next(1, 100), rnd.Next(30, 200));
            HistoryItem item2 = new HistoryItem(DateTime.Now, "BuyRequest", b2, rnd.Next(10000, 99999));
            bool found2 = false;
            foreach (HistoryItem item in HistoryTable.getHistoryList())
                if (item.Equals(item2))
                    found2 = true;
            Assert.AreEqual(false, found2);
            HistoryTable.Add(item2);
            foreach (HistoryItem item in HistoryTable.getHistoryList())
                if (item.Equals(item2))
                    found2 = true;
            Assert.AreEqual(true, found2);
            item2._status = Status.cancelled;
            bool found3=false;
            foreach (HistoryItem item in HistoryTable.getHistoryList())
                if (item._status==Status.cancelled)
                    found3 = true;
            Assert.AreEqual(true, found3);
            HistoryTable.update();
            Assert.AreEqual(true, item2._status==Status.cancelled);
        }

    }
}
