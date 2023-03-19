using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace Utilities
{
    public class Logger
    {
        static MyILogger myLog;
        public static Queue<LogItem> myQueue = new Queue<LogItem>();

        private static Logger _LoggerInstance = new Logger();

        public static Logger LoggerInstance
        {
            get { return _LoggerInstance; }
        }


        public Logger()
        {

            switch (GetLogProvider())
            {
                case "File":
                    myLog = new LogFile();
                    myLog.Init();
                    break;
                case "DB":
                    myLog = new LogDB();
                    myLog.Init();
                    break;
                case "Console":
                    myLog = new LogConsole();
                    myLog.Init();
                    break;
                default:
                    myLog = new LogNone();
                    myLog.Init();
                    break;
            }
        }


        public static void AddToLog(LogItem item)
        {
            try { 
            if (item != null)
            {
                myQueue.Enqueue(item);
            }
			}
            catch(Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }

		}


        private string GetLogProvider()
        {
            return Configuration.RunCommandResultSingleValue("Select Value from Configuration where Name = 'LogProvider'").ToString();
        }
    }
}
