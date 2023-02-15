using Dapper;
using Npgsql;
using System.ComponentModel;
using System.Configuration;
using System.Data;

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
            conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            Dtp_StartTime.Value = DateTime.Now.AddDays(-1);
            Dtp_EndTime.Value = DateTime.Now;

            Cb_Trace.Checked= true; 
            Cb_Debug.Checked = true;
            Cb_Info.Checked = true;
            Cb_Warning.Checked = true;
            Cb_Error.Checked = true;
            Cb_Critical.Checked = true;

            Search();
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
                conn.Open();
                sql = string.Format("SELECT * FROM log WHERE {0} {1} {2} ORDER BY timestamp DESC", timeStmt, levelStmt, keywordStmt);

                // original start
                cmd = new NpgsqlCommand(sql, conn);
                //dt = new DataTable();
                //dt.Load(cmd.ExecuteReader());
                conn.Close();
                //Dgv_Log.DataSource = null;
                //Dgv_Log.DataSource = dt;
                // original end

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
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
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

        private void Cb_Off_CheckedChanged(object sender, EventArgs e)
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

        private void Dgv_Log_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;

        }

        private void Btn_CreateIssue_Click(object sender, EventArgs e)
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
        }
    }
}