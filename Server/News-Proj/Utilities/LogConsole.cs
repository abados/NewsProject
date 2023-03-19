using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class LogConsole : MyILogger
    {
      
        public void PrintMessage(LogItem item)
        {
            item.DateTime = DateTime.Now;
            if (item.exception == null)
            {
                Console.WriteLine($"{item.DateTime.Day}/{item.DateTime.Month} - {item.DateTime.Hour}:{item.DateTime.Minute} - {item.Message}");
            }
            else
            {
                Console.WriteLine($"{item.DateTime.Day}/{item.DateTime.Month} - {item.DateTime.Hour} : {item.DateTime.Minute}  -  {item.exception.Message}");
            }
        }
        public void Init()
        {
           
        }

        public void LogCheckHouseKeeping()
        {
           
        }

        public void LogError(LogItem item)
        {
            PrintMessage(item);
        }

        public void LogEvent(LogItem item)
        {
            PrintMessage(item);
        }

        public void LogException(LogItem item)
        {
            PrintMessage(item);
        }
    }
}
