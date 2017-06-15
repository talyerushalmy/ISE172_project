using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer
{
    public static class DatabaseSocket
    {
        private static string _connectionString = @"Data Source=ise172.ise.bgu.ac.il;Initial Catalog=history;Persist Security Info=True;User ID=labuser;Password=wonsawheightfly";
        private static SqlConnection _myConnection = new SqlConnection(_connectionString);


        public static DataTable getDealsFromDate(DateTime start)
        {
            try
            {
                _myConnection.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            DataTable dt = new DataTable();
            SqlCommand command = new SqlCommand(@"SELECT *
            From [dbo].[items] 
            Where @Timestamp>="+start.ToString());
            dt.Load(command.ExecuteReader());
            _myConnection.Close();
            return dt;
        }
        public static DataTable getLastDealsByAmount()
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
            SqlCommand command = new SqlCommand(@"SELECT TOP 30 @Price @Amount @Commodity
            From [dbo].[items]") ;
            dt.Load(command.ExecuteReader());
            _myConnection.Close();
            return dt;
        }
        public static DataTable getOurLastHistory()
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
            SqlCommand command = new SqlCommand(@"SELECT TOP 50*
            From [dbo].[items] Where @buyer="+"46"+" OR @seller="+46);
            dt.Load(command.ExecuteReader());
            _myConnection.Close();
            return dt;
        }
        public static DataTable getPriceOfCommodityPerPeriod(int commodityID, DateTime start)
        {
            try
            {
                _myConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            DataTable output = new DataTable();
            SqlCommand command = new SqlCommand(@"SELECT @timestamp,@price 
            From [dbo].[items] Where @timestamp>=" + start.ToString() + "AND @commodity=" + commodityID);
            output.Load(command.ExecuteReader());
            _myConnection.Close();
            return output;
        }
        

    }
}
