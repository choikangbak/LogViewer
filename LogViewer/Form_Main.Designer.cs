namespace LogViewer
{
    partial class Form_Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.Dgv_Log = new System.Windows.Forms.DataGridView();
            this.Dtp_StartTime = new System.Windows.Forms.DateTimePicker();
            this.Dtp_EndTime = new System.Windows.Forms.DateTimePicker();
            this.Lb_FromTo = new System.Windows.Forms.Label();
            this.Cb_Debug = new System.Windows.Forms.CheckBox();
            this.Cb_Info = new System.Windows.Forms.CheckBox();
            this.Cb_Warning = new System.Windows.Forms.CheckBox();
            this.Cb_Error = new System.Windows.Forms.CheckBox();
            this.Cb_Critical = new System.Windows.Forms.CheckBox();
            this.Tb_SearchLog = new System.Windows.Forms.TextBox();
            this.Btn_SearchLog = new System.Windows.Forms.Button();
            this.Cb_Trace = new System.Windows.Forms.CheckBox();
            this.Tb_DbPassword = new System.Windows.Forms.TextBox();
            this.Btn_InsertDbPassword = new System.Windows.Forms.Button();
            this.Pb_LoadLog = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_Log)).BeginInit();
            this.SuspendLayout();
            // 
            // Dgv_Log
            // 
            this.Dgv_Log.AllowUserToAddRows = false;
            this.Dgv_Log.AllowUserToDeleteRows = false;
            this.Dgv_Log.AllowUserToOrderColumns = true;
            this.Dgv_Log.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Dgv_Log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgv_Log.Enabled = false;
            this.Dgv_Log.Location = new System.Drawing.Point(25, 75);
            this.Dgv_Log.Margin = new System.Windows.Forms.Padding(2);
            this.Dgv_Log.Name = "Dgv_Log";
            this.Dgv_Log.ReadOnly = true;
            this.Dgv_Log.RowHeadersWidth = 51;
            this.Dgv_Log.RowTemplate.Height = 25;
            this.Dgv_Log.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Dgv_Log.Size = new System.Drawing.Size(971, 522);
            this.Dgv_Log.TabIndex = 0;
            this.Dgv_Log.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Dgv_Log_MouseUp);
            // 
            // Dtp_StartTime
            // 
            this.Dtp_StartTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.Dtp_StartTime.Enabled = false;
            this.Dtp_StartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_StartTime.Location = new System.Drawing.Point(25, 43);
            this.Dtp_StartTime.Margin = new System.Windows.Forms.Padding(2);
            this.Dtp_StartTime.Name = "Dtp_StartTime";
            this.Dtp_StartTime.Size = new System.Drawing.Size(176, 23);
            this.Dtp_StartTime.TabIndex = 1;
            this.Dtp_StartTime.Value = new System.DateTime(2023, 2, 21, 0, 0, 0, 0);
            // 
            // Dtp_EndTime
            // 
            this.Dtp_EndTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.Dtp_EndTime.Enabled = false;
            this.Dtp_EndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_EndTime.Location = new System.Drawing.Point(225, 43);
            this.Dtp_EndTime.Margin = new System.Windows.Forms.Padding(2);
            this.Dtp_EndTime.Name = "Dtp_EndTime";
            this.Dtp_EndTime.Size = new System.Drawing.Size(174, 23);
            this.Dtp_EndTime.TabIndex = 2;
            this.Dtp_EndTime.Value = new System.DateTime(2023, 2, 21, 0, 0, 0, 0);
            // 
            // Lb_FromTo
            // 
            this.Lb_FromTo.AutoSize = true;
            this.Lb_FromTo.Location = new System.Drawing.Point(205, 46);
            this.Lb_FromTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_FromTo.Name = "Lb_FromTo";
            this.Lb_FromTo.Size = new System.Drawing.Size(15, 15);
            this.Lb_FromTo.TabIndex = 3;
            this.Lb_FromTo.Text = "~";
            // 
            // Cb_Debug
            // 
            this.Cb_Debug.AutoSize = true;
            this.Cb_Debug.Checked = true;
            this.Cb_Debug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Cb_Debug.Enabled = false;
            this.Cb_Debug.Location = new System.Drawing.Point(470, 45);
            this.Cb_Debug.Margin = new System.Windows.Forms.Padding(2);
            this.Cb_Debug.Name = "Cb_Debug";
            this.Cb_Debug.Size = new System.Drawing.Size(62, 19);
            this.Cb_Debug.TabIndex = 4;
            this.Cb_Debug.Text = "Debug";
            this.Cb_Debug.UseVisualStyleBackColor = true;
            // 
            // Cb_Info
            // 
            this.Cb_Info.AutoSize = true;
            this.Cb_Info.Checked = true;
            this.Cb_Info.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Cb_Info.Enabled = false;
            this.Cb_Info.Location = new System.Drawing.Point(534, 45);
            this.Cb_Info.Margin = new System.Windows.Forms.Padding(2);
            this.Cb_Info.Name = "Cb_Info";
            this.Cb_Info.Size = new System.Drawing.Size(47, 19);
            this.Cb_Info.TabIndex = 5;
            this.Cb_Info.Text = "Info";
            this.Cb_Info.UseVisualStyleBackColor = true;
            // 
            // Cb_Warning
            // 
            this.Cb_Warning.AutoSize = true;
            this.Cb_Warning.Checked = true;
            this.Cb_Warning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Cb_Warning.Enabled = false;
            this.Cb_Warning.Location = new System.Drawing.Point(584, 45);
            this.Cb_Warning.Margin = new System.Windows.Forms.Padding(2);
            this.Cb_Warning.Name = "Cb_Warning";
            this.Cb_Warning.Size = new System.Drawing.Size(71, 19);
            this.Cb_Warning.TabIndex = 6;
            this.Cb_Warning.Text = "Warning";
            this.Cb_Warning.UseVisualStyleBackColor = true;
            // 
            // Cb_Error
            // 
            this.Cb_Error.AutoSize = true;
            this.Cb_Error.Checked = true;
            this.Cb_Error.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Cb_Error.Enabled = false;
            this.Cb_Error.Location = new System.Drawing.Point(658, 45);
            this.Cb_Error.Margin = new System.Windows.Forms.Padding(2);
            this.Cb_Error.Name = "Cb_Error";
            this.Cb_Error.Size = new System.Drawing.Size(51, 19);
            this.Cb_Error.TabIndex = 7;
            this.Cb_Error.Text = "Error";
            this.Cb_Error.UseVisualStyleBackColor = true;
            // 
            // Cb_Critical
            // 
            this.Cb_Critical.AutoSize = true;
            this.Cb_Critical.Checked = true;
            this.Cb_Critical.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Cb_Critical.Enabled = false;
            this.Cb_Critical.Location = new System.Drawing.Point(711, 45);
            this.Cb_Critical.Margin = new System.Windows.Forms.Padding(2);
            this.Cb_Critical.Name = "Cb_Critical";
            this.Cb_Critical.Size = new System.Drawing.Size(63, 19);
            this.Cb_Critical.TabIndex = 8;
            this.Cb_Critical.Text = "Critical";
            this.Cb_Critical.UseVisualStyleBackColor = true;
            // 
            // Tb_SearchLog
            // 
            this.Tb_SearchLog.Enabled = false;
            this.Tb_SearchLog.Location = new System.Drawing.Point(777, 42);
            this.Tb_SearchLog.Margin = new System.Windows.Forms.Padding(2);
            this.Tb_SearchLog.Name = "Tb_SearchLog";
            this.Tb_SearchLog.Size = new System.Drawing.Size(163, 23);
            this.Tb_SearchLog.TabIndex = 10;
            this.Tb_SearchLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tb_SearchLog_KeyDown);
            // 
            // Btn_SearchLog
            // 
            this.Btn_SearchLog.Enabled = false;
            this.Btn_SearchLog.Location = new System.Drawing.Point(940, 42);
            this.Btn_SearchLog.Margin = new System.Windows.Forms.Padding(2);
            this.Btn_SearchLog.Name = "Btn_SearchLog";
            this.Btn_SearchLog.Size = new System.Drawing.Size(50, 23);
            this.Btn_SearchLog.TabIndex = 11;
            this.Btn_SearchLog.Text = "검색";
            this.Btn_SearchLog.UseVisualStyleBackColor = true;
            this.Btn_SearchLog.Click += new System.EventHandler(this.Btn_SearchLog_Click);
            // 
            // Cb_Trace
            // 
            this.Cb_Trace.AutoSize = true;
            this.Cb_Trace.Checked = true;
            this.Cb_Trace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Cb_Trace.Enabled = false;
            this.Cb_Trace.Location = new System.Drawing.Point(413, 45);
            this.Cb_Trace.Margin = new System.Windows.Forms.Padding(2);
            this.Cb_Trace.Name = "Cb_Trace";
            this.Cb_Trace.Size = new System.Drawing.Size(54, 19);
            this.Cb_Trace.TabIndex = 13;
            this.Cb_Trace.Text = "Trace";
            this.Cb_Trace.UseVisualStyleBackColor = true;
            // 
            // Tb_DbPassword
            // 
            this.Tb_DbPassword.Location = new System.Drawing.Point(25, 14);
            this.Tb_DbPassword.Margin = new System.Windows.Forms.Padding(2);
            this.Tb_DbPassword.Name = "Tb_DbPassword";
            this.Tb_DbPassword.PasswordChar = '*';
            this.Tb_DbPassword.Size = new System.Drawing.Size(163, 23);
            this.Tb_DbPassword.TabIndex = 15;
            this.Tb_DbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tb_DbPassword_KeyDown);
            // 
            // Btn_InsertDbPassword
            // 
            this.Btn_InsertDbPassword.Location = new System.Drawing.Point(192, 14);
            this.Btn_InsertDbPassword.Margin = new System.Windows.Forms.Padding(2);
            this.Btn_InsertDbPassword.Name = "Btn_InsertDbPassword";
            this.Btn_InsertDbPassword.Size = new System.Drawing.Size(94, 24);
            this.Btn_InsertDbPassword.TabIndex = 16;
            this.Btn_InsertDbPassword.Text = "비밀번호 입력";
            this.Btn_InsertDbPassword.UseVisualStyleBackColor = true;
            this.Btn_InsertDbPassword.Click += new System.EventHandler(this.Btn_InsertDbPassword_Click);
            // 
            // Pb_LoadLog
            // 
            this.Pb_LoadLog.Location = new System.Drawing.Point(856, 603);
            this.Pb_LoadLog.MarqueeAnimationSpeed = 0;
            this.Pb_LoadLog.Name = "Pb_LoadLog";
            this.Pb_LoadLog.Size = new System.Drawing.Size(140, 22);
            this.Pb_LoadLog.Step = 1;
            this.Pb_LoadLog.TabIndex = 17;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 637);
            this.Controls.Add(this.Pb_LoadLog);
            this.Controls.Add(this.Btn_InsertDbPassword);
            this.Controls.Add(this.Tb_DbPassword);
            this.Controls.Add(this.Cb_Trace);
            this.Controls.Add(this.Btn_SearchLog);
            this.Controls.Add(this.Tb_SearchLog);
            this.Controls.Add(this.Cb_Critical);
            this.Controls.Add(this.Cb_Error);
            this.Controls.Add(this.Cb_Warning);
            this.Controls.Add(this.Cb_Info);
            this.Controls.Add(this.Cb_Debug);
            this.Controls.Add(this.Lb_FromTo);
            this.Controls.Add(this.Dtp_EndTime);
            this.Controls.Add(this.Dtp_StartTime);
            this.Controls.Add(this.Dgv_Log);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1036, 676);
            this.MinimumSize = new System.Drawing.Size(1036, 676);
            this.Name = "Form_Main";
            this.Text = "로그 뷰어 - CLE Inc.";
            this.Load += new System.EventHandler(this.Form_Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_Log)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView Dgv_Log;
        private DateTimePicker Dtp_StartTime;
        private DateTimePicker Dtp_EndTime;
        private Label Lb_FromTo;
        private CheckBox Cb_Debug;
        private CheckBox Cb_Info;
        private CheckBox Cb_Warning;
        private CheckBox Cb_Error;
        private CheckBox Cb_Critical;
        private TextBox Tb_SearchLog;
        private Button Btn_SearchLog;
        private CheckBox Cb_Trace;
        private DataGridViewTextBoxColumn createdAtDataGridViewTextBoxColumn;
        private TextBox Tb_DbPassword;
        private Button Btn_InsertDbPassword;
        private ProgressBar Pb_LoadLog;
    }
}