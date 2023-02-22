using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer
{
    internal class File
    {
        public string FilePath;
        public string FileName;
        public string FileExtension;

        public File(string filePath, string fileName, string fileExtension) 
        {
            FilePath = filePath;
            FileName = fileName;
            FileExtension = fileExtension;
        }
    }
}
