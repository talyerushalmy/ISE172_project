using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Program
{
    public static class DatabaseSocket
    {
        //This class supposed to extract variety of information from the server.
        private static string _connectionString = @"Data Source=ise172.ise.bgu.ac.il;Initial Catalog=history;Persist Security Info=True;User ID=labuser;Password=wonsawheightfly";
        private static SqlConnection _myConnection = new SqlConnection(_connectionString);

        // Returns a table of all the history of the market
        public static DataTable GetAllHistory(bool justOurs)
        {
            DataTable history = new DataTable();
            try
            {
                _myConnection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[items] ORDER BY timestamp DESC");
                history.Load(command.ExecuteReader());
                _myConnection.Close();
                return history;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return history;
            }
        }

        // Returns a table of the market history of the last N trades 
        public static DataTable GetHistoryOfLastNTrades(int n, bool justOurs)
        {
            DataTable history = new DataTable();
            try
            {
                if (n<0 || n> 7331)
                {
                    throw new ArgumentException("n is out of range");
                }
                _myConnection.Open();
                SqlCommand command = new SqlCommand("SELECT TOP " + n + " * FROM [dbo].[items] ORDER BY timestamp DESC");
                history.Load(command.ExecuteReader());
                _myConnection.Close();
                return history;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return history;
            }
        }
 
        public static DataTable GetHistoryOfLastDay()
        {
            DataTable history = new DataTable();
            try
            {
                _myConnection.Open();
                string query = "SELECT TOP " + LIMIT + " * FROM[dbo].[items] WHERE[timestamp] BETWEEN CONVERT(datetime,'"+start+"', 105) AND CONVERT(datetime,'"+end+"', 105)";
                if (justOurs)
                    query += " WHERE buyer='user46' OR seller='user46'";
                query += " ORDER BY timestamp DESC";
                Console.WriteLine(start);
                SqlCommand command = new SqlCommand(query, _myConnection);
                history.Load(command.ExecuteReader());
                Console.WriteLine("Last timestamp : " + history.Rows[history.Rows.Count-1].ItemArray[0]);
                Console.WriteLine("First timestamp : " + history.Rows[0].ItemArray[0]);
                Console.WriteLine();
                _myConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
            }
            return history;
        }

        // Returns a table which contains the market history of the last day
        public static DataTable GetHistoryOfLastDay(bool justOurs)
        {
            DataTable historyOfToday = new DataTable();
            try
            {
                DateTime today = DateTime.Now;
                string yesterday = today.AddDays(-1).ToString();
                string now = today.ToString();
                Console.WriteLine("TODAY : " + today);
                Console.WriteLine("YESTERDAY : " + yesterday);
                _myConnection.Open();
                SqlCommand command = new SqlCommand(@"SELECT * FROM [dbo].[items] WHERE [timestamp]>=DATEADD(day,-1,GETUTCDATE()) ORDER BY timestamp DESC", _myConnection);
                historyOfToday.Load(command.ExecuteReader());
                Console.WriteLine("Last timestamp :" + Convert.ToDateTime(historyOfToday.Rows[historyOfToday.Rows.Count - 1].ItemArray[0]));
                Console.WriteLine("First timestamp :" + (historyOfToday.Rows[0].ItemArray[0]));
                _myConnection.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();

            }
            return historyOfToday;
        }

        // Returns a table which contains the market history of the last hour
        public static DataTable GetHistoryOfLastHour(bool justOurs)
        {
            DataTable history = new DataTable();
            try
            {
                _myConnection.Open();
                SqlCommand command = new SqlCommand(@"SELECT * FROM [dbo].[items] WHERE [timestamp]>=DATEADD(hour,-1,GETUTCDATE()) ORDER BY timestamp DESC", _myConnection);
                history.Load(command.ExecuteReader());
                _myConnection.Close();
                Console.WriteLine("Last timestamp :" + Convert.ToDateTime(history.Rows[history.Rows.Count - 1].ItemArray[0]));
                Console.WriteLine("First timestamp :" + (history.Rows[0].ItemArray[0]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
            }
            return history;
        }

        // Returns an array representing the market shared based on the last N trades of the market
        public static int[,] getMarketShare(int n)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"WITH s AS (SELECT TOP " + n + " * FROM dbo.items ORDER BY timestamp DESC) SELECT commodity, SUM(amount) AS sum_amounts FROM s GROUP BY commodity ORDER BY sum_amounts", _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                int [,] output = new int[dt.Rows.Count, 2];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        output[i, j] = Convert.ToInt32(dt.Rows[i].ItemArray[j]);
                    }
                }

                return Sanitize(output);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return new int[0, 0];
            }
        }

        // Returns an array representing the market share between two given dates
        public static int[,] getMarketShareBetweenDates(DateTime start, DateTime end)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"WITH s AS (SELECT TOP " + 10*LIMIT + " * FROM dbo.items WHERE [timestamp] BETWEEN CONVERT(datetime,'" + start + "', 105) AND CONVERT(datetime,'" + end + "', 105) ORDER BY timestamp DESC) SELECT commodity, SUM(amount) AS sum_amounts FROM s GROUP BY commodity ORDER BY sum_amounts", _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                int[,] output = new int[dt.Rows.Count, 2];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        output[i, j] = Convert.ToInt32(dt.Rows[i].ItemArray[j]);
                    }
                }
                return Sanitize(output);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return new int[0, 0];
            }
        }

        // Returns an array representing the market share of the market based on the last day
        public static int[,] GetMarketShareOfLastDay()
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"WITH s AS (SELECT TOP " + 10 * LIMIT + " * FROM dbo.items WHERE [timestamp]>=DATEADD(day,-1,GETUTCDATE()) ORDER BY timestamp DESC) SELECT commodity, SUM(amount) AS sum_amounts FROM s GROUP BY commodity ORDER BY sum_amounts", _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                int[,] output = new int[dt.Rows.Count, 2];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        output[i, j] = Convert.ToInt32(dt.Rows[i].ItemArray[j]);
                    }
                }
                return Sanitize(output);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return new int[0, 0];
            }
        }

        // Returns an array representing the market share of the market based on the last hour
        public static int[,] GetMarketShareOfLastHour()
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"WITH s AS (SELECT TOP " + 10 * LIMIT + " * FROM dbo.items WHERE [timestamp]>=DATEADD(hour,-1,GETUTCDATE()) ORDER BY timestamp DESC) SELECT commodity, SUM(amount) AS sum_amounts FROM s GROUP BY commodity ORDER BY sum_amounts", _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                int[,] output = new int[dt.Rows.Count, 2];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        output[i, j] = Convert.ToInt32(dt.Rows[i].ItemArray[j]);
                    }
                }
                return Sanitize(output);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return new int[0, 0];
            }
        }

        // Returns an array representing the market history of the user
        public static Transaction[] getOurLastHistory()
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"SELECT TOP 50 * From [dbo].[items] Where buyer = 46 OR seller = 46 ORDER BY timestamp DESC", _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                Transaction[] transactions = new Transaction[50];
                for (int i = 0; i < transactions.Length; i++)
                {
                    DateTime date = (DateTime)dt.Rows[i].ItemArray[0];
                    int commodityID = Convert.ToInt32(dt.Rows[i].ItemArray[1]);
                    int amount = Convert.ToInt32(dt.Rows[i].ItemArray[2]);
                    int price = Convert.ToInt32(dt.Rows[i].ItemArray[3]);
                    transactions[i] = new Transaction(date, commodityID, amount, price);
                }
                return transactions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return new Transaction[0];
            }
        }

        // Returns an array representing the price of a certain commodity starting on a certain date
        public static Transaction[] getPriceOfCommFromStartDate(int commodityID, DateTime start)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"SELECT [timestamp], [price] From [dbo].[items] Where @timestamp>=" + start.ToString() + "AND @commodity=" + commodityID, _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                int count = dt.Rows.Count;
                Transaction[] transactions = new Transaction[count];
                for (int i = 0; i < transactions.Length; i++)
                {
                    DateTime date = Convert.ToDateTime(dt.Rows[i].ItemArray[0]);
                    int price = Convert.ToInt32(dt.Rows[i].ItemArray[1]);
                    transactions[i] = new Transaction(date, price);
                }
                return transactions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return new Transaction[0];
            }
        }

        // Returns an array representing the price of a certain commodity based on the last N trades of it
        public static Transaction[] getPriceOfCommByLastNTrades(int commID, int n)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"SELECT TOP " + n + " [timestamp], [price] FROM [dbo].[items] WHERE [commodity] = " + commID + " ORDER BY timestamp DESC", _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                int count = dt.Rows.Count;
                Transaction[] transactions = new Transaction[count];
                for (int i = 0; i < transactions.Length; i++)
                {
                    DateTime date = Convert.ToDateTime(dt.Rows[i].ItemArray[0]);
                    int price = Convert.ToInt32(dt.Rows[i].ItemArray[1]);
                    transactions[i] = new Transaction(date, price);
                }
                return transactions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return new Transaction[0];
            }
        }

        private static int[,] Sanitize(int[,] matrix)
        {
            int nonZero = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, 1] > 0)
                    nonZero++;
            }

            int[,] toReturn = new int[nonZero, 2];

            for (int i = 0; i < nonZero; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    toReturn[i, j] = matrix[i, j];
                }
            }
            return toReturn;
        }


    }
}
