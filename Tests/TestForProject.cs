using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace Program
{
    [TestFixture]
    public class TestForProject
    {
        public static void Main(string[] args)
        {
            testUnit1();
        }
        [Test]
        public static void testUnit1()
        {
            MarketClient m = new MarketClient();
            m.SendSellRequest(1, 1, 1);

        //Unit 2- checking reaction of the logger to 3 types of messages
        [Test]
        public static void TestInfoLog()
        {
            //Directory.SetCurrentDirectory(@"./GUI/bin/Debug");
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
            Directory.SetCurrentDirectory(@"./GUI/bin/Debug");
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
            //Directory.SetCurrentDirectory(@"./GUI/bin/Debug");
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
            //Directory.SetCurrentDirectory(@"./GUI/bin/Debug");
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
            //Directory.SetCurrentDirectory(@"./GUI/bin/Debug");
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
            //Directory.SetCurrentDirectory(@"./GUI/bin/Debug");
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
