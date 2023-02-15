using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly NpgsqlConnection conn;

        public LogRepository()
        {
            conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            conn.Open();
        }

        public async Task<List<T>> GetAll<T>(string command, object parms)
        {
            List<T> result = new List<T>();

            result = (await conn.QueryAsync<T>(command, parms)).ToList();

            return result;
        }
    }
}
