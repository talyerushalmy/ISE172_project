using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public static class HistoryTable
    {
        private readonly static LinkedList<HistoryItem> _historyList = new LinkedList<HistoryItem>();
        private static MarketClient _marketClient = new MarketClient();

        public static void Add(HistoryItem toAdd)
        {
            _historyList.AddLast(toAdd);
        }
        public static void update()
        {
            int[] requests = ((MarketUserData)_marketClient.SendQueryUserRequest()).requests;
            foreach (HistoryItem item in _historyList.Where(x => x._status == Status.pending).Where(x => !requests.Contains(x._id)))
            {
                item._status = Status.completed;
                Logger.InfoLog("Request " + item._id + " has completed");
            }
        }
        public static LinkedList<HistoryItem> getHistoryList()
        {
            return _historyList;
        }

        public static void PrintHistory()
        {
            update();
            Console.WriteLine("History");
            Console.WriteLine("-------");
            foreach (var item in getHistoryList())
            {
                Console.WriteLine("id " + item._id + ", type request: " + item._requestType + ", date: " + item._date + ", information: " + item._info + ", Status: " + item._status);
            }
        }
    }

}
