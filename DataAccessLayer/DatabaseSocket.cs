﻿using System;
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
        private static string _connectionString = @"Data Source=ise172.ise.bgu.ac.il;Initial Catalog=history;Persist Security Info=True;User ID=labuser;Password=wonsawheightfly";
        private static SqlConnection _myConnection = new SqlConnection(_connectionString);
        private const int LIMIT = 100000;

        // Returns a table of all the history of the market
        public static DataTable GetAllHistory(bool justOurs)
        {
            DataTable history = new DataTable();
            try
            {
                _myConnection.Open();
                SqlCommand command = new SqlCommand("SELECT TOP " + LIMIT + " * FROM [dbo].[items] ORDER BY timestamp DESC", _myConnection);
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

        // Returns a table of the market history of the last N trades 
        public static DataTable GetHistoryOfLastNTrades(int n, bool justOurs)
        {
            DataTable history = new DataTable();
            try
            {
                if (n < 0 || n > LIMIT)
                {
                    throw new ArgumentException("n is out of range");
                }
                _myConnection.Open();
                string query = "SELECT TOP " + n + " * FROM [dbo].[items]";
                if (justOurs)
                    query += " WHERE buyer='user46' OR seller='user46'";
                query += " ORDER BY timestamp DESC";
                SqlCommand command = new SqlCommand(query, _myConnection);
                history.Load(command.ExecuteReader());
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

        // Creates a table of all the market history between two dates
        public static DataTable GetHistoryBetweenTwoDates(DateTime start, DateTime end, bool justOurs)
        {
            DataTable history = new DataTable();
            try
            {
                _myConnection.Open();
                string query = "SELECT TOP " + LIMIT + " timestamp, price FROM[dbo].[items] WHERE[timestamp] BETWEEN CONVERT(datetime,'" + start + "', 105) AND CONVERT(datetime,'" + end + "', 105)";
                if (justOurs)
                    query += " WHERE buyer='user46' OR seller='user46'";
                query += " ORDER BY timestamp DESC";
                Console.WriteLine(start);
                SqlCommand command = new SqlCommand(query, _myConnection);
                history.Load(command.ExecuteReader());
                Console.WriteLine("Last timestamp : " + history.Rows[history.Rows.Count - 1].ItemArray[0]);
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
                _myConnection.Open();
                string query = "SELECT TOP " + LIMIT + " * FROM [dbo].[items] WHERE [timestamp]>=DATEADD(day,-1,GETUTCDATE())";
                if (justOurs)
                    query += " AND buyer='user46' OR seller='user46'";
                query += " ORDER BY timestamp DESC";
                SqlCommand command = new SqlCommand(query, _myConnection);
                historyOfToday.Load(command.ExecuteReader());
                //Console.WriteLine("Last timestamp :" + Convert.ToDateTime(historyOfToday.Rows[historyOfToday.Rows.Count - 1].ItemArray[0]));
                //Console.WriteLine("First timestamp :" + (historyOfToday.Rows[0].ItemArray[0]));
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
                string query = "SELECT TOP " + LIMIT + " * FROM [dbo].[items] WHERE [timestamp]>=DATEADD(hour,-1,GETUTCDATE())";
                if (justOurs)
                    query += " WHERE buyer='user46' OR seller='user46'";
                query += " ORDER BY timestamp DESC";
                SqlCommand command = new SqlCommand(query, _myConnection);
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
                if (n < 0 || n > LIMIT)
                    throw new ArgumentException("n is above the limit");
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"WITH s AS (SELECT TOP " + n + " * FROM dbo.items ORDER BY timestamp DESC) SELECT commodity, SUM(amount) AS sum_amounts FROM s GROUP BY commodity ORDER BY sum_amounts", _myConnection);
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

        // Returns an array representing the market share between two given dates
        public static int[,] getMarketShareBetweenDates(DateTime start, DateTime end)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"WITH s AS (SELECT TOP " + 10 * LIMIT + " * FROM dbo.items WHERE [timestamp] BETWEEN CONVERT(datetime,'" + start.ToString("dd/M/yyyy") + "', 105) AND CONVERT(datetime,'" + end.ToString("dd/M/yyyy") + "', 105) ORDER BY timestamp DESC) SELECT commodity, SUM(amount) AS sum_amounts FROM s GROUP BY commodity ORDER BY sum_amounts", _myConnection);
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

        public static int[,] GetMarketShareOfLastWeek()
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"WITH s AS (SELECT TOP " + 10 * LIMIT + " * FROM dbo.items WHERE [timestamp]>=DATEADD(day,-7,GETUTCDATE()) ORDER BY timestamp DESC) SELECT commodity, SUM(amount) AS sum_amounts FROM s GROUP BY commodity ORDER BY sum_amounts", _myConnection);
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

        public static Transaction[] getPriceOfCommBetweenDates(int commodityID, DateTime start, DateTime end)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"SELECT dateadd(hour, case when datepart(minute,timestamp) < 30 then 0 else 1 end +  datepart(hour, timestamp), cast(convert(varchar(10),timestamp, 112) as datetime)) as formatted_time, AVG(price) FROM [dbo].[items] WHERE commodity=" + commodityID + " AND [timestamp] BETWEEN CONVERT(datetime,'" + start.ToString("dd/M/yyyy") + "', 105) AND CONVERT(datetime,'" + end.ToString("dd/M/yyyy") + "', 105) GROUP BY dateadd(hour, case when datepart(minute,timestamp) < 30 then 0 else 1 end +  datepart(hour, timestamp), cast(convert(varchar(10),timestamp, 112) as datetime)) ORDER BY dateadd(hour, case when datepart(minute,timestamp) < 30 then 0 else 1 end +  datepart(hour, timestamp), cast(convert(varchar(10),timestamp, 112) as datetime)) DESC", _myConnection);
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

        public static Transaction[] getPriceOfCommByLastHour(int commodityID)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"SELECT dateadd(hour, case when datepart(minute,timestamp) < 30 then 0 else 1 end +  datepart(hour, timestamp), cast(convert(varchar(10),timestamp, 112) as datetime)) as formatted_time, AVG(price) FROM [dbo].[items] WHERE [timestamp] >= DATEADD(hour, -1, GETUTCDATE()) AND commodity=" + commodityID + " GROUP BY dateadd(hour, case when datepart(minute,timestamp) < 30 then 0 else 1 end +  datepart(hour, timestamp), cast(convert(varchar(10),timestamp, 112) as datetime))", _myConnection);
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

        public static Transaction[] getPriceOfCommByLastDay(int commodityID)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand(@"SELECT dateadd(hour, case when datepart(minute,timestamp) < 30 then 0 else 1 end +  datepart(hour, timestamp), cast(convert(varchar(10),timestamp, 112) as datetime)) as formatted_time, AVG(price) FROM [dbo].[items] WHERE [timestamp] >= DATEADD(DAY, -1, GETUTCDATE()) AND commodity=" + commodityID + " GROUP BY dateadd(hour, case when datepart(minute,timestamp) < 30 then 0 else 1 end +  datepart(hour, timestamp), cast(convert(varchar(10),timestamp, 112) as datetime))", _myConnection);
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
                SqlCommand command = new SqlCommand(@"SELECT TOP " + n + " dateadd(hour, case when datepart(minute,timestamp) < 30 then 0 else 1 end +  datepart(hour, timestamp), cast(convert(varchar(10),timestamp, 112) as datetime)) as formatted_time, AVG(price) FROM [dbo].[items] WHERE commodity=" + commID + " GROUP BY dateadd(hour, case when datepart(minute,timestamp) < 30 then 0 else 1 end +  datepart(hour, timestamp), cast(convert(varchar(10),timestamp, 112) as datetime))", _myConnection);
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

        public static List<String> GetHighlightsBiggestPrice(string TIME_WINDOW)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                string query = @"SELECT TOP 1 price, commodity, timestamp FROM [dbo].[items] ";
                switch (TIME_WINDOW)
                {
                    case "DAY":
                        query += @"WHERE[timestamp] >= DATEADD(day, -1, GETUTCDATE())";
                        break;
                    case "WEEK":
                        query += @"WHERE [timestamp]>=DATEADD(day,-7,GETUTCDATE())";
                        break;
                    case "HOUR":
                        query += @"WHERE [timestamp]>=DATEADD(hour,-1,GETUTCDATE())";
                        break;
                }
                query += " ORDER BY price DESC";
                SqlCommand command = new SqlCommand(query, _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                List<String> output = new List<string>();
                for (int i = 0; i < 3; i++)
                {
                    output.Add(Convert.ToString(dt.Rows[0].ItemArray[i]));
                }
                return output;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return null;
            }
        }

        public static List<String> GetHighlightsMostSoldComm(string TIME_WINDOW)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                string query = @"SELECT TOP 1 commodity, SUM(amount) FROM [dbo].[items] ";
                switch (TIME_WINDOW)
                {
                    case "DAY":
                        query += @"WHERE[timestamp] >= DATEADD(day, -1, GETUTCDATE())";
                        break;
                    case "WEEK":
                        query += @"WHERE [timestamp]>=DATEADD(day,-7,GETUTCDATE())";
                        break;
                    case "HOUR":
                        query += @"WHERE [timestamp]>=DATEADD(hour,-1,GETUTCDATE())";
                        break;
                }
                query += " GROUP BY commodity ORDER BY SUM(amount) DESC";
                SqlCommand command = new SqlCommand(query, _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                List<String> output = new List<string>();
                for (int i = 0; i < 2; i++)
                {
                    output.Add(Convert.ToString(dt.Rows[0].ItemArray[i]));
                }
                return output;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return new List<String>();
            }
        }

        public static List<String> GetHighlightsLeastSoldComm(string TIME_WINDOW)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                string query = @"SELECT TOP 1 commodity, SUM(amount) FROM [dbo].[items] ";
                switch (TIME_WINDOW)
                {
                    case "DAY":
                        query += @"WHERE[timestamp] >= DATEADD(day, -1, GETUTCDATE())";
                        break;
                    case "WEEK":
                        query += @"WHERE [timestamp]>=DATEADD(day,-7,GETUTCDATE())";
                        break;
                    case "HOUR":
                        query += @"WHERE [timestamp]>=DATEADD(hour,-1,GETUTCDATE())";
                        break;
                }
                query += " GROUP BY commodity ORDER BY SUM(amount)";
                SqlCommand command = new SqlCommand(query, _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                List<String> output = new List<string>();
                for (int i = 0; i < 2; i++)
                {
                    output.Add(Convert.ToString(dt.Rows[0].ItemArray[i]));
                }
                return output;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return new List<String>();
            }
        }

        public static List<String> GetHighlightsBestSale(string TIME_WINDOW)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                string query = @"SELECT TOP 1 (amount * price), amount, price, commodity FROM [dbo].[items] ";
                switch (TIME_WINDOW)
                {
                    case "DAY":
                        query += @"WHERE[timestamp] >= DATEADD(day, -1, GETUTCDATE())";
                        break;
                    case "WEEK":
                        query += @"WHERE [timestamp]>=DATEADD(day,-7,GETUTCDATE())";
                        break;
                    case "HOUR":
                        query += @"WHERE [timestamp]>=DATEADD(hour,-1,GETUTCDATE())";
                        break;
                }
                query += " ORDER BY (amount * price) DESC";
                SqlCommand command = new SqlCommand(query, _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                List<String> output = new List<string>();
                for (int i = 0; i < 4; i++)
                {
                    output.Add(Convert.ToString(dt.Rows[0].ItemArray[i]));
                }
                return output;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return new List<String>();
            }
        }

        public static List<String> GetHighlightsWorstSale(string TIME_WINDOW)
        {
            try
            {
                _myConnection.Open();
                DataTable dt = new DataTable();
                string query = @"SELECT TOP 1 (amount * price), amount, price, commodity FROM [dbo].[items] ";
                switch (TIME_WINDOW)
                {
                    case "DAY":
                        query += @"WHERE[timestamp] >= DATEADD(day, -1, GETUTCDATE())";
                        break;
                    case "WEEK":
                        query += @"WHERE [timestamp]>=DATEADD(day,-7,GETUTCDATE())";
                        break;
                    case "HOUR":
                        query += @"WHERE [timestamp]>=DATEADD(hour,-1,GETUTCDATE())";
                        break;
                }
                query += " ORDER BY (amount * price)";
                SqlCommand command = new SqlCommand(query, _myConnection);
                dt.Load(command.ExecuteReader());
                _myConnection.Close();
                List<String> output = new List<string>();
                for (int i = 0; i < 4; i++)
                {
                    output.Add(Convert.ToString(dt.Rows[0].ItemArray[i]));
                }
                return output;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (_myConnection.State == ConnectionState.Open)
                    _myConnection.Close();
                return new List<String>();
            }
        }

    }
}
