using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace News.Entities
{
    public class CommonWebManager
    {
        // Singleton CommonWebManager class
        private static readonly CommonWebManager _Instance = new CommonWebManager();
        public static CommonWebManager Instance
        {
            get { return _Instance; }
        }
        private CommonWebManager() { }


        // Getting ten top articles (most clicked)
        public Dictionary<string, List<Article>> GetTopArticles(string AuthID)
        {
            Logger.AddToLog(new LogItem { Message = "GetTopArticles function has been called", Type = "Event" });
            return Data_SQL.ArticleDataSQL.GetTopArticles(AuthID);
        }

        // Checking if user is registered, if not register him
        public bool CheckIfRegistered(string AuthID)
        {
            Logger.AddToLog(new LogItem { Message = "CheckIfRegistered function has been called", Type = "Event" });
            return Data_SQL.UserDataSql.CheckIfRegistered(AuthID);
        }

        // Getting all categories and also user's favorite
        public Dictionary<string, List<Category>> GetCategories(string AuthID)
        {
            Logger.AddToLog(new LogItem { Message = "GetCategories function has been called", Type = "Event" });
            return Data_SQL.CategoriesDataSql.GetCategories(AuthID);
        }

        // Updating user's favorite categories
        public bool UpdateFavoriteCategories(string AuthID, int[] CategoriesID)
        {
            Logger.AddToLog(new LogItem { Message = "UpdateFavoriteCategories function has been called", Type = "Event" });
            return Data_SQL.CategoriesDataSql.UpdateFavoriteCategories(AuthID, CategoriesID);
        }

        // Getting 10 articles per Category from each source (Max 120 - 10 * 3 * 4)
        public Dictionary<string, List<Article>> GetPopularArticles(string AuthID)
        {
            Logger.AddToLog(new LogItem { Message = "GetPopularArticles function has been called", Type = "Event" });
            return Data_SQL.ArticleDataSQL.GetPopularArticles(AuthID);
        }

        // Getting 10 articles that no one has clicked yet
        public List<Article> GetCuriousArticles(string AuthID)
        {
            Logger.AddToLog(new LogItem { Message = "GetCuriousArticles function has been called", Type = "Event" });
            return Data_SQL.ArticleDataSQL.GetCuriousArticles(AuthID);
        }

        // Updating number of clicks each time a user clicks on an article (only one click counts per user)
        // Updating Article table for each user that clicked and UserClicks table for his own clicks
        public void UpdateNumberOfClicks(string AuthID, int ArticleID)
        {
            Logger.AddToLog(new LogItem { Message = "UpdateNumberOfClicks function has been called", Type = "Event" });
            Data_SQL.UserDataSql.UpdateNumberOfClicks(AuthID, ArticleID);
        }
    }
}
