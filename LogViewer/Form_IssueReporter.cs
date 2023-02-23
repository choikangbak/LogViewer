using Microsoft.VisualBasic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace LogViewer
{
    public partial class Form_IssueReporter : Form
    {
        private List<string> _logsSelected;
        private List<File> _filesSelected;
        private readonly NameValueCollection _appSettings;

        public Form_IssueReporter()
        {
            InitializeComponent();
            _appSettings = ConfigurationManager.AppSettings;
        }

        private void Form_IssueReporter_Load(object sender, EventArgs e) { }

        private void EnableControls(bool isEnable)
        {
            Tb_IssueTitle.Enabled = isEnable;
            Tb_IssueContent.Enabled = isEnable;
            Tb_IssueLog.Enabled = isEnable;
            Tb_Attachment.Enabled = isEnable;
            Btn_AttachFile.Enabled = isEnable;
            Btn_ReportIssue.Enabled = isEnable;
            Btn_CancelReportIssue.Enabled = isEnable;
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

                    _filesSelected = new List<File>();

                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        string filePath = openFileDialog.FileNames[i];
                        string fileName = Path.GetFileNameWithoutExtension(filePath);
                        string fileExtension = Path.GetExtension(filePath);

                        File file = new File(filePath, fileName, fileExtension);

                        _filesSelected.Add(file);

                        Tb_Attachment.Text = filePath;
                    }
                }
            } 
        }

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

        private void Btn_ReportIssue_Click(object sender, EventArgs e)
        {
            if (Tb_IssueTitle.Text.Trim().Length == 0 || 
                Tb_IssueContent.Text.Trim().Length == 0)
            {
                MessageBox.Show("제목과 내용을 모두 입력해주세요.", "메시지 - CLE Inc.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                EnableControls(false);

                string issueTitle = Tb_IssueTitle.Text;
                string issueContent = Tb_IssueContent.Text;

                SendIssue2Notion(issueTitle, issueContent);
                SendIssue2Slack(issueTitle, issueContent);

                this.Close();
            }
        }

        private void SendIssue2Notion(string issueTitle, string issueContent)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(_appSettings["NotionApiUrl"]);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            httpWebRequest.Headers.Add("Authorization", _appSettings["NotionAuthorization"]);
            httpWebRequest.Headers.Add("Notion-Version", _appSettings["NotionVersion"]);

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
            }
        }

        private void SendIssue2Slack(string issueTitle, string issueContent)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(_appSettings["SlackApiUrl"]);
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
                    Console.WriteLine(result);
                }
            }
        }

        private string GetNotionHttpRequestBody(string issueTitle, string issueContent)
        {
            string parent = "\"parent\":{\"database_id\":\"" + _appSettings["NotionDatabaseId"] + "\"}";

            string icon = "\"icon\":{\"emoji\":\"📢\"}";

            string properties_title = "\"Title\":{\"title\":[{\"text\":{\"content\":\"" + issueTitle + "\"}}]}";
            string properties_content = "\"Content\":{\"type\":\"rich_text\",\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + issueContent + "\"}}]}";
            string properties_status = "\"Status\":{\"type\":\"select\",\"select\":{\"name\":\"Not in Progress\",\"color\":\"red\"}}";
            string properties_image = "";
            if (_filesSelected != null)
            {
                properties_image += ",";

                List<string> images = new List<string>();

                for (int i = 0; i < _filesSelected.Count; i++)
                {
                    FileUploader uploader = new FileUploader();

                    var service = uploader.GetService();

                    var image = uploader.UploadFile(_filesSelected[i]);

                    images.Add(image);
                }

                properties_image += "\"Image\":{\"type\":\"files\",\"files\":[";
                for (int j = 0; j < images.Count; j++)
                {
                    properties_image += "{\"name\":\"" + _filesSelected[j].FileName + "\",\"type\":\"external\",\"external\":{\"url\":\"https://drive.google.com/file/d/" + images[j] + "\"}}";

                    if (j < images.Count - 1) properties_image += ",";
                }
                properties_image += "]}";
            }
            string properties = "\"properties\":{" + properties_title + "," + properties_content + "," + properties_status + properties_image + "}";

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

        private string GetSlackHttpRequestBody(string issueTitle, string issueContent)
        { 
            string json = "{\"text\":\"*** 새로운 이슈 ***\n제목: " + issueTitle + "\n내용: " + issueContent + "\n\n이슈 링크: \n" + _appSettings["SlackIssueTrackerUrl"] + "\"}";

            return json;
        }
    }
}

