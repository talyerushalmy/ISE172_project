using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Program
{
    public static class HistoryTable
    {
        private static string FILENAME = "History_File";
        private readonly static LinkedList<HistoryItem> _historyList = new LinkedList<HistoryItem>();
        private static MarketClient _marketClient = new MarketClient();

        public static void SaveList()
        {
            string json = JsonConvert.SerializeObject(_historyList);
            StreamWriter streamWriter = new StreamWriter(FILENAME, true);
            streamWriter.Write("History from " + DateTime.Now.ToLongDateString() + '\n' + json + "\n\n");
            streamWriter.Close();
        }

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
