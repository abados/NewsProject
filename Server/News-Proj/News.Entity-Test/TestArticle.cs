using News.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using News.DAL;
using News.Data_SQL;
using Utilities;
using News.Models;

namespace News.Entity_Test
{
    public class TestArticle
    {
        string AuthID;
        bool answer;
       

		[SetUp]//first to run
		public void Init()
		{
			AuthID = "Semion";
			MainManager.Instance.Init();
			//  Logger.AddToLog(new LogItem());
		}

		[Test, Order(1), Category("Find User")]
		public void RunUserTest()
		{
			//check if it can find the user
			answer = CommonWebManager.Instance.CheckIfRegistered(AuthID);
			Assert.True(answer, "Find User - working");

			//check if it can find that the user dont exsits
			answer = CommonWebManager.Instance.CheckIfRegistered("Hay");
			Assert.True(!answer, "found that user dont exsits - working");

		}

		[Test, Order(2), Category("Find Categories")]
		public void RunCategoriesTest()
		{
			//check if it can find the user
			Dictionary<string, List<Category>> checkCategories = CommonWebManager.Instance.GetCategories(AuthID);

			Assert.IsNotNull(checkCategories, "Find Categories - working");

			Dictionary<string, List<Category>> checkFavoriteCategoriesNewUser = CommonWebManager.Instance.GetCategories("AAA");
			Assert.AreNotEqual(checkCategories.Count, checkFavoriteCategoriesNewUser.Count);
		}

		[Test, Order(3), Category("Find Articles")]
		public void RunArticlesTest()
		{
			//check if it can find the user
			Dictionary<string, List<Article>> checkArticles = CommonWebManager.Instance.GetTopArticles(AuthID);
			Dictionary<string, List<Article>> checkPopularArticles = CommonWebManager.Instance.GetPopularArticles(AuthID);
			List<Article> checkCuriousArticles = CommonWebManager.Instance.GetCuriousArticles(AuthID);
			Assert.IsNotNull(checkArticles, "Find Top Articles - working");
			Assert.IsNotNull(checkPopularArticles, "Find Favorite Articles - working");
			Assert.IsNotNull(checkCuriousArticles, "Find Curious Articles - working");

		}
	}
}
