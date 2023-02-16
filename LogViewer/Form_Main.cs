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

            DisableControls();
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
                Console.WriteLine("logList.Count: " + logList.Count); // later to be deleted

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
                Dgv_Log.Columns["timestamp"].Width = 170;
                Dgv_Log.Columns["level"].Width = 80;

                // timestamp format
                Dgv_Log.Columns["timestamp"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss.ss";

                for (int i = 0; i < Dgv_Log.Rows.Count; i++)
                {
                    string levelValue = Dgv_Log.Rows[i].Cells["level"].Value.ToString();
                    Dgv_Log.Rows[i].Cells["level"].Value = getFullLevel(levelValue);

                }
            }
            catch (Exception ex) 
            {
                conn.Close();
                Console.WriteLine("Error: " + ex.Message);
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

                Cb_Trace.Checked = true;
                Cb_Debug.Checked = true;
                Cb_Info.Checked = true;
                Cb_Warning.Checked = true;
                Cb_Error.Checked = true;
                Cb_Critical.Checked = true;

                Dtp_StartTime.Value = DateTime.Now.AddDays(-1);
                Dtp_EndTime.Value = DateTime.Now;

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

        private string getFullLevel(string s)
        {
            if (s == "T") return "Trace";
            else if (s == "D") return "Debug";
            else if (s == "I") return "Info";
            else if (s == "W") return "Warning";
            else if (s == "E") return "Error";
            else if (s == "C") return "Critical";
            else return "????";
        }

        private List<string> getSelectedLogs()
        {
            List<string> logs = new List<string>();
            
            for (int i = 0; i < Dgv_Log.Rows.Count; i++)
            {
                var row = Dgv_Log.Rows[i];
                if (row.Selected)
                {
                    string time = row.Cells["timestamp"].Value.ToString();
                    string level = row.Cells["level"].Value.ToString();
                    string msg = row.Cells["message"].Value.ToString();
                    string log = string.Format("[{0}] [{1}] {2}", time, level, msg);
                    logs.Add(log);
                }
            }
            //foreach(string log in logs) Console.WriteLine(log);
            return logs;
        }

        private void ToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            FormMakeIssue dlg = new FormMakeIssue();
            dlg.setLogs(getSelectedLogs());
            dlg.ShowDialog();
//            throw new NotImplementedException();
        }

        private void Dgv_Log_Scroll(object sender, ScrollEventArgs e)
        {
            Console.WriteLine("===================================");
            Console.WriteLine("{0} = {1}", "ScrollOrientation", e.ScrollOrientation);
            Console.WriteLine("{0} = {1}", "Type", e.Type);
            Console.WriteLine("{0} = {1}", "NewValue", e.NewValue);
            Console.WriteLine("{0} = {1}", "OldValue", e.OldValue);
            Console.WriteLine("Dgv_Log.Rows.Count: " + Dgv_Log.Rows.Count);
            Console.WriteLine("Dgv_Log.FirstDisplayedScrollingRowIndex: " + Dgv_Log.FirstDisplayedScrollingRowIndex);
            Console.WriteLine("Dgv_Log.RowCount: " + Dgv_Log.RowCount);
            Console.WriteLine("Hit the bottom?: " + (Dgv_Log.FirstDisplayedScrollingRowIndex == Dgv_Log.RowCount-1));
            Console.WriteLine("===================================");
        }

        private void Dgv_Log_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip menu = new System.Windows.Forms.ContextMenuStrip();
                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("이슈생성");
                toolStripMenuItem.Click += ToolStripMenuItem_Click;
                menu.Items.Add(toolStripMenuItem);
                menu.Show(MousePosition);
            }
        }
    }
}