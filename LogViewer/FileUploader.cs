using System;
using System.Collections.Generic;
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
using static Google.Apis.Drive.v3.DriveService;

namespace LogViewer
{
    public class FileUploader
    {
        private const string serviceAccountCredentialPath = @"C:\enhanced-victor-378408-070c7f6dbe72.json";
        private const string ServiceAccountEmail = "test-999@enhanced-victor-378408.iam.gserviceaccount.com";
        private const string UploadFilePath = @"C:\Users\jun_c\Downloads\logo_mark_square.png";
        private const string UploadFileName = "클레";
        private const string mime = "image/png";
        private const string DirectoryId = "11RQP2w8HdhlB3PdkbDSjFdMn8wogqGO2";

        public DriveService GetService()
        {
            var credential = GoogleCredential.FromFile(serviceAccountCredentialPath).CreateScoped(DriveService.ScopeConstants.Drive);

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            return service;
        }

        public string UploadFile(string filePath, string fileMime, string folderId)
        {
            DriveService service = GetService();

            DateTime now = DateTime.Now;
            string fileName = now + ""; // later to be changed
            
            var driveFile = new Google.Apis.Drive.v3.Data.File();
                    driveFile.Name = fileName; // this should be time + name
                    driveFile.MimeType = fileMime;
                    driveFile.Parents = new string[] { folderId };

            var fsSource = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var request = service.Files.Create(driveFile, fsSource, fileMime);
            request.Fields = "*";

            var response = request.Upload();

            if (response.Status == UploadStatus.Failed)
            {
                Console.WriteLine($"Error uploading file: {response.Exception.Message}");
            }

            return request.ResponseBody.Id;
        }

        public void DeleteFile(string fileId)
        {
            var service = GetService();
            var command = service.Files.Delete(fileId);
            var result = command.Execute();
        }
    }
}
