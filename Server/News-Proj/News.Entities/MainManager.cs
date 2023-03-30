using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace News.Entities
{
	public class MainManager
	{
		// Singleton MainManager class
		private static readonly MainManager _Instance = new MainManager();
		public static MainManager Instance
		{
			get { return _Instance; }
		}
		private const int TaskSuspendTime = 1000 * 60 * 60; // 1sec * 60 * 60 = 1 hour\
		private MainManager() { }

		public void Init()
		{
			try
			{
				Logger.AddToLog(new LogItem { Message = "Software has started.", Type = "Event" });

			}
			catch (Exception ex)
			{
				Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
				throw;
			}
			// Initiallizing All web managers and activates the tasks for getting new articles each hour
			WallaWebsiteManager.Instance.Init();
			MaarivWebsiteManager.Instance.Init();
			GlobesWebsiteManager.Instance.Init();
			YnetWebsiteManager.Instance.Init();


			Task.Run(async () =>
			{
				while (true)
				{
					try
					{
						await GetVideoLinks();
						Thread.Sleep(TaskSuspendTime);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						throw;
					}
					
				}
			});
		}

		// Gets video links from YouTube

		public async Task GetVideoLinks()
		{
			try
			{
				string[] YouTubeAPIs = Data_SQL.ArticleDataSQL.GetApi();
				string[] ArticleTitles = Data_SQL.ArticleDataSQL.GetArticleTitles();
				List<YT_Video> YT_Videos = new List<YT_Video>();
				int VideoIndex = 0;
				foreach (var Title in ArticleTitles)
				{
					VideoIndex = 0;
					var youtubeService = new YouTubeService(new BaseClientService.Initializer()
					{
						ApiKey = YouTubeAPIs[1],
						ApplicationName = "NewsProject"
					});

					var searchListRequest = youtubeService.Search.List("snippet");
					searchListRequest.Q = Title; // Replace with your search term.
					searchListRequest.MaxResults = 3;

					// Call the search.list method to retrieve results matching the specified query term.
					var searchListResponse = await searchListRequest.ExecuteAsync();

					List<string> videos = new List<string>();

					// Add each result to the appropriate list, and then display the lists of
					// matching videos, channels, and playlists.

					foreach (var searchResult in searchListResponse.Items)
					{
						if (VideoIndex < 3)
						{
							videos.Add("https://www.youtube.com/watch?v=" + searchResult.Id.VideoId);
							VideoIndex++;
						}
					}

					YT_Videos.Add(new YT_Video { ArticleTitle = Title, Links = videos.ToArray() });
					Console.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
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
