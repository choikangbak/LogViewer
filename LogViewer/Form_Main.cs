using Dapper;
using Microsoft.VisualBasic.Logging;
using Npgsql;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Net;
using System.Text.Json.Nodes;

namespace LogViewer
{
    public partial class Form_Main : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand cmd;
        DataTable dt;
        string sql;

        public Form_Main()
        {
            InitializeComponent();

            Dtp_StartTime.Enabled = false;
            Dtp_EndTime.Enabled = false;
            Cb_Trace.Enabled = false;
            Cb_Debug.Enabled = false;
            Cb_Info.Enabled = false;
            Cb_Warning.Enabled = false;
            Cb_Error.Enabled = false;
            Cb_Critical.Enabled = false;
            Tb_Search.Enabled = false;
            Btn_SearchLog.Enabled = false;
            Btn_CreateIssue.Enabled = false;
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                Search();
            }
        }

        void DisableControls()
        {
            Dtp_StartTime.Enabled = false;
            Dtp_EndTime.Enabled = false;
            Cb_Trace.Enabled = false;
            Cb_Debug.Enabled = false;
            Cb_Info.Enabled = false;
            Cb_Warning.Enabled = false;
            Cb_Error.Enabled = false;
            Cb_Critical.Enabled = false;
            Tb_Search.Enabled = false;
            Btn_SearchLog.Enabled = false;
            Dgv_Log.Enabled = false;

            Btn_CreateIssue.Enabled = false; // later to be deleted
        }

        void EnableControls()
        {
            Dtp_StartTime.Enabled = true;
            Dtp_EndTime.Enabled = true;
            Cb_Trace.Enabled = true;
            Cb_Debug.Enabled = true;
            Cb_Info.Enabled = true;
            Cb_Warning.Enabled = true;
            Cb_Error.Enabled = true;
            Cb_Critical.Enabled = true;
            Tb_Search.Enabled = true;
            Btn_SearchLog.Enabled = true;
            Dgv_Log.Enabled = true;

            Btn_CreateIssue.Enabled = true; // later to be deleted
        }

        private void Search()
        {
            string startTime = Dtp_StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss.ss");
            string endTime = Dtp_EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss.ss");

            List<string> levels = new List<string>();
            if (Cb_Trace.Checked) levels.Add("T");
            if (Cb_Debug.Checked) levels.Add("D");
            if (Cb_Info.Checked) levels.Add("I");
            if (Cb_Warning.Checked) levels.Add("W");
            if (Cb_Error.Checked) levels.Add("E");
            if (Cb_Critical.Checked) levels.Add("C");

            string keyword = Tb_Search.Text.Trim();

            string timeStmt = GetTimeStmt(startTime, endTime);
            string keywordStmt = GetKeywordStmt(keyword);
            string levelStmt = GetLevelStmt(levels);

            try
            {

                /* 
                conn.Open();
                sql = string.Format("SELECT * FROM log WHERE {0} {1} {2} ORDER BY timestamp DESC", timeStmt, levelStmt, keywordStmt);
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                Dgv_Log.DataSource = null;
                Dgv_Log.DataSource = dt;
                */

                conn.Open();
                sql = string.Format("SELECT * FROM log WHERE {0} {1} {2} ORDER BY timestamp DESC", timeStmt, levelStmt, keywordStmt);
                cmd = new NpgsqlCommand(sql, conn);
                conn.Close();

                // Dapper ORM
                List<Log> logList = conn.Query<Log>(sql).ToList();

                // binding
                var bindingList = new BindingList<Log>(logList);
                var source = new BindingSource(bindingList, null);
                Dgv_Log.DataSource = source;

                // visibility
                Dgv_Log.Columns["id"].Visible = false;
                Dgv_Log.Columns["created_at"].Visible = false;

                // header text
                Dgv_Log.Columns["timestamp"].HeaderText = "시간";
                Dgv_Log.Columns["level"].HeaderText = "레벨";
                Dgv_Log.Columns["message"].HeaderText = "내용";

                // width
                Dgv_Log.Columns["timestamp"].Width = 150;
                Dgv_Log.Columns["level"].Width = 80;

                for (int i = 0; i < Dgv_Log.Rows.Count; i++)
                {
                    string levelValue = Dgv_Log.Rows[i].Cells["level"].Value.ToString();
                    if (levelValue == "T") Dgv_Log.Rows[i].Cells["level"].Value = "Trace";
                    if (levelValue == "D") Dgv_Log.Rows[i].Cells["level"].Value = "Debug";
                    if (levelValue == "I") Dgv_Log.Rows[i].Cells["level"].Value = "Info";
                    if (levelValue == "W") Dgv_Log.Rows[i].Cells["level"].Value = "Warning";
                    if (levelValue == "E") Dgv_Log.Rows[i].Cells["level"].Value = "Error";
                    if (levelValue == "C") Dgv_Log.Rows[i].Cells["level"].Value = "Critical";
                }
            }
            catch (Exception ex) // deal with this later
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Btn_InsertDbPassword_Click(object sender, EventArgs e)
        {
            string password = Tb_DbPassword.Text;
            string connectionString = string.Format("Host=localhost;Port=5432;Username=postgres;Password={0};Database=postgres;", password);
            conn = new NpgsqlConnection(connectionString);

            try
            {
                conn.Open();
            } 
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: 비밀번호가 잘못되었습니다.");
            }

            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close() ;
                EnableControls();

                // Password textbox and tutton deactivate
                Tb_DbPassword.Enabled = false;
                Btn_InsertDbPassword.Enabled = false;

                Dtp_StartTime.Value = DateTime.Now.AddDays(-1);
                Dtp_EndTime.Value = DateTime.Now;

                Cb_Trace.Checked = true;
                Cb_Debug.Checked = true;
                Cb_Info.Checked = true;
                Cb_Warning.Checked = true;
                Cb_Error.Checked = true;
                Cb_Critical.Checked = true;

                Search();
            }
        }

        private void Btn_SearchLog_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Dtp_StartTime_ValueChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void Dtp_EndTime_ValueChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void Cb_Debug_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void Cb_Trace_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void Cb_Info_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void Cb_Warning_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void Cb_Error_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void Cb_Critical_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }

        private string GetTimeStmt(string startTime, string endTime)
        {
            string timeStmt = string.Format(" ( timestamp >= '{0}' AND timestamp <= '{1}' ) ", startTime, endTime);

            return timeStmt;
        }

        private string GetKeywordStmt(string keyword)
        {
            string keywordStmt = string.Format(" AND ( message LIKE '%{0}%' ) ", keyword);

            return keywordStmt;
        }

        private string GetLevelStmt(List<string> levels)
        {
            int n = levels.Count;

            if (n == 0)
            {
                return " AND level NOT IN ('T', 'D', 'I', 'W', 'E', 'C') ";
            }

            string levelStmt = " AND ( ";
            for (int i = 0; i < n; i++)
            {
                string level = levels[i];

                levelStmt += string.Format(" level = '{0}' ", level);

                if (i < n - 1) levelStmt += " OR ";
            }
            levelStmt += " ) ";

            return levelStmt;
        }

        private void Btn_CreateIssue_Click(object sender, EventArgs e) // later to be deleted
        {
            List<string> logsSelected = new List<string>();
            for (int i = 0; i < Dgv_Log.Rows.Count; i++)
            {
                var row = Dgv_Log.Rows[i];
                if (row.Selected)
                {
                    string timestamp = row.Cells["timestamp"].Value.ToString();
                    string level = row.Cells["level"].Value.ToString();
                    string message = row.Cells["message"].Value.ToString();

                    if (level == "T") level = "Trace";
                    if (level == "D") level = "Debug";
                    if (level == "I") level = "Info";
                    if (level == "W") level = "Warning";
                    if (level == "E") level = "Error";
                    if (level == "C") level = "Critical";

                    string log = string.Format("[{0}] [{1}] {2}", timestamp, level, message);
                    logsSelected.Add(log);
                }
            }
            foreach (string log in logsSelected)
            {
                Console.WriteLine(log);
            }
            // TEST
            SendIssue("제목1", "내용1", logsSelected);
        }

        private void SendIssue(string issueTitle, string issueContent, List<string> logsSelected)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.notion.com/v1/pages");
            const string authToken = "secret_0dhSZOLiOJagsBigLH9ok1C0PlBaAGesjCdNOBcCoOp";
            const string notionVersion = "2022-02-22";
            httpWebRequest.Headers.Add("Authorization", authToken);
            httpWebRequest.Headers.Add("Notion-Version", notionVersion);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            const string databaseId = "337d9b78209b442185cccb11ba028dc4";

            string parent = "\"parent\":{\"database_id\":\"" + databaseId + "\"}";
            string icon = "\"icon\":{\"emoji\":\"📢\"}";
            string title = "\"Title\":{\"title\":[{\"text\":{\"content\":\"" + issueTitle + "\"}}]}";
            string content = "\"Content\":{\"type\":\"rich_text\",\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + issueContent + "\"}}]}";
            string properties = "\"properties\":{" + title + "," + content + "}";
            string child_heading2_log = "{\"object\":\"block\",\"type\":\"heading_2\",\"heading_2\":{\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + "관련된 로그(들)" + "\"}}]}}";
            string children = "\"children\":[" + child_heading2_log + ",";
            for (int i = 0; i < logsSelected.Count; i++)
            {
                var logSelected = logsSelected[i];
                children += "{\"object\":\"block\",\"type\":\"bulleted_list_item\",\"bulleted_list_item\":{\"rich_text\":[{\"type\":\"text\",\"text\":{\"content\":\"" + logSelected + "\"}}]}}";

                if (i < logsSelected.Count - 1) children += ",";
                if (i == logsSelected.Count - 1) children += "]";
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
    }
}