using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models
{
    public class UserFavorites
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }

    }
}
