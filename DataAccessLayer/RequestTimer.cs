using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public static class RequestTimer
    {
        private static int _length = 18;
        //private static Queue<DateTime> queue = new Queue<DateTime>(LENGTH);
        private static DateTime[] _lastRequests = new DateTime[_length];
        private static int _index = 0;

        public static void addRequest()
        { 
            _lastRequests[_index] = DateTime.Now;
            _index++;
            _index %= _length;
        }

        public static void initializeArray()
        {
            for (int i = 0; i < _lastRequests.Length; i++)
            {
                _lastRequests[i] = new DateTime(2001, 9, 11);
            }
        }

        public static int availableRequests()
        {
            int count = 0;
            foreach (DateTime dt in _lastRequests)
            {
                if (deltaSeconds(dt) > 10)
                    count++;
            }
            Console.WriteLine("availableRequests: " + count);
            return count;
        }

        public static DateTime getOldestRequest()
        {
            return _lastRequests[(_index) % _lastRequests.Length];
        }

        public static void printPleaseWait()
        {
            Console.WriteLine("Please wait " + getWaitTime() + " seconds before sending another request");
        }

        public static int getWaitTime()
        {
            DateTime date = getOldestRequest();
            int seconds = deltaSeconds(date);
            Console.WriteLine("The wait time is " + seconds);
            return seconds;
        }

        public static int getWaitTime(int n)
        {
            DateTime date = _lastRequests[(_index + n - 1) % _lastRequests.Length];
            int seconds = deltaSeconds(date);
            int start = (_index + n - 1) % _lastRequests.Length;
            Console.WriteLine("The wait time is " + seconds);
            return seconds;
        }

        public static int deltaSeconds(DateTime date)
        {
            DateTime now = DateTime.Now;
            return (int)Math.Ceiling(now.Subtract(date).TotalSeconds);
        }

        public static DateTime[] getLastRequests()
        {
            return _lastRequests;
        }

        public static void wait()
        {
            if (RequestTimer.availableRequests() <= 0)
            {
                Console.WriteLine("wait time :" + RequestTimer.getWaitTime());
                System.Threading.Thread.Sleep((RequestTimer.getWaitTime() + 1) * 1000);
            }
        }

        public static void wait(int n)
        {
            if (RequestTimer.availableRequests() <= 0)
            {
                Console.WriteLine("wait time :" + RequestTimer.getWaitTime(n));
                System.Threading.Thread.Sleep((RequestTimer.getWaitTime(n) + 1) * 1000);
            }
        }
    }
}
