using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace LogViewer
{
    public partial class Form_IssueReporter : Form
    {
        private List<string> _logsSelected;
        private List<string> _files; // 

        public Form_IssueReporter()
        {
            InitializeComponent();
        }

        private void Btn_OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Image";
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileName.Length > 0)
                {
                    foreach(string s in openFileDialog.FileNames)
                    {
                        this.Tb_Attachment.Text = s;
                        _files = new List<string>();
                        _files.Add(s); //
                    }
                }
            }
        }


        public void SetLogs(List<string> logsSelected)
        {
            _logsSelected = logsSelected;

            this.Tb_IssueLog.Text = "";
            foreach (string log in logsSelected)
            {
                this.Tb_IssueLog.Text += log + "\r\n";
            }
        }

        private void SendIssue2Notion(string issueTitle, string issueContent)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.notion.com/v1/pages");
            const string authToken = "secret_0dhSZOLiOJagsBigLH9ok1C0PlBaAGesjCdNOBcCoOp";
            const string notionVersion = "2022-02-22";
            httpWebRequest.Headers.Add("Authorization", authToken);
            httpWebRequest.Headers.Add("Notion-Version", notionVersion);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

//            const string databaseId = Constants.notionDbId;
            string parent = "\"parent\":{\"database_id\":\"" + Constants.notionDbId + "\"}";

            string icon = "\"icon\":{\"emoji\":\"📢\"}";

            string properties_title = "\"Title\":{\"title\":[{\"text\":{\"content\":\"" + issueTitle + "\"}}]}";
            string properties_content = "\"Content\":{\"type\":\"rich_text\",\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + issueContent + "\"}}]}";
            string properties_status = "\"Status\":{\"type\":\"select\",\"select\":{\"name\":\"NotinProgress\",\"color\":\"red\"}}";

            string properties_image = "";
            if (_files.Count > 0)
            {
                List<string> images = new List<string>();
                foreach (string file in _files)
                {
                    FileUploader uploader = new FileUploader();
                    var service = uploader.GetService();
                    string image = uploader.UploadFile(file, "image/"+file[^3..], Constants.directoryId);
                    images.Add(image);
                }

                properties_image += "\"Image\":{\"type\":\"files\",\"files\":[";
                for (int j = 0; j < images.Count; j++)
                {
                    var image = images[j];
                    properties_image += "{\"name\":\"" + image + "\",\"type\":\"external\",\"external\":{\"url\":\"https://drive.google.com/file/d/" + image + "\"}}";

                    if (j < images.Count - 1) properties_image += ",";
                    if (j == images.Count - 1) properties_image += "]}";
                }
            }

            string properties = "\"properties\":{" + properties_title + "," + properties_content + "," + properties_status + "," + properties_image + "}";

            string child_heading2_log = "{\"object\":\"block\",\"type\":\"heading_2\",\"heading_2\":{\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + "관련된 로그(들)" + "\"}}]}}";
            string children = "\"children\":[" + child_heading2_log + ",";
            for (int i = 0; i < _logsSelected.Count; i++)
            {
                var logSelected = _logsSelected[i];
                children += "{\"object\":\"block\",\"type\":\"bulleted_list_item\",\"bulleted_list_item\":{\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + logSelected + "\"}}]}}";

                if (i < _logsSelected.Count - 1) children += ",";
                if (i == _logsSelected.Count - 1) children += "]";
            }
            string json = "{" + parent + "," + icon + "," + properties + "," + children + "}";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                Console.WriteLine(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }

        private void Btn_ReportIssue_Click(object sender, EventArgs e)
        {
            string title = this.Tb_IssueTitle.Text;
            string content = this.Tb_IssueContent.Text;
            // get image
            string[] files = openFileDlg.FileNames;

            SendIssue2Notion(title, content);
            SendIssue2Slack(Constants.urlSlack, GetSlackMessage(title, content));

            this.Close();
        }

        private void SendIssue2Slack(string URL, string message)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"text\":\"" + message + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine("slack send "+result);
                }
            }
        }

        private string GetSlackMessage(string title, string contents, string urlNotion=Constants.urlIssueNotion)
        {
            return "*** 이슈 생성 ***\n 제목 : " + title + "\n 내용 : " + contents + "\n\nIssue Tracker : " + urlNotion;
        }

    }
}

static class Constants
{
    public const string urlIssueNotion = "https://www.notion.so/clevision/8651a74e9c344c67bd8407272d31664c?v=1f1718363a4a4823b5ba3e8afd1ae86c";
    public const string notionDbId = "337d9b78209b442185cccb11ba028dc4";
    public const string urlSlack = "https://hooks.slack.com/services/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";
    public const string labelTime = "시간";
    public const string labelLevel = "레벨";
    public const string labelContent = "내용";
    public const string timeFormat = "yyyy-MM-dd HH:mm:ss";
    public const string directoryId = "11RQP2w8HdhlB3PdkbDSjFdMn8wogqGO2";
}

