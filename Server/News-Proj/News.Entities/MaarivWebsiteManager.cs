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
    public class MaarivWebsiteManager : BaseEntity, IWebsite 
    {
        // Singleton MaarivWebsiteManager class inhert from BaseEntity
        // Constructor Updating WebAddressCategories and updating NewestArticlePerCategory array
        private MaarivWebsiteManager()
        {

            Logger.AddToLog(new LogItem { Message = "Data_SQL.ArticleDataSQL.GetCategoryLinks function has called", Type = "Event" });
            string SourceName = ConfigurationDataSql.GetSingleConfigurationData("Select Value from Configuration where ID = 2");
            WebAddressCategories = Data_SQL.CategoriesDataSql.GetCategoryLinks(SourceName);
            Logger.AddToLog(new LogItem { Message = "CheckNewestArticleFromDB function has called", Type = "Event" });
            NewestArticlePerCategory = CheckNewestArticleFromDB(SourceName);
        }
        private static MaarivWebsiteManager _Instance = new MaarivWebsiteManager();

        public static MaarivWebsiteManager Instance
        {
            get { return _Instance; }
            set { _Instance = value; }
        }

        // Initiallizing MaarivWebsiteManager and calling FetchAndSaveData function
        public void Init()
        {
            Logger.AddToLog(new LogItem { Message = "MaarivWebsiteManager has initiallized and calling FetchAndSaveData function" });
            FetchAndSaveData();
        }
    }
}
