using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace News.Entities
{

    public interface IWebsite
    {
        Dictionary<int, string> WebAddressCategories { get; set; }
        void FetchAndSaveData();
        string GetDescriptionText(XmlNode descriptionNode);
        Task<List<Article>> GetArticles();
        string GetImageSrc(XmlNode descriptionNode, XmlNode node);
        void CreateNewestArticleList(List<Article> Articles);
        string GetSource(string imgSource);
        void LoadArticlesFromXML(List<Article> Articles, string content, int CategoryID);
        Dictionary<int, string> GetCategoryLinks(string Source);
        Article[] CheckNewestArticleFromDB(string SourceName);
    }

}
