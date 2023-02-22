using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public DateTime Created_At { get; set; }

        public Log(int id, DateTime timestamp, string level, string message, DateTime created_at) 
        {
            Id = id;
            Timestamp = timestamp;
            Level = level;
            Message = message;
            Created_At = created_at;
        }
    }
}
