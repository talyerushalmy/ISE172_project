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
        private static string _connectionString = @"Data Source=ise172.ise.bgu.ac.il;Initial Catalog=history;Persist Security Info=True;User ID=labuser;Password=wonsawheightfly";
        private static SqlConnection _myConnection = new SqlConnection(_connectionString);

        public static int[,] marketShare(int x, int numOfComms)
        {
            try
            {
                _myConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            DataTable dt = new DataTable();
            SqlCommand command = new SqlCommand(@"WITH s AS (SELECT TOP 1000 * FROM dbo.items ORDER BY timestamp DESC) SELECT commodity, SUM(amount) AS sum_amounts FROM s GROUP BY commodity ORDER BY sum_amounts DESC", _myConnection);
            dt.Load(command.ExecuteReader());
            _myConnection.Close();

            int[,] output = new int[numOfComms, 2];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    output[i, j] = Convert.ToInt32(dt.Rows[i].ItemArray[j]);
                }
            }

            return Sanitize(output);
        }
        public static Transaction[] getOurLastHistory()
        {
            try
            {
                _myConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
        public static Transaction[] getPriceOfCommodityFromStartDate(int commodityID, DateTime start)
        {
            try
            {
                _myConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            DataTable dt = new DataTable();
            SqlCommand command = new SqlCommand(@"SELECT @timestamp,@price 
            From [dbo].[items] Where @timestamp>=" + start.ToString() + "AND @commodity=" + commodityID);
            dt.Load(command.ExecuteReader());
            _myConnection.Close();
            int count = dt.Rows.Count;
            Transaction[] transactions = new Transaction[count];
            for (int i = 0; i < transactions.Length; i++)
            {
                DateTime date = (DateTime)dt.Rows[i].ItemArray[0];
                int price = (int)dt.Rows[i].ItemArray[1];
                transactions[i] = new Transaction(date, price);
            }
            return transactions;
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
