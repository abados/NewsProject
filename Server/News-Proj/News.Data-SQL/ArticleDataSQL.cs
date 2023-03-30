using News.DAL;
using News.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using Utilities;

namespace News.Data_SQL
{
	public class ArticleDataSQL : BaseDataSQL
	{
		// ArticleDataSQL class inhert from BaseDataSQL

		// Gets the Title of each article
		public static string[] GetArticleTitles()
		{
			try
			{
				string SqlQuery = "select Title from Article";
				Logger.AddToLog(new LogItem { Message = "RunCommandResultMultiValues has been called, getting Article Titles", Type = "Event" });
				return Dal.RunCommandResultMultiValues(SqlQuery);
			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}
		}

		// Gets YouTube API Query and Key
		public static string[] GetApi()
		{
			try
			{
				string SqlQuery = "select Value from Configuration where Name = 'YouTubeApiQuery' \r\nor Name = 'YouTubeApiKey'";
				Logger.AddToLog(new LogItem { Message = "RunCommandResultMultiValues has been called, getting YouTube API values", Type = "Event" });
				return Dal.RunCommandResultMultiValues(SqlQuery);
			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}
		}

		// Saves article list to Sql
		public static void SaveArticlesToDB(List<Article> NewArticles)
		{
			try
			{
				Logger.AddToLog(new LogItem { Message = "SaveArticlesToDB has been called", Type = "Event" });
				string SqlQuery = "INSERT INTO Article VALUES (@Title, @Description, @Image, @Link, @Source, @CategoryID, @NumberOfClicks)";
				Dal.SaveArticlesToDB(SqlQuery, NewArticles);
			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}
		}

		// Getting an array of Newest Articles (1 per category)
		public static Article[] GetNewestArticles(string Source)
		{
			try
			{
				Logger.AddToLog(new LogItem { Message = "_GetNewestArticles has been called", Type = "Event" });
				string SqlQuery = $" SELECT * FROM (\r\n    SELECT *, ROW_NUMBER() OVER (PARTITION BY CategoryID ORDER BY ID DESC) AS rn\r\n    FROM Article\r\n    WHERE Source = '{Source}'\r\n) t\r\nWHERE t.rn = 1";
				return (Article[])Dal.GetDataFromDB(SqlQuery, _GetNewestArticles);

			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}
		}

		// Getting favorite articles (by favorite category from each source)
		public static Dictionary<string, List<Article>> GetTopArticles(string AuthID)
		{
			try
			{
				Logger.AddToLog(new LogItem { Message = "_GetTopArticles has been called", Type = "Event" });
				string SqlQuery = $"EXEC GetFavoriteArticles @AuthID = '{AuthID}'";
				return (Dictionary<string, List<Article>>)Dal.GetDataFromDB(SqlQuery, _GetTopArticles);

			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}
		}

		// Getting popular articles (10 that has most clicks)
		public static Dictionary<string, List<Article>> GetPopularArticles(string AuthID)
		{
			try
			{
				Logger.AddToLog(new LogItem { Message = "_GetArticleList has been called", Type = "Event" });
				string SqlQuery = $"EXEC GetPopularArticles '{AuthID}'";
				return (Dictionary<string, List<Article>>)Dal.GetDataFromDB(SqlQuery, _GetPopularArticles);

			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}
		}

		// Getting "Curious Articles" (10 with 0 clicks from his favorite category)
		public static List<Article> GetCuriousArticles(string AuthID)
		{
			try
			{
				Logger.AddToLog(new LogItem { Message = "_GetArticleList has been called", Type = "Event" });
				string SqlQuery = $"EXEC GetCuriousArticles '{AuthID}'";
				return (List<Article>)Dal.GetDataFromDB(SqlQuery, _GetCuriousArticles);

			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}
		}

		// Delegate function - Getting newest articles
		private static Article[] _GetNewestArticles(SqlDataReader reader)
		{
			try
			{
				Logger.AddToLog(new LogItem { Message = "Getting newest articles from Sql", Type = "Event" });
				List<Article> Articles = new List<Article>();
				while (reader.Read())
				{
					Article Article = new Article
					{
						Title = reader.GetString(1),
						Description = reader.GetString(2),
						Image = reader.GetString(3),
						Link = reader.GetString(4),
						Source = reader.GetString(5),
						CategoryID = reader.GetInt32(6),
						TimesClicked = reader.GetInt32(7)
					};

					Articles.Add(Article);

				}
				return Articles.ToArray();

			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}
		}

