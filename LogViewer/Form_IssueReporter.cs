using Microsoft.VisualBasic;
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

        public Form_IssueReporter()
        {
            InitializeComponent();
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
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.notion.com/v1/pages");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            httpWebRequest.Headers.Add("Authorization", "secret_0dhSZOLiOJagsBigLH9ok1C0PlBaAGesjCdNOBcCoOp");
            httpWebRequest.Headers.Add("Notion-Version", "2022-02-22");

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
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://hooks.slack.com/services/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");
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
                }
            }
        }

        private string GetNotionHttpRequestBody(string issueTitle, string issueContent)
        {
            string parent = "\"parent\":{\"database_id\":\"" + "337d9b78209b442185cccb11ba028dc4" + "\"}";

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

                    var image = uploader.UploadFile(_filesSelected[i].FilePath, "image/" + _filesSelected[i].FileExtension, "11RQP2w8HdhlB3PdkbDSjFdMn8wogqGO2");
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

        private string GetSlackHttpRequestBody(string issueTitle, string issueContent, string issueTrackerUrl="https://www.notion.so/337d9b78209b442185cccb11ba028dc4?v=dda35a4831034c02bd1c5100e9d5abe6")
        { 
            string json = "{\"text\":\"*** 새로운 이슈 ***\n제목: " + issueTitle + "\n내용: " + issueContent + "\n\n이슈 링크: \n" + issueTrackerUrl + "\"}";

            return json;
        }
    }
}

