using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Repositories
{
    public interface ILogRepository
    {
        Task<List<T>> GetAll<T>(string command, object parms);
    }
}
