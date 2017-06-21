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

        }
    }
}
