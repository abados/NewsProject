using News.DAL;
using News.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace News.Data_SQL
{
    public class CategoriesDataSql : BaseDataSQL
    {
        // CategoriesDataSql class inhert from BaseDataSQL


        // Getting category links from Sql
        public static Dictionary<int, string> GetCategoryLinks(string Source)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Getting category links from Sql", Type = "Event" });
                string SqlQuery = $"select * from CategoriesLink where Source = '{Source}'";
                return (Dictionary<int, string>)Dal.GetDataFromDB(SqlQuery, GetCategoryLinksFromDB);
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }

        // Getting number of categories from Sql
        public static int GetNumberOfCategories()
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Getting number of categories from Sql", Type = "Event" });
                string SqlQuery = "select count (*) from Categories";
                return (int)Dal.RunCommandResultSingleValue(SqlQuery);
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }

        // Getting categories from Sql (All categories and user's favorite)
        public static Dictionary<string, List<Category>> GetCategories(string AuthID)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Getting categories from Sql (All categories and user's favorite)", Type = "Event" });
                string SqlQuery = $"exec GetCategories @AuthID = '{AuthID}'";
                return (Dictionary<string, List<Category>>)Dal.GetDataFromDB(SqlQuery, _GetCategories);
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }

        // Updating user's favorite categories
        public static bool UpdateFavoriteCategories(string AuthID, int[] CategoriesID)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Updating user's favorite categories in Sql", Type = "Event" });
                string categoryIDsString = string.Join(",", CategoriesID.Select(id => id.ToString()));

                // Construct the SQL query
                string SqlQuery = $@"DECLARE @authID nvarchar(50) = '{AuthID}';
                                   BEGIN TRANSACTION;
                                   DELETE FROM UserFavorites
                                   WHERE UserID = (SELECT ID FROM Users WHERE AuthID = @authID);
                                   INSERT INTO UserFavorites (UserID, CategoryID)
                                   SELECT u.ID, c.value
                                   FROM Users u
                                   CROSS JOIN (VALUES {string.Join(",", CategoriesID.Select(id => $"(CAST({id} AS int))"))}) c(value)
                                   WHERE u.AuthID = @authID;
                                   COMMIT TRANSACTION;";
                return Dal.RunCommandUpdate(SqlQuery);
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                return false;
            }
        }

        // Getting Category links from Sql, called by using Delegate
        public static Dictionary<int, string> GetCategoryLinksFromDB(SqlDataReader reader)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Getting Category links from Sql", Type = "Event" });
                Dictionary<int, string> webCategoryLinks = new Dictionary<int, string>();
                while (reader.Read())
                {
                    string categoryLink = reader.GetString(1);
                    int categoryId = reader.GetInt32(2);
                    webCategoryLinks.Add(categoryId, categoryLink);
                }
                return webCategoryLinks;
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }

        // Getting Categories from Sql, called by using Delegate
        private static Dictionary<string, List<Category>> _GetCategories(SqlDataReader reader)
        {
            try
            {
                Logger.AddToLog(new LogItem { Message = "Getting Categories from Sql", Type = "Event" });
                Dictionary<string, List<Category>> CategorisDict = new Dictionary<string, List<Category>>();
                List<Category> CategorisList = new List<Category>();
                int index = 0;
                while (reader.Read())
                {

                    Category category = new Category();
                    category.ID = reader.GetInt32(0);
                    category.Name = reader.GetString(1);
                    category.Image = reader.GetString(2);

                    index++;
					if (CategorisList.Count > 1 && category.ID <= CategorisList.Last().ID && CategorisDict.Count == 0)
					{
                        CategorisDict.Add("All", CategorisList);
                        CategorisList = new List<Category>();
                    }
                    CategorisList.Add(category);
                }
                if(CategorisDict.Count == 0)
                {
                    CategorisDict.Add("All", CategorisList);
                }
                else
                {
                    CategorisDict.Add("Favorite", CategorisList);
                }  
                return CategorisDict;
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }
    }
}
