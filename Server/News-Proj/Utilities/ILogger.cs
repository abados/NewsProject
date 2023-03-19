namespace Utilities
{
    public interface MyILogger
    {
        void Init();
        void LogEvent(LogItem item);
        void LogError(LogItem item);
        void LogException(LogItem item);
        void LogCheckHouseKeeping();
    }
  
}