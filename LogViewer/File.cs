using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer
{
    public class File
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileMime { get; set; }

        public File(string filePath, string fileName, string fileExtension, string fileMime) 
        {
            FilePath = filePath;
            FileName = fileName;
            FileExtension = fileExtension;
            FileMime = fileMime;
        }
    }
}
