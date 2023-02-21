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
        private DatabaseConnector databaseConnector;
        private LogDataAccess logDataAccess;

        public Form_Main()
        {
            InitializeComponent();
        }

        private void Form_Main_Load(object sender, EventArgs e) { }

        private void EnableControls(bool isEnable)
        {
            Dtp_StartTime.Enabled = isEnable;
            Dtp_EndTime.Enabled = isEnable;
            Cb_Trace.Enabled = isEnable;
            Cb_Debug.Enabled = isEnable;
            Cb_Info.Enabled = isEnable;
            Cb_Warning.Enabled = isEnable;
            Cb_Error.Enabled = isEnable;
            Cb_Critical.Enabled = isEnable;
            Tb_SearchLog.Enabled = isEnable;
            Btn_SearchLog.Enabled = isEnable;
            Dgv_Log.Enabled = isEnable;
        }

        private void Btn_InsertDbPassword_Click(object sender, EventArgs e)
        {
            string dbPassword = Tb_DbPassword.Text;
            
            string connectionString = string.Format("Host=localhost;Port=5432;Username=postgres;Password={0};Database=postgres;", dbPassword); // remove this later

            databaseConnector = new DatabaseConnector(connectionString);

            NpgsqlConnection connection = databaseConnector.GetConnection();

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                connection.Close();

                Console.WriteLine("Error: "+ex.Message);

                MessageBox.Show("Error: 비밀번호가 잘못되었습니다.");
            }

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();

                logDataAccess = new LogDataAccess(connection);

                Tb_DbPassword.Enabled = false;
                Btn_InsertDbPassword.Enabled = false;

                EnableControls(true);

                Dtp_StartTime.Value = DateTime.Now.AddDays(-1);
                Dtp_EndTime.Value = DateTime.Now;

                SearchLog();
            }
        }

        private void SearchLog()
        {
            EnableControls(false);

            string startTime = Dtp_StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = Dtp_EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss");

            List<string> levels = new List<string>();
            if (Cb_Trace.Checked) levels.Add("T");
            if (Cb_Debug.Checked) levels.Add("D");
            if (Cb_Info.Checked) levels.Add("I");
            if (Cb_Warning.Checked) levels.Add("W");
            if (Cb_Error.Checked) levels.Add("E");
            if (Cb_Critical.Checked) levels.Add("C");

            string keyword = Tb_SearchLog.Text.Trim();

            List<Log> logList = logDataAccess.SearchLog(startTime, endTime, levels, keyword);

            DataTable logTable = GetLogTable(logList);

            Dgv_Log.DataSource = logTable;

            Dgv_Log.Columns["시간"].Width = 170;
            Dgv_Log.Columns["레벨"].Width = 80;
            Dgv_Log.Columns["시간"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";

            EnableControls(true);
        }

        private DataTable GetLogTable(List<Log> logList)
        {
            DataTable logTable = new DataTable();
            logTable.Columns.Add("시간", typeof(DateTime));
            logTable.Columns.Add("레벨", typeof(String));
            logTable.Columns.Add("내용", typeof(String));

            Pb_LoadLog.Maximum = logList.Count;
            for (int i = 0; i < logList.Count; i++)
            {
                DataRow dr = logTable.NewRow();
                dr["시간"] = logList[i].Timestamp;
                dr["레벨"] = GetFullLevel(logList[i].Level);
                dr["내용"] = logList[i].Message;
                logTable.Rows.Add(dr);

                Pb_LoadLog.Value = i;
            }
            Pb_LoadLog.Value = 0;

            return logTable;
        }

        private void Btn_SearchLog_Click(object sender, EventArgs e)
        {
            SearchLog();
        }

        private string GetFullLevel(string level)
        {
            if (level == "T") return "Trace";
            else if (level == "D") return "Debug";
            else if (level == "I") return "Info";
            else if (level == "W") return "Warning";
            else if (level == "E") return "Error";
            else if (level == "C") return "Critical";
            else return "N/A";
        }

        private List<string> GetSelectedLogs()
        {
            List<string> logsSelected = new List<string>();

            var selectedRows = Dgv_Log.SelectedRows;

            for (int i = 0; i < selectedRows.Count; i++)
            {
                var selectedRow = Dgv_Log.Rows[selectedRows[i].Index];

                var time = selectedRow.Cells["시간"].Value.ToString();
                var level = selectedRow.Cells["레벨"].Value.ToString();
                var message = selectedRow.Cells["내용"].Value.ToString();

                string log = string.Format("[{0}] [{1}] {2}", time, level, message);
                logsSelected.Add(log);
            }

            return logsSelected;
        }

        private void Tb_DbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Btn_InsertDbPassword_Click(sender, e);
            }
        }

        private void Tb_SearchLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Btn_SearchLog_Click(sender, e);
            }
        }

        private void Tsmi_ReportIssue_Click(object sender, EventArgs e)
        {
            Form_IssueReporter form_IssueReporter = new Form_IssueReporter();

            List<string> logsSelected = GetSelectedLogs();

            form_IssueReporter.SetLogs(logsSelected);
            form_IssueReporter.ShowDialog();
        }

        private void Dgv_Log_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("이슈 생성");

                toolStripMenuItem.Click += Tsmi_ReportIssue_Click;
                contextMenuStrip.Items.Add(toolStripMenuItem);
                contextMenuStrip.Show(MousePosition);
            }
        }
    }
}