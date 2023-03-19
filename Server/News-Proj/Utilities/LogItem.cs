using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class LogItem
    {
        public LogItem() { DateTime = DateTime.Now; }
        public string Message { get; set; }
        public string Type { get; set; }
        public Exception exception { get; set; } = null;
        public DateTime DateTime { get; set; }
    }
}
