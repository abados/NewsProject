using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
	public class YT_Video
	{
		public int ID { get; set; }
		public int ArticleID { get; set; }
		public string ArticleTitle { get; set; }
		public string[] Links { get; set; }
	}
}
