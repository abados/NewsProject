using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Utilities
{
    public class LogDB : MyILogger
    {
        static string ConnectionString = Environment.GetEnvironmentVariable("ConnectionString");
        public void Init()
        {
            Task.Run(() =>
             {
                 while (true)
                 {
                     if (Logger.myQueue.Count > 0)
                     {
                         LogItem item = Logger.myQueue.Dequeue();
                         if (item.Type == "Error")
                         {
                             LogError(item);
                         }
                         else if (item.Type == "Event")
                         {
                             LogEvent(item);
                         }
                         else if (item.Type == "Exception")
                         {
                             LogException(item);
                         }
                         System.Threading.Thread.Sleep(60000 * 10); // when queue is empty, waits 10 minutes
                     }
                 }
             });

            Task.Run(() =>
            {
                while (true)
                {
                    LogCheckHouseKeeping();
                    System.Threading.Thread.Sleep(60000 * 10); // when queue is empty, waits 10 minutes
                }
            });
        }
        public void AddToDB(LogItem item)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                item.DateTime = DateTime.Now;
                string queryString;
                if (item.exception != null)
                {
                    queryString = $"insert into Log (Stack_Trace, Message, Type, Date) values (@stackTrace,'@message','@type', @dateTime)";
                }
                else
                {
                    queryString = $"insert into Log (Type ,Message, Type, Date) values ( '@type','@message',@dateTime)";
                }

                // Adapter
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    if (item.exception != null)
                    {
                        command.Parameters.AddWithValue("@message", item.exception.Message);
                        command.Parameters.AddWithValue("@stackTrace", item.exception.StackTrace);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@message", item.Message);
                    }
                    command.Parameters.AddWithValue("@dateTime", item.DateTime);
                    command.Parameters.AddWithValue("@type", item.Type);
                    //Reader
                    command.ExecuteNonQuery();
                }
            }
        }

        public void LogCheckHouseKeeping()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                string queryString = "Delete from log where Date < DATEADD(month, -3, GETDATE())";

                // Adapter
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    //Reader
                    command.ExecuteNonQuery();
                }
            }
        }

        public void LogError(LogItem item)
        {
            AddToDB(item);
        }

        public void LogEvent(LogItem item)
        {
            AddToDB(item);
        }

        public void LogException(LogItem item)
        {
            AddToDB(item);
        }
    }
}
