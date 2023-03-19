using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Configuration
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
				if (ConnectionString == null)
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
	

	// Runs Command with single value result
	public static object RunCommandResultSingleValue(string SqlQuery)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Runs Command with single value result", Type = "Event" });
                object result;
                using (SqlConnection connection = new SqlConnection(GetCString()))
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
    }
}
