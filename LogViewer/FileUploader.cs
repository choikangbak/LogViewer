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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static Google.Apis.Drive.v3.DriveService;

namespace LogViewer
{
    public class FileUploader
    {
        private readonly NameValueCollection _configuration;

        public FileUploader() 
        {
            _configuration = ConfigurationManager.AppSettings;
        }

        public DriveService GetService()
        {
            var json = new
            {
                type = _configuration["GoogleDriveType"],
                project_id = _configuration["GoogleDriveProjectId"],
                private_key_id = _configuration["GoogleDrivePrivateKeyId"],
                private_key = _configuration["GoogleDrivePrivateKey"],
                client_email = _configuration["GoogleDriveClientEmail"],
                client_id = _configuration["GoogleDriveClientId"],
                auth_uri = _configuration["GoogleDriveAuthUri"],
                token_uri = _configuration["GoogleDriveTokenUri"],
                auth_provider_x509_cert_url = _configuration["GoogleDriveAuthProviderX509CertUrl"],
                client_x509_cert_url = _configuration["GoogleDriveClientX509CertUrl"]
            };

            var credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(json)).CreateScoped(DriveService.ScopeConstants.Drive);

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
                string fileMime = file.FileMime; 

                var driveFile = new Google.Apis.Drive.v3.Data.File();
                driveFile.Name = fileName;
                driveFile.MimeType = fileMime;
                driveFile.Parents = new string[] { _configuration["GoogleDriveFolderId"] };

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
