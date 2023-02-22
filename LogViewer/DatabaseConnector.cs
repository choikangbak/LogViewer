using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer
{
    public class DatabaseConnector
    {
        private string _connectionString;

        public DatabaseConnector(string connectionString) 
        {
            _connectionString = connectionString;
        }

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
