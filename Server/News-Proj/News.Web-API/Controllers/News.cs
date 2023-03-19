using Microsoft.AspNetCore.Mvc;
using News.Entities;
using News.Models;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using Utilities;

namespace News.Web_API.Controllers
{
    [ApiController, Route("Api/News")]
    public class News : ControllerBase
    {

        [HttpGet("GetTopArticles/{AuthID}")]
        public IActionResult GetTopArticles(string AuthID)
        {
            if (AuthID == null)
            {
                return BadRequest(HttpStatusCode.BadRequest);
            }
            try
            {
                // Check if registered, if not, register him
                CommonWebManager.Instance.CheckIfRegistered(AuthID);

                // Getting 10 newest articles from his favorite categories from each source
                Dictionary<string, List<Article>> ArticlesDict = CommonWebManager.Instance.GetTopArticles(AuthID);
               
                // if no categories picked yet, dict is empty
                if(ArticlesDict == null)
                {
                    Logger.AddToLog(new LogItem { Message = "User hasn't picked any categories yet.", Type = "Event" });
                    return Ok("No categories selected yet");
                }
                Logger.AddToLog(new LogItem { Message = "User is getting articles by his favorite categories from each source", Type = "Event" });
                return Ok(System.Text.Json.JsonSerializer.Serialize(ArticlesDict));
            }
            catch (Exception ex)
            {
                Utilities.Logger.AddToLog(new Utilities.LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                return BadRequest(HttpStatusCode.BadRequest);
            }
        }


        [HttpGet("GetCategories/{AuthID}")]
        public IActionResult GetCategories(string AuthID)
        {
            try
            {
                // Getting all categories and user's favorite categories
                Logger.AddToLog(new LogItem { Message = "GetCategories function called", Type = "Event" });
                return Ok(CommonWebManager.Instance.GetCategories(AuthID));
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                
                return BadRequest(HttpStatusCode.BadRequest);
            }         
        }

        [HttpPost("UpdateFavoriteCategories/{AuthID}/{CategoriesID}")]
        public IActionResult UpdateFavoriteCategories(string AuthID, string CategoriesID)
        {
            try
            {
                // Updating user's favorite categories
                Logger.AddToLog(new LogItem { Message = "UpdateFavoriteCategories function called", Type = "Event" });
                string[] strArray = CategoriesID.Split(','); // split the string into an array of substrings
                int[] CategoriesIDArr = new int[strArray.Length]; // create an integer array of the same length as the string array

                for (int i = 0; i < strArray.Length; i++)
                {
                    CategoriesIDArr[i] = int.Parse(strArray[i]); // convert each substring to an integer and store it in the integer array
                }
                return Ok(CommonWebManager.Instance.UpdateFavoriteCategories(AuthID, CategoriesIDArr));
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                return BadRequest(HttpStatusCode.BadRequest);
            }  
        }

        [HttpGet("GetPopularArticles/{AuthID}")]
        public IActionResult GetPopularArticles(string AuthID)
        {
            try
            {
                // Getting the articles with the most clicks (most popular)
                Logger.AddToLog(new LogItem { Message = "GetPopularArticles function called", Type = "Event" });
                return Ok(CommonWebManager.Instance.GetPopularArticles(AuthID));
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                return BadRequest(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet("GetCuriousArticles/{AuthID}")]
        public IActionResult GetCuriousArticles(string AuthID)
        {
            try
            {
                // Getting the articles with 0 clicks (from his favorite categories)
                Logger.AddToLog(new LogItem { Message = "GetCuriousArticles function called", Type = "Event" });
                return Ok(CommonWebManager.Instance.GetCuriousArticles(AuthID));
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                return BadRequest(HttpStatusCode.BadRequest);
            }
         
        }

        [HttpPost("UpdateNumberOfClicks/{AuthID}/{ArticleID}")]
        public IActionResult UpdateNumberOfClicks(string AuthID, int ArticleID)
        {
            try
            {
                // Updating clicks per Article
                CommonWebManager.Instance.UpdateNumberOfClicks(AuthID, ArticleID);
                Logger.AddToLog(new LogItem { Message = "The number of clicks has been updated", Type = "Event" });
                return Ok("All good");
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                return BadRequest(HttpStatusCode.BadRequest);
            }
        }
    }
}
