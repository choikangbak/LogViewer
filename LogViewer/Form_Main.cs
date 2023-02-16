using Dapper;
using LogViewer.Repositories;
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
            Cb_Off.Checked = true;

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
            if (Cb_Off.Checked) levels.Add("O");

            string keyword = Tb_Search.Text.Trim();

            string timeStmt = GetTimeStmt(startTime, endTime);
            string keywordStmt = GetKeywordStmt(keyword);
            string levelStmt = GetLevelStmt(levels);

            try
            {
                conn.Open();
                sql = string.Format("SELECT * FROM log WHERE {0} {1} {2} ORDER BY timestamp DESC", timeStmt, levelStmt, keywordStmt);

                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                Dgv_Log.DataSource = null;
                Dgv_Log.DataSource = dt;
                Dgv_Log.Columns[0].Visible = false;
                Dgv_Log.Columns[4].Visible = false;
                DataGridViewColumn colTime = Dgv_Log.Columns["timestamp"];
                colTime.FillWeight = 20;
                DataGridViewColumn colLvl = Dgv_Log.Columns["level"];
                colLvl.FillWeight = 10;
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Btn_Search_Click(object sender, EventArgs e)
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
                return " AND level NOT IN ('T', 'D', 'I', 'W', 'E', 'C', 'O') ";
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

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                ContextMenuStrip menu = new System.Windows.Forms.ContextMenuStrip();
                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("ÀÌ½´»ý¼º");
                toolStripMenuItem.Click += ToolStripMenuItem_Click;
                menu.Items.Add(toolStripMenuItem);
                menu.Show(MousePosition);
            }
        }

        private void ToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            FormMakeIssue dlg = new FormMakeIssue();
            dlg.ShowDialog();

//            throw new NotImplementedException();
        }
    }
}