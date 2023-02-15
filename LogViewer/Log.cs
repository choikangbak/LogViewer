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
        [System.ComponentModel.DataAnnotations.Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("level")]
        public string Level { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
