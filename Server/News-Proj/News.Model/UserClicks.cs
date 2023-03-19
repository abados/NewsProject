using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    public class UserClicks
    {
        public int ID { get; set; }
        public int ArticleID { get; set; }
        public int UserID { get; set; }
        public int TimesClicked { get; set; } = 0;
    }
}
