using News.DAL;
using News.Data_SQL;
using News.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Utilities;

namespace News.Entities
{
    public abstract class BaseEntity : BaseNewsSystem, IWebsite
    {

        // Get's number of Categories from DB
        int NumberOfCategories;
        // Stores newest articles per category, 1 of each (for each source)
        public Article[] NewestArticlePerCategory;
        // The suspention time between task execution
        private const int TaskSuspendTime = 1000 * 60 * 60; // 1sec * 60 * 60 = 1 hour\

        // Stores the Web Addresses for each category per source
        public Dictionary<int, string> WebAddressCategories { get; set; }

        // The first function that runs that is in charge for fetching the articles once a hour
        public void FetchAndSaveData()
        {
            Logger.AddToLog(new LogItem { Message = "Fetching the articles once a hour - Using XML's tp get all article's info ", Type = "Event" });
            Task.Run(async () =>
            {
                NumberOfCategories = Data_SQL.CategoriesDataSql.GetNumberOfCategories();
                if (NewestArticlePerCategory == null || NewestArticlePerCategory.Length == 0)
                {
                    NewestArticlePerCategory = new Article[NumberOfCategories];
                }
                while (true)
                {
                    await GetArticles();

                    Thread.Sleep(TaskSuspendTime);
                }
            });

        }

        // Gets the articles from the XML
        public async Task<List<Article>> GetArticles()
        {
            try
            {

                List<Article> Articles = new List<Article>();
                using (var client = new HttpClient())
                {
                    foreach (var entry in WebAddressCategories)
                    {


                        // Make a GET request to the URL 
                        var response = await client.GetAsync(entry.Value);

                        // Ensure the response was successful 
                        response.EnsureSuccessStatusCode();

                        // Read the content of the response 
                        var content = await response.Content.ReadAsStringAsync();

                        // Create a new HttpResponseMessage with the JSON content 
                        var result = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(content, Encoding.UTF8, "application/json")
                        };
                        LoadArticlesFromXML(Articles, content, entry.Key);
                    }
                    if (NewestArticlePerCategory[0] == null)
                    {
                        Data_SQL.ArticleDataSQL.SaveArticlesToDB(Articles);
                        UpdateNewestArticlesArray(Articles);
                    }
                    else
                    {
                        CreateNewestArticleList(Articles);
                    }
                }
                return Articles; // must have return because of async and await
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }
        // Get's the description text per article
        public string GetDescriptionText(XmlNode descriptionNode)
        {
            try
            {
                string descriptionText = descriptionNode.InnerText.Trim();
                // Remove HTML tags from the description text
                descriptionText = Regex.Replace(descriptionText, @"<[^>]+>|&nbsp;", "").Trim();
                return descriptionText;

            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }

		// Gets the Image source from the XML
		public string GetImageSrc(XmlNode descriptionNode, XmlNode node)
		{
			try
			{
				XmlNode imgNode = descriptionNode.SelectSingleNode(".//a/img");
				if (imgNode != null)
				{
					return imgNode.Attributes["src"]?.Value;
				}
				else
				{
					string srcAttributeValue = Regex.Match(node.InnerXml, @"<img.+?src=[\""'](.+?)[\""'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
					if (string.IsNullOrEmpty(srcAttributeValue))
					{
						srcAttributeValue = Regex.Match(descriptionNode.InnerText, @"src=(['""])(.*?)\1", RegexOptions.IgnoreCase).Groups[2].Value;
						if (srcAttributeValue == "")
						{

							XmlNamespaceManager nsmgr = new XmlNamespaceManager(node.OwnerDocument.NameTable);
							nsmgr.AddNamespace("media", "http://search.yahoo.com/mrss/");
							XmlNode mediaContentNode = node.SelectSingleNode("media:content", nsmgr);
							return mediaContentNode.Attributes["url"].Value;
						}
					}
					return srcAttributeValue;
				}
			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}
		}

		// Loads Articles from XML
		public void LoadArticlesFromXML(List<Article> Articles, string content, int CategoryID)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(content);

                foreach (XmlNode node in doc.SelectNodes("//item"))
                {

                    Article article = new Article
                    {
                        Title = node["title"].InnerText,
                        Link = node["link"].InnerText,
                        CategoryID = CategoryID
                    };
                    XmlNode descriptionNode = node["description"];
                    if (descriptionNode != null)
                    {
                        article.Description = GetDescriptionText(descriptionNode);
                        article.Image = GetImageSrc(descriptionNode, node);

                    }
                    article.Source = GetSource(article.Link);
                    article.TimesClicked = 0;
                    Articles.Add(article);
                    // if it's not first run and the array is empty
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }


        // Gets the source of the XML from Link address
        public string GetSource(string Link)
        {
            string[] sources = ConfigurationDataSql.GetAllSources();
            if (Link.Contains(sources[0].ToLower()))
                return sources[0];
            if (Link.Contains(sources[1].ToLower()))
                return sources[1];
            if (Link.Contains(sources[2].ToLower()))
                return sources[2];
            if (Link.Contains(sources[3].ToLower()))
                return sources[3];
            return null;
        }

        // Gets category links per source
        public Dictionary<int, string> GetCategoryLinks(string Source)
        {
            return Data_SQL.CategoriesDataSql.GetCategoryLinks(Source);
        }

        // Gets the newest article from DB per Category
        public Article[] CheckNewestArticleFromDB(string SourceName)
        {
            try
            {
                NewestArticlePerCategory = Data_SQL.ArticleDataSQL.GetNewestArticles(SourceName);
                return NewestArticlePerCategory;
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }

        // Creates an article list from the entire list by comparing it to the newest
        // articles per category
        public void CreateNewestArticleList(List<Article> Articles)
        {
            int lastIndex = 0;
            List<Article> NewArticles = new List<Article>();
            try
            {
                if (Articles.Count > 0 && NewestArticlePerCategory[0] != null)
                {
                    for (int i = 0; i < NewestArticlePerCategory.Length ; i++)
                    {
                        for (int index = lastIndex; index < Articles.Count; index++)
                        {
                            if (Articles[index].Link != NewestArticlePerCategory[i].Link && Articles[index].CategoryID == NewestArticlePerCategory[i].CategoryID)
                            {
                                NewArticles.Add(Articles[index]);
                            }
                            else
                            {
                                lastIndex = index;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    NewArticles = Articles;
                }
                UpdateNewestArticlesArray(NewArticles);
                Data_SQL.ArticleDataSQL.SaveArticlesToDB(NewArticles);
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }

        // Update newest Articles Array with the list of newest articles
        public void UpdateNewestArticlesArray(List<Article> NewestArticles)
        {
            try
            {
                int index = 0;
                if (NewestArticlePerCategory[0] != null)
                {
                    for (int i = 0; i < NewestArticles.Count && index < NumberOfCategories; i++)
                    {
                        if ((NewestArticlePerCategory[index].CategoryID == NewestArticles[i].CategoryID) && NewestArticlePerCategory[index].Link != NewestArticles[i].Link)
                        {
                            NewestArticlePerCategory[index] = NewestArticles[i];
                            index++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < NewestArticles.Count && index < NumberOfCategories; i++)
                    {
                        if ((NewestArticlePerCategory[0] == null || NewestArticlePerCategory[index - 1].CategoryID != NewestArticles[i].CategoryID))
                        {
                            NewestArticlePerCategory[index] = NewestArticles[i];
                            index++;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }
        }
    }
}
