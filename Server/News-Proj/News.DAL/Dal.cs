using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using News.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace News.DAL
{

    // Class to get connection string (Sql)
    public static class ConfigDB
    {


		public static string GetCString()
        {
            string ConnectionString;


			try
            {
				Logger.AddToLog(new LogItem { Message = "Getting connection string", Type = "Event" });
				//var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
				//return config.GetConnectionString("MY_CS"); // enter hard codded for testing
				var config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                                                             .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true).Build();

                ConnectionString = config.GetConnectionString("MY_CS");
                if(ConnectionString==null)
                {
                    ConnectionString = "Integrated Security = SSPI; Persist Security Info = False; Initial Catalog = News; Data Source = DESKTOP-183SD6J\\SQLEXPRESS01";

				}

				return ConnectionString; // enter hard codded for testing

			}
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
			
		}
    }

    // DAL class
    public static class  Dal
    {
        // Delegate - Function pointer
        public delegate object SetDataReader_delegate(SqlDataReader reader);
        public static string ConnectionStringTest = ConfigDB.GetCString();

        public static void SetConnectionString(string connectionString)
        {
            ConnectionStringTest = connectionString;
        }
        // Getting data from Sql
        public static object GetDataFromDB(string SqlQuery, SetDataReader_delegate func)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Getting data from Sql", Type = "Event" });
                object Data = null;
                using (SqlConnection connection = new SqlConnection(ConnectionStringTest))
                {

                    // Adapter
                    using (SqlCommand command = new SqlCommand(SqlQuery, connection))
                    {
                        connection.Open();
                        //Reader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Logger.AddToLog(new LogItem { Message = "Getting Data From DB", Type = "Event" });

                            Data = func(reader);
                        }

                    }
                }
                return Data;
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }          
        }

        // Saving articles to Sql
        public static void SaveArticlesToDB(string SqlQuery, List<Article> articles)
        {
            int index;
            try
            {
                
                Logger.AddToLog(new LogItem { Message = "Saving articles to Sql", Type = "Event" });
                using (SqlConnection connection = new SqlConnection(ConnectionStringTest))
                {
                    // Adapter
                    using (SqlCommand command = new SqlCommand(SqlQuery, connection))
                    {
                        
                        connection.Open();
                        //Reader
                        for (index = articles.Count - 1; index >= 0; index--)
                        {
                            command.Parameters.AddWithValue("@Title", articles[index].Title);
                            command.Parameters.AddWithValue("@Description", articles[index].Description);
                            command.Parameters.AddWithValue("@Image", articles[index].Image);
                            command.Parameters.AddWithValue("@Link", articles[index].Link);
                            command.Parameters.AddWithValue("@Source", articles[index].Source);
                            command.Parameters.AddWithValue("@CategoryID", articles[index].CategoryID);
                            command.Parameters.AddWithValue("@NumberOfClicks", articles[index].TimesClicked);
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }

        // Runs Command with single value result
        public static object RunCommandResultSingleValue(string SqlQuery)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Runs Command with single value result", Type = "Event" });
                object result;          
                using (SqlConnection connection = new SqlConnection(ConnectionStringTest))
                {
                    // Adapter
                    using (SqlCommand command = new SqlCommand(SqlQuery, connection))
                    {
                        connection.Open();
                        result = command.ExecuteScalar();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }


        // Runs Command with multi values result
        public static string[] RunCommandResultMultiValues(string SqlQuery)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Runs Command with single value result", Type = "Event" });
                List<string> MultipleStrings = new List<string>();
                using (SqlConnection connection = new SqlConnection(ConnectionStringTest))
                {
                   
                    // Adapter
                    using (SqlCommand command = new SqlCommand(SqlQuery, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Logger.AddToLog(new LogItem { Message = "Getting Data From DB", Type = "Event" });
                            while(reader.Read())
                            {
                                MultipleStrings.Add(reader.GetString(0));
                            }                            
                        }
                    }
                }
                return MultipleStrings.ToArray();
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }

        // Runs command for check
        public static bool RunCommandCheck(string sqlQuery, string AuthID)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Runs command for check", Type = "Event" });
                using (SqlConnection connection = new SqlConnection(ConnectionStringTest))
                {
                    bool answer;
                    string queryString = sqlQuery;

                    // Adapter
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@AuthID", AuthID);
                        answer = (bool)command.ExecuteScalar();
                    }
                    return answer;
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                return false;
            }
        }

        // Runs command to update
        public static bool RunCommandUpdate(string SqlQuery)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Runs command to update", Type = "Event" });
                using (SqlConnection connection = new SqlConnection(ConnectionStringTest))
                {
                    connection.Open();
                    using(SqlCommand command = new SqlCommand(SqlQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    } 
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                return false;
            }
        }

        // Runs command non query
        public static void RunNonQuery(string sqlQuery)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Runs command to update", Type = "Event" });
                using (SqlConnection connection = new SqlConnection(ConnectionStringTest))
                {

                    string queryString = sqlQuery;

                    // Adapter
                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        connection.Open();
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            LogItem item = new LogItem { exception = ex, Message = ex.Message, Type = "Exception" };
                            Logger.myQueue.Enqueue(item);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }

    }
}
