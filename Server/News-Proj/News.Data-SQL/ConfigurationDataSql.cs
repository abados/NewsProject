using News.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Data_SQL
{
    public class ConfigurationDataSql
    {

        public static string GetSingleConfigurationData(string SqlQuey)
        {
            return (string)Dal.RunCommandResultSingleValue(SqlQuey);
        }
        

        public static string[] GetAllSources()
        {
            string SqlQuery = "Select Value from Configuration where ID >= 1 and ID < 5 ";
            return (string[])Dal.RunCommandResultMultiValues(SqlQuery);
        }
    }
}
