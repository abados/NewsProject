using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    public class Article
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string Source { get; set; }
        public int CategoryID { get; set; }
        public int TimesClicked { get; set; } = 0;
    }
}
