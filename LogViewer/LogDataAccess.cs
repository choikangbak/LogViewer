using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer
{
    public class LogDataAccess
    {
        private NpgsqlConnection _connection;

        public LogDataAccess(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public List<Log> SearchLog(string startTime, string endTime, List<string> levels, string keyword, string order)
        {
            try
            {
                _connection.Open();

                string sql = string.Format("SELECT * FROM log WHERE {0} {1} {2} ORDER BY created_at {3}",
                                            GetTimeStatement(startTime, endTime),
                                            GetLevelStatement(levels),
                                            GetKeywordStatement(keyword),
                                            order);

                List<Log> logList = _connection.Query<Log>(sql).ToList();

                _connection.Close();

                return logList;
            }
            catch (Exception ex)
            {
                _connection.Close();

                Console.WriteLine("Error: "+ex.Message);

                return new List<Log> { };
            }
        }

        private string GetTimeStatement(string startTime, string endTime)
        {
            string timeStmt = string.Format(" ( timestamp >= '{0}' AND timestamp <= '{1}' ) ", startTime, endTime);

            return timeStmt;
        }

        private string GetKeywordStatement(string keyword)
        {
            string keywordStmt = string.Format(" AND ( message LIKE '%{0}%' ) ", keyword);

            return keywordStmt;
        }

        private string GetLevelStatement(List<string> levels)
        {
            int n = levels.Count;

            if (n == 0)
            {
                return " AND level NOT IN ('T', 'D', 'I', 'W', 'E', 'C') "; // later to be ('Trace', 'Debug', 'Info', 'Warning', 'Error', 'Critical')
            }

            string levelStmt = " AND ( ";
            for (int i = 0; i < n; i++)
            {
                string level = levels[i];

                levelStmt += string.Format(" level = '{0}' ", level);

                if (i < n - 1) levelStmt += " OR ";
            }
            levelStmt += " ) ";

            return levelStmt;
        }
    }
}
