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
using Utilities;
using News.Models;
using News.Data_SQL;

namespace News.Entities
{
    public class WallaWebsiteManager : BaseEntity
    {
        // Singleton WallaWebsiteManager class inhert from BaseEntity
        // Constructor Updating WebAddressCategories and updating NewestArticlePerCategory array
        private WallaWebsiteManager()
        {
            Logger.AddToLog(new LogItem { Message = "Data_SQL.ArticleDataSQL.GetCategoryLinks function has called", Type = "Event" });
            string SourceName = ConfigurationDataSql.GetSingleConfigurationData("Select Value from Configuration where ID = 1");
            WebAddressCategories = Data_SQL.CategoriesDataSql.GetCategoryLinks(SourceName);
            Logger.AddToLog(new LogItem { Message = "CheckNewestArticleFromDB function has called", Type = "Event" });
            NewestArticlePerCategory = CheckNewestArticleFromDB(SourceName);
        }

        private static WallaWebsiteManager _Instance = new WallaWebsiteManager();

        public static WallaWebsiteManager Instance
        {
            get { return _Instance; }
            set { _Instance = value; }
        }

        // Initiallizing WallaWebsiteManager and calling FetchAndSaveData function
        public void Init()
        {
            Logger.AddToLog(new LogItem { Message = "WallaWebManager has initiallized and calling FetchAndSaveData function" }); 
            FetchAndSaveData();
        }
    }
}
