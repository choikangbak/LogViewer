using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.VisualBasic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace LogViewer
{
    public partial class Form_IssueReporter : Form
    {
        private FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        private BackgroundWorker _backgroundWorker;
        private NameValueCollection _configuration;
        private FileUploader _fileUploader;
        private List<string> _logsSelected;
        private List<File> _filesSelected;
        private List<string> _fileIdList;

        public Form_IssueReporter()
        {
            InitializeComponent();
            _fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
            _backgroundWorker = new BackgroundWorker();
            _configuration = ConfigurationManager.AppSettings;
            _fileUploader = new FileUploader();
            _logsSelected = new List<string>(); 
            _filesSelected = new List<File>();
            _fileIdList = new List<string>();
        }

        private void Form_IssueReporter_Load(object sender, EventArgs e) { }

        public void SetLogsSelected(List<string> logsSelected)
        {
            _logsSelected = logsSelected;

            DisplayLogsSelected();
        }

        private void DisplayLogsSelected()
        {
            Tb_IssueLog.Text = "";

            for (int i = 0; i < _logsSelected.Count; i++)
            {
                Tb_IssueLog.Text += _logsSelected[i] + "\r\n";
            }
        }

        private void Btn_OpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
                openFileDialog.Title = "Open file";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePaths = openFileDialog.FileNames;

                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        string filePath = openFileDialog.FileNames[i];
                        string fileName = Path.GetFileNameWithoutExtension(filePath);
                        string fileExtension = Path.GetExtension(filePath);
                        if(!_fileExtensionContentTypeProvider.TryGetContentType(filePath, out string fileMime))
                        {
                            fileMime = _configuration["DefaultFileMime"];
                        }

                        File file = new File(filePath, fileName, fileExtension, fileMime);

                        _filesSelected.Add(file);

                        Tb_Attachment.Text += filePath + "\r\n";
                    }
                }
            } 
        }

        private void Btn_ReportIssue_Click(object sender, EventArgs e)
        {
            string issueTitle = Tb_IssueTitle.Text.Trim();
            string issueContent = Tb_IssueContent.Text.Trim();

            if (issueTitle.Length == 0 ||
                issueContent.Length == 0)
            {
                MessageBox.Show("제목과 내용을 모두 입력해주세요.", "메시지 - CLE Inc.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!isRegularExpression(issueTitle) || !isRegularExpression(issueContent))
            {
                MessageBox.Show("특수문자('{}', '\\', '`', '&', '|', '^', ';')는 포함하실 수 없습니다.", "메시지 - CLE Inc.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!IsConnected2Internet())
            {
                MessageBox.Show("인터넷을 연결해주세요.", "메시지 - CLE Inc.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                EnableControls(false);

                Pb_SendIssue.Style = ProgressBarStyle.Marquee;
                Pb_SendIssue.MarqueeAnimationSpeed = 100;

                _backgroundWorker.DoWork += Bw_SendIssue2Notion;
                _backgroundWorker.DoWork += Bw_SendIssue2Slack;
                _backgroundWorker.RunWorkerCompleted += Bw_SendIssueCompleted;
                _backgroundWorker.RunWorkerAsync();
            }
        }

        private bool isRegularExpression(string str)
        {
            var regex = new Regex(_configuration["RegularExpression"]);

            if (regex.IsMatch(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsConnected2Internet()
        {
            try
            {
                Ping ping = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = ping.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        private void EnableControls(bool isEnable)
        {
            Tb_IssueTitle.Enabled = isEnable;
            Tb_IssueContent.Enabled = isEnable;
            Tb_IssueLog.Enabled = isEnable;
            Tb_Attachment.Enabled = isEnable;
            Btn_AttachFile.Enabled = isEnable;
            Btn_ReportIssue.Enabled = isEnable;
            Btn_CancelReportIssue.Enabled = isEnable;
            this.ControlBox = isEnable;
        }

        private void Bw_SendIssue2Notion(object sender, DoWorkEventArgs e)
        {
            string issueTitle = Tb_IssueTitle.Text;
            string issueContent = Tb_IssueContent.Text; 

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(_configuration["NotionApiUrl"]);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                httpWebRequest.Headers.Add("Authorization", _configuration["NotionAuthorization"]);
                httpWebRequest.Headers.Add("Notion-Version", _configuration["NotionVersion"]);

                string json = GetNotionHttpRequestBody(issueTitle, issueContent);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine($"Result: {result}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("이슈 전송에 실패했습니다.", "메시지 - CLE Inc.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void Bw_SendIssue2Slack(object sender, DoWorkEventArgs e)
        {
            string issueTitle = Tb_IssueTitle.Text; 
            string issueContent = Tb_IssueContent.Text; 

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(_configuration["SlackApiUrl"]);
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";

                string json = GetSlackHttpRequestBody(issueTitle, issueContent);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        Console.WriteLine($"Result: {result}");
                    }
                }

                MessageBox.Show("이슈가 정상적으로 등록되었습니다.", "메시지 - CLE Inc.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void Bw_SendIssueCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Pb_SendIssue.MarqueeAnimationSpeed = 0;
            Pb_SendIssue.Value = 0;

            this.Close();
        }

        private string GetNotionHttpRequestBody(string issueTitle, string issueContent)
        {
            // 1. parent 
            string parent = "\"parent\":{\"database_id\":\"" + _configuration["NotionDatabaseId"] + "\"}";

            // 2. icon
            string icon = "\"icon\":{\"emoji\":\"📢\"}";

            // 3. properties
            string properties_title = "\"Title\":{\"title\":[{\"text\":{\"content\":\"" + issueTitle + "\"}}]}";
            string properties_content = "\"Content\":{\"type\":\"rich_text\",\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + issueContent + "\"}}]}";
            string properties_status = "\"Status\":{\"type\":\"select\",\"select\":{\"name\":\"Not in Progress\",\"color\":\"red\"}}";
            string properties_file = "";
            if (_filesSelected.Count >= 1)
            {
                List<string> fileIdList = GetFileIdList();

                if (fileIdList.Count >= 1)
                {
                    properties_file += ",\"File\":{\"type\":\"files\",\"files\":[";
                    for (int j = 0; j < fileIdList.Count; j++)
                    {
                        properties_file += "{\"name\":\"" + _filesSelected[j].FileName + _filesSelected[j].FileExtension + "\",\"type\":\"external\",\"external\":{\"url\":\"https://drive.google.com/file/d/" + fileIdList[j] + "\"}}";

                        if (j < fileIdList.Count - 1) properties_file += ",";
                    }
                    properties_file += "]}";
                }
            }
            string properties = "\"properties\":{" + properties_title + "," + properties_content + "," + properties_status + properties_file + "}";

            // 4. children
            string children = "\"children\":[" + "{\"object\":\"block\",\"type\":\"heading_2\",\"heading_2\":{\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + "관련된 로그(들)" + "\"}}]}}";
            if (_logsSelected.Count > 0)
            {
                children += ",";
                for (int i = 0; i < _logsSelected.Count; i++)
                {
                    children += "{\"object\":\"block\",\"type\":\"bulleted_list_item\",\"bulleted_list_item\":{\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + _logsSelected[i] + "\"}}]}}";

                    if (i < _logsSelected.Count - 1) children += ",";
                }
                children += "]";
            }

            string json = "{" + parent + "," + icon + "," + properties + "," + children + "}";

            return json;
        }

        private List<string> GetFileIdList()
        {   
            for (int i = 0; i < _filesSelected.Count; i++)
            {
                var file = _fileUploader.UploadFile(_filesSelected[i]);

                if (file != string.Empty)
                {
                    _fileIdList.Add(file);
                }
                else
                {
                    MessageBox.Show("파일 전송에 실패하였습니다.", "메시지 - CLE Inc.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }

            if (_filesSelected.Count != _fileIdList.Count)
            {
                for (int j = 0; j < _fileIdList.Count; j++)
                {
                    _fileUploader.DeleteFile(_fileIdList[j]);
                }

                _fileIdList.Clear();
            }

            return _fileIdList;
        }

        private string GetSlackHttpRequestBody(string issueTitle, string issueContent)
        { 
            string json = "{\"text\":\"*** 새로운 이슈 ***\n제목: " + issueTitle + "\n내용: " + issueContent + "\n\n이슈 링크: \n" + _configuration["SlackIssueTrackerUrl"] + "\"}";

            return json;
        }

        private void Btn_CancelReportIssue_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}