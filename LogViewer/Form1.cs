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
    public partial class FormMakeIssue : Form
    {
        public FormMakeIssue()
        {
            InitializeComponent();
        }

        private void btnAttachFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files | *.*";
            dlg.ShowDialog();
            if(dlg.FileName.Length > 0)
            {
                foreach(string s in dlg.FileNames)
                {
                    this.textBoxAttachedFile.Text = s;
                }
            }
        }

        private List<string> m_logs;

        public void setLogs(List<string> logs)
        {
            m_logs = logs;

            this.textBoxLogs.Text = "";
            foreach (string log in logs)
            {
                this.textBoxLogs.Text += log + "\r\n";
            }
        }

        private void sendIssue2Notion(string issueTitle, string issueContent)
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
            string title = "\"Title\":{\"title\":[{\"text\":{\"content\":\"" + issueTitle + "\"}}]}";
            string content = "\"Content\":{\"type\":\"rich_text\",\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + issueContent + "\"}}]}";
            string properties = "\"properties\":{" + title + "," + content + "}";
            string child_heading2_log = "{\"object\":\"block\",\"type\":\"heading_2\",\"heading_2\":{\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + "관련된 로그(들)" + "\"}}]}}";
            string children = "\"children\":[" + child_heading2_log + ",";
            for (int i = 0; i < m_logs.Count; i++)
            {
                var logSelected = m_logs[i];
                children += "{\"object\":\"block\",\"type\":\"bulleted_list_item\",\"bulleted_list_item\":{\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + logSelected + "\"}}]}}";

                if (i < m_logs.Count - 1) children += ",";
                if (i == m_logs.Count - 1) children += "]";
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

        private void btnMakeIssue_Click(object sender, EventArgs e)
        {
            string title = this.textBoxIssueTitle.Text;
            string content = this.textBoxContents.Text;

            //sendIssue2Notion(title, content);
            slackSendMessage(Constants.urlSlack, slackMessage(title, content));

            this.Close();
        }

        private void slackSendMessage(string URL, string message)
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

        private string slackMessage(string title, string contents, string urlNotion=Constants.urlIssueNotion)
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
}

