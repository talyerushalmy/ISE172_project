using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public static class RequestTimer
    {
        private static int _length = 20;
        private static Queue<DateTime> _lastRequests = new Queue<DateTime>(_length);

        public static void addRequest()
        {
            update();
            if (_lastRequests.Count < 20)
                _lastRequests.Enqueue(DateTime.Now);
            else
                printPleaseWait();
        }

        public static void update()
        {
            while (_lastRequests.Count > 0 && deltaSeconds(_lastRequests.Peek())>10)
            {
                _lastRequests.Dequeue();
            }
        }

        public static int availableRequests()
        {
            update();
            return _length - _lastRequests.Count;
        }

        public static DateTime getOldestRequest()
        {
            update();
            return _lastRequests.Peek();
        }

        public static void printPleaseWait()
        {
            Console.WriteLine("Please wait " + getWaitTime() + " seconds before sending another request");
        }

        public static int getWaitTime()
        {
            update();
            return deltaSeconds(_lastRequests.Peek());
        }

        public static int getWaitTime(int n)
        {
            update();
            return deltaSeconds(_lastRequests.ElementAt(n - 1));
        }

        public static int deltaSeconds(DateTime date)
        {
            DateTime now = DateTime.Now;
            return (int)Math.Ceiling(now.Subtract(date).TotalSeconds);
        }

        public static Queue<DateTime> getLastRequests()
        {
            return _lastRequests;
        }

        public static void wait()
        {
            update();
            if (availableRequests() <= 0)
            {
                System.Threading.Thread.Sleep(getWaitTime() * 1000);
            }
        }

        public static void wait(int n)
        {
            update();
            if (availableRequests() <= 0)
            {
                System.Threading.Thread.Sleep(getWaitTime(n) * 1000);
            }
        }
    }
}