		// Delegate function - Getting top articles from Sql
		private static Dictionary<string, List<Article>> _GetTopArticles(SqlDataReader reader)
		{
			try
			{
				Logger.AddToLog(new LogItem { Message = "Getting top articles from Sql", Type = "Event" });
				Dictionary<string, List<Article>> ArticlesDictionary = new Dictionary<string, List<Article>>();
				List<Article> ArticlesList = new List<Article>();
				string CategoryName = "";
				while (reader.Read())
				{
					Article Article = new Article
					{
						ID = reader.GetInt32(reader.GetOrdinal("ID")),
						Title = reader.GetString(reader.GetOrdinal("Title")),
						Description = reader.GetString(reader.GetOrdinal("Description")),
						Image = reader.GetString(reader.GetOrdinal("Image")),
						Link = reader.GetString(reader.GetOrdinal("Link")),
						Source = reader.GetString(reader.GetOrdinal("Source")),
						CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
						TimesClicked = reader.GetInt32(reader.GetOrdinal("NumberOfClicks"))
					};

					if (ArticlesList.Count > 0 && Article.CategoryID != ArticlesList.Last().CategoryID)
					{
						ArticlesDictionary.Add(CategoryName, ArticlesList);
						ArticlesList = new List<Article>();
					}
					CategoryName = reader.GetString(reader.GetOrdinal("Name"));
					ArticlesList.Add(Article);
				}
				if (CategoryName != "" && !ArticlesDictionary.ContainsKey(CategoryName))
				{
					ArticlesDictionary.Add(CategoryName, ArticlesList);
					return ArticlesDictionary;
				}
				return null;
			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}

		}

		// Delegate function - Getting article list from Sql
		private static Dictionary<string, List<Article>> _GetPopularArticles(SqlDataReader reader)
		{
			try
			{
				Logger.AddToLog(new LogItem { Message = "Getting article list from Sql", Type = "Event" });
				Dictionary<string, List<Article>> ArticlesDict = new Dictionary<string, List<Article>>();
				List<Article> ArticlesList = new List<Article>();
				string CategoryName = "";
				while (reader.Read())
				{
					Article Article = new Article
					{
						ID = reader.GetInt32(reader.GetOrdinal("ID")),
						Title = reader.GetString(reader.GetOrdinal("Title")),
						Description = reader.GetString(reader.GetOrdinal("Description")),
						Image = reader.GetString(reader.GetOrdinal("Image")),
						Link = reader.GetString(reader.GetOrdinal("Link")),
						Source = reader.GetString(reader.GetOrdinal("Source")),
						CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
						TimesClicked = reader.GetInt32(reader.GetOrdinal("NumberOfClicks"))
					};
					if (ArticlesList.Count > 0 && ArticlesList.Last().CategoryID != Article.CategoryID)
					{
						ArticlesDict.Add(CategoryName, ArticlesList);
						ArticlesList = new List<Article>();
					}
					CategoryName = reader.GetString(reader.GetOrdinal("Name"));
					ArticlesList.Add(Article);
				}
				if (CategoryName != "" && !ArticlesDict.ContainsKey(CategoryName))
				{
					ArticlesDict.Add(CategoryName, ArticlesList);
					return ArticlesDict;
				}
				return ArticlesDict;
			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}

		}

		// Delegate function - Getting article list from Sql
		private static List<Article> _GetCuriousArticles(SqlDataReader reader)
		{
			try
			{
				Logger.AddToLog(new LogItem { Message = "Getting article list from Sql", Type = "Event" });

				List<Article> ArticlesList = new List<Article>();
				while (reader.Read())
				{
					Article Article = new Article
					{
						ID = reader.GetInt32(reader.GetOrdinal("ID")),
						Title = reader.GetString(reader.GetOrdinal("Title")),
						Description = reader.GetString(reader.GetOrdinal("Description")),
						Image = reader.GetString(reader.GetOrdinal("Image")),
						Link = reader.GetString(reader.GetOrdinal("Link")),
						Source = reader.GetString(reader.GetOrdinal("Source")),
						CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
					};
					ArticlesList.Add(Article);
				}
				return ArticlesList;
			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}

		}
	}
}