using News.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace News.Entities
{

	public interface IWebsite
	{
		Dictionary<int, string> WebAddressCategories { get; set; }
		void FetchAndSaveData();
		string GetDescriptionText(XmlNode descriptionNode);
		Task GetArticles();
		string GetImageSrc(XmlNode descriptionNode, XmlNode node);
		void CreateNewestArticleList(List<Article> Articles);
		string GetSource(string imgSource);
		void LoadArticlesFromXML(List<Article> Articles, string content, int CategoryID);
		Dictionary<int, string> GetCategoryLinks(string Source);
		Article[] CheckNewestArticleFromDB(string SourceName);
	}

}
