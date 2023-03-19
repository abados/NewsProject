using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class LogFile : MyILogger
    {
        private readonly long File_Max_Size = 1024 * 1024; // 1 mega bytes

		private static string File_Name = Path.GetFullPath(Configuration.RunCommandResultSingleValue("Select Value from Configuration where Name = 'LogFileName'").ToString());

		private readonly string dirPath = Path.GetDirectoryName('@' + File_Name);

		private readonly string Original_File_Name = Path.GetFileNameWithoutExtension(File_Name);

        private readonly string Ending = Path.GetExtension(File_Name);
        private int fileNameCounter = 0;
        private readonly object obj = new object();

        public void Init()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (Logger.myQueue.Count > 0)
                    {
                        LogItem item = Logger.myQueue.Dequeue();
                        AddToFile(item);
                        System.Threading.Thread.Sleep(1000); // writes to log each 1 sec
                    }
                }
            });
            Task.Run(() =>
            {
                while (true)
                {
                    LogCheckHouseKeeping();
                    System.Threading.Thread.Sleep(60000 * 10); // checks if file is at max size each 10 minutes
                }
            });

        }
        public void AddToFile(LogItem item)
        {
            string hour, minute, seconds;
            lock (obj)
            {
                if (item != null)
                {


                    using (StreamWriter sw = new StreamWriter(File_Name, true))
                    {

                        if (item.DateTime.Hour < 10)
                        {
                            hour = "0" + item.DateTime.Hour;
                        }
                        else
                        {
                            hour = item.DateTime.Hour.ToString();
                        }
                        if (item.DateTime.Minute < 10)
                        {
                            minute = "0" + item.DateTime.Minute;
                        }
                        else
                        {
                            minute = item.DateTime.Minute.ToString();
                        }
                        if (item.DateTime.Second < 10)
                        {
                            seconds = "0" + item.DateTime.Second;
                        }
                        else
                        {
                            seconds = item.DateTime.Second.ToString();
                        }
                        if (item.exception == null)
                        {
                            sw.WriteLine($"{item.DateTime.Day}/{item.DateTime.Month}/{item.DateTime.Year} - {hour}:{minute}:{seconds} - {item.Message}");
                        }
                        else
                        {
                            sw.WriteLine($"{item.DateTime.Day} / {item.DateTime.Month} / {item.DateTime.Year}  -  {hour} : {minute}:{seconds}  -  {item.exception.Message} : {item.exception.StackTrace}");
                        }
                    }
                }
            }
        }
        public void LogCheckHouseKeeping()
        {
            FileInfo fileInfo = new FileInfo(Original_File_Name + Ending);
            while (fileInfo.Exists)
            {
                if (fileInfo.Length >= File_Max_Size)
                {
                    fileNameCounter++;
                    File_Name = dirPath + Original_File_Name + fileNameCounter.ToString() + Ending;
                    fileInfo = new FileInfo(File_Name);
                }
                else
                {
                    return;
                }
            }
        }

        public void LogError(LogItem item)
        {
            item.Type = "Error";
            AddToFile(item);
        }

        public void LogEvent(LogItem item)
        {
            item.Type = "Event";
            AddToFile(item);
        }

        public void LogException(LogItem item)
        {
            item.Type = "Exception";
            AddToFile(item);
        }
    }
}
