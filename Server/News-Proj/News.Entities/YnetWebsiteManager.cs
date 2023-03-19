using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Threading;
using System.Text.RegularExpressions;
using News.Models;
using Utilities;
using News.Data_SQL;

namespace News.Entities
{
    public class YnetWebsiteManager : BaseEntity
    {
        // Singleton YnetWebsiteManager class inhert from BaseEntity
        // Constructor Updating WebAddressCategories and updating NewestArticlePerCategory array
        private YnetWebsiteManager()
        {
            Logger.AddToLog(new LogItem { Message = "Data_SQL.ArticleDataSQL.GetCategoryLinks function has called", Type = "Event" });
            string SourceName = ConfigurationDataSql.GetSingleConfigurationData("Select Value from Configuration where ID = 4");
            WebAddressCategories = Data_SQL.CategoriesDataSql.GetCategoryLinks(SourceName);
            Logger.AddToLog(new LogItem { Message = "CheckNewestArticleFromDB function has called", Type = "Event" });
            NewestArticlePerCategory = CheckNewestArticleFromDB(SourceName);
        }
        private static YnetWebsiteManager _Instance = new YnetWebsiteManager();

        public static YnetWebsiteManager Instance
        {
            get { return _Instance; }
            set { _Instance = value; }
        }

        // Initiallizing YnetWebsiteManager and calling FetchAndSaveData function
        public void Init()
        {
            Logger.AddToLog(new LogItem { Message = "YnetWebsiteManager has initiallized and calling FetchAndSaveData function" });
            FetchAndSaveData();
        }
    }
}