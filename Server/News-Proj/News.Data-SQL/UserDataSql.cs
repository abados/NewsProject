using News.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace News.Data_SQL
{
    public class UserDataSql: BaseDataSQL
    {
        // ArticleDataSQL class inhert from BaseDataSQL

        // Checks if user is registered, if not, register him
        public static bool CheckIfRegistered(string AuthID)
        {
            try
            {
                string SqlQuery = $"Exec CheckIfHasFavorite @AuthID";
                Logger.AddToLog(new LogItem { Message = "RunCommandCheck function has been called" });
                return Dal.RunCommandCheck(SqlQuery, AuthID);
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }

        }

        // Updating number of clicks in two tables, Users individual and Article common
        public static void UpdateNumberOfClicks(string AuthID, int ArticleID)
        {
            try
            {
                string SqlQuery = $"Exec UserClicked '{AuthID}', {ArticleID}";
                Logger.AddToLog(new LogItem { Message = "RunNonQuery function has been called" });
                Dal.RunNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                Logger.AddToLog(new LogItem { exception = ex, Message = ex.Message, Type = "Exception" });
                throw;
            }

        }


    }
}
