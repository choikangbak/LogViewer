using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using static Google.Apis.Drive.v3.DriveService;

namespace LogViewer
{
    public class FileUploader
    {
        private readonly NameValueCollection _appSettings;

        public FileUploader() 
        {
            _appSettings = ConfigurationManager.AppSettings;
        }

        public DriveService GetService()
        {
            var credential = GoogleCredential.FromFile(_appSettings["GoogleDriveCredentialFilePath"]).CreateScoped(DriveService.ScopeConstants.Drive);

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            return service;
        }

        public string UploadFile(File file)
        {
            try
            {
                DriveService service = GetService();

                string fileName = file.FileName + "[" + DateTime.Now + "]";
                string fileMime = "image/" + file.FileExtension;

                var driveFile = new Google.Apis.Drive.v3.Data.File();
                driveFile.Name = fileName; // this should be time + name
                driveFile.MimeType = fileMime;
                driveFile.Parents = new string[] { _appSettings["GoogleDriveFolderId"] };

                var fsSource = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read);

                var request = service.Files.Create(driveFile, fsSource, fileMime);
                request.Fields = "*";

                var response = request.Upload();

                if (response.Status == UploadStatus.Failed)
                {
                    Console.WriteLine($"Error uploading file: {response.Exception.Message}");
                }

                return request.ResponseBody.Id;
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                return string.Empty;
            }
        }

        public void DeleteFile(string fileId)
        {
            try
            {
                var service = GetService();
                var command = service.Files.Delete(fileId);
                var result = command.Execute();
            } 
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
