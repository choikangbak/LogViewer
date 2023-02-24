using Dapper;
using Microsoft.VisualBasic.Logging;
using Npgsql;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Net;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.VisualBasic.Devices;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LogViewer
{
    public partial class Form_Main : Form
    {
        private BackgroundWorker _backgroundWorker;
        private NameValueCollection _appSettings;
        private DatabaseConnector _databaseConnector;
        private LogDataAccess _logDataAccess;
        private List<Log> _logList;

        public Form_Main()
        {
            InitializeComponent();
            _appSettings = ConfigurationManager.AppSettings;
            _backgroundWorker = new BackgroundWorker();
            _logList = new List<Log>();
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
            this.ControlBox = isEnable;
        }

        private void Btn_InsertDbPassword_Click(object sender, EventArgs e)
        {
            string dbPassword = Tb_DbPassword.Text;
            
            string connectionString = ConfigurationManager.ConnectionStrings["Default"] + "Password=" + dbPassword + ";";

            _databaseConnector = new DatabaseConnector(connectionString);

            NpgsqlConnection connection = _databaseConnector.GetConnection();

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                connection.Close();

                Console.WriteLine($"Error: {ex.Message}");

                MessageBox.Show("비밀번호가 잘못되었습니다.", "메시지 - CLE Inc.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();

                _logDataAccess = new LogDataAccess(connection);

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
            if (!isWithinTheTimePeriod(Dtp_StartTime.Value))
            {
                MessageBox.Show("72시간 이전의 로그는 조회하실 수 없습니다.", "메시지 - CLE Inc.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!isRegularExpression(Tb_SearchLog.Text.Trim()))
            {
                MessageBox.Show("검색어에는 한글, 영어, 숫자, 공백 및 특수문자('.', '_', '-')만 포함 가능합니다.", "메시지 - CLE Inc.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                EnableControls(false);

                Pb_LoadLog.Style = ProgressBarStyle.Marquee;
                Pb_LoadLog.MarqueeAnimationSpeed = 100;

                _backgroundWorker.DoWork += Bw_GetSearchedLog;
                _backgroundWorker.RunWorkerCompleted += Bw_GetSearchedLogCompleted;
                _backgroundWorker.RunWorkerAsync();
            }
        }

        private bool isWithinTheTimePeriod(DateTime startTime)
        {
            if (startTime >= DateTime.Now.AddDays(-3))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isRegularExpression(string str)
        {
            var regex = new Regex(_appSettings["RegularExpression"]);

            if (regex.IsMatch(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Bw_GetSearchedLog(object sender, DoWorkEventArgs e)
        {
            string startTime = Dtp_StartTime.Value.ToString(_appSettings["DateTimeFormat"]);
            string endTime = Dtp_EndTime.Value.ToString(_appSettings["DateTimeFormat"]);

            List<string> levels = new List<string>();
            if (Cb_Trace.Checked) levels.Add("Trace");
            if (Cb_Debug.Checked) levels.Add("Debug");
            if (Cb_Info.Checked) levels.Add("Info");
            if (Cb_Warning.Checked) levels.Add("Warning");
            if (Cb_Error.Checked) levels.Add("Error");
            if (Cb_Critical.Checked) levels.Add("Critical");

            string keyword = Tb_SearchLog.Text.Trim();

            _logList = _logDataAccess.SearchLog(startTime, endTime, levels, keyword);
        }

        private void Bw_GetSearchedLogCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Pb_LoadLog.Style = ProgressBarStyle.Blocks;
            Pb_LoadLog.Value = 0;

            Dgv_Log.DataSource = _logList;

            Dgv_Log.Columns[0].Visible = false;
            Dgv_Log.Columns[4].Visible = false;

            Dgv_Log.Columns[1].HeaderText = _appSettings["TimestampHeaderText"];
            Dgv_Log.Columns[2].HeaderText = _appSettings["LevelHeaderText"];
            Dgv_Log.Columns[3].HeaderText = _appSettings["MessageHeaderText"];

            Dgv_Log.Columns[1].Width = 150;
            Dgv_Log.Columns[2].Width = 80;

            Dgv_Log.Columns[1].DefaultCellStyle.Format = _appSettings["DateTimeFormat"];

            EnableControls(true);
        }

        private void Btn_SearchLog_Click(object sender, EventArgs e)
        {
            SearchLog();
        }

        private List<string> GetSelectedLogs()
        {
            List<string> logsSelected = new List<string>();

            var selectedRows = Dgv_Log.SelectedRows;

            for (int i = 0; i < selectedRows.Count; i++)
            {
                var selectedRow = Dgv_Log.Rows[selectedRows[i].Index];
                
                var timestamp = selectedRow.Cells["timestamp"].Value.ToString();
                var level = selectedRow.Cells["level"].Value.ToString();
                var message = selectedRow.Cells["message"].Value.ToString();

                string log = string.Format("[{0}] [{1}] {2}", timestamp, level, message);
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

            form_IssueReporter.SetLogsSelected(logsSelected);
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