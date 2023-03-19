using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private MainManager() { }

        public void Init()
        {
            Logger.AddToLog(new LogItem { Message = "Software has started.", Type = "Event"});
            // Initiallizing All web managers and activates the tasks for getting new articles each hour
            WallaWebsiteManager.Instance.Init();
            MaarivWebsiteManager.Instance.Init();
            GlobesWebsiteManager.Instance.Init();
            YnetWebsiteManager.Instance.Init();
        }
    }
}
