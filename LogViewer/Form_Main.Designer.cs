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
            this.Dgv_Log = new System.Windows.Forms.DataGridView();
            this.Dtp_StartTime = new System.Windows.Forms.DateTimePicker();
            this.Dtp_EndTime = new System.Windows.Forms.DateTimePicker();
            this.Lb_FromTo = new System.Windows.Forms.Label();
            this.Cb_Debug = new System.Windows.Forms.CheckBox();
            this.Cb_Info = new System.Windows.Forms.CheckBox();
            this.Cb_Warning = new System.Windows.Forms.CheckBox();
            this.Cb_Error = new System.Windows.Forms.CheckBox();
            this.Cb_Critical = new System.Windows.Forms.CheckBox();
            this.Tb_Search = new System.Windows.Forms.TextBox();
            this.Btn_Search = new System.Windows.Forms.Button();
            this.Cb_Off = new System.Windows.Forms.CheckBox();
            this.Cb_Trace = new System.Windows.Forms.CheckBox();
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
            this.Dgv_Log.Location = new System.Drawing.Point(33, 66);
            this.Dgv_Log.Name = "Dgv_Log";
            this.Dgv_Log.ReadOnly = true;
            this.Dgv_Log.RowTemplate.Height = 25;
            this.Dgv_Log.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Dgv_Log.Size = new System.Drawing.Size(1051, 512);
            this.Dgv_Log.TabIndex = 0;
            this.Dgv_Log.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.Dgv_Log_RowStateChanged);
            // 
            // Dtp_StartTime
            // 
            this.Dtp_StartTime.CustomFormat = "yyyy-MM-dd HH:mm:ss.ss";
            this.Dtp_StartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_StartTime.Location = new System.Drawing.Point(33, 24);
            this.Dtp_StartTime.Name = "Dtp_StartTime";
            this.Dtp_StartTime.Size = new System.Drawing.Size(176, 23);
            this.Dtp_StartTime.TabIndex = 1;
            this.Dtp_StartTime.Value = new System.DateTime(2023, 2, 13, 16, 51, 41, 0);
            this.Dtp_StartTime.ValueChanged += new System.EventHandler(this.Dtp_StartTime_ValueChanged);
            // 
            // Dtp_EndTime
            // 
            this.Dtp_EndTime.CustomFormat = "yyyy-MM-dd HH:mm:ss.ss";
            this.Dtp_EndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Dtp_EndTime.Location = new System.Drawing.Point(236, 24);
            this.Dtp_EndTime.Name = "Dtp_EndTime";
            this.Dtp_EndTime.Size = new System.Drawing.Size(174, 23);
            this.Dtp_EndTime.TabIndex = 2;
            this.Dtp_EndTime.ValueChanged += new System.EventHandler(this.Dtp_EndTime_ValueChanged);
            // 
            // Lb_FromTo
            // 
            this.Lb_FromTo.AutoSize = true;
            this.Lb_FromTo.Location = new System.Drawing.Point(215, 28);
            this.Lb_FromTo.Name = "Lb_FromTo";
            this.Lb_FromTo.Size = new System.Drawing.Size(15, 15);
            this.Lb_FromTo.TabIndex = 3;
            this.Lb_FromTo.Text = "~";
            // 
            // Cb_Debug
            // 
            this.Cb_Debug.AutoSize = true;
            this.Cb_Debug.Location = new System.Drawing.Point(487, 26);
            this.Cb_Debug.Name = "Cb_Debug";
            this.Cb_Debug.Size = new System.Drawing.Size(62, 19);
            this.Cb_Debug.TabIndex = 4;
            this.Cb_Debug.Text = "Debug";
            this.Cb_Debug.UseVisualStyleBackColor = true;
            this.Cb_Debug.CheckedChanged += new System.EventHandler(this.Cb_Debug_CheckedChanged);
            // 
            // Cb_Info
            // 
            this.Cb_Info.AutoSize = true;
            this.Cb_Info.Location = new System.Drawing.Point(555, 26);
            this.Cb_Info.Name = "Cb_Info";
            this.Cb_Info.Size = new System.Drawing.Size(47, 19);
            this.Cb_Info.TabIndex = 5;
            this.Cb_Info.Text = "Info";
            this.Cb_Info.UseVisualStyleBackColor = true;
            this.Cb_Info.CheckedChanged += new System.EventHandler(this.Cb_Info_CheckedChanged);
            // 
            // Cb_Warning
            // 
            this.Cb_Warning.AutoSize = true;
            this.Cb_Warning.Location = new System.Drawing.Point(608, 26);
            this.Cb_Warning.Name = "Cb_Warning";
            this.Cb_Warning.Size = new System.Drawing.Size(71, 19);
            this.Cb_Warning.TabIndex = 6;
            this.Cb_Warning.Text = "Warning";
            this.Cb_Warning.UseVisualStyleBackColor = true;
            this.Cb_Warning.CheckedChanged += new System.EventHandler(this.Cb_Warning_CheckedChanged);
            // 
            // Cb_Error
            // 
            this.Cb_Error.AutoSize = true;
            this.Cb_Error.Location = new System.Drawing.Point(685, 26);
            this.Cb_Error.Name = "Cb_Error";
            this.Cb_Error.Size = new System.Drawing.Size(51, 19);
            this.Cb_Error.TabIndex = 7;
            this.Cb_Error.Text = "Error";
            this.Cb_Error.UseVisualStyleBackColor = true;
            this.Cb_Error.CheckedChanged += new System.EventHandler(this.Cb_Error_CheckedChanged);
            // 
            // Cb_Critical
            // 
            this.Cb_Critical.AutoSize = true;
            this.Cb_Critical.Location = new System.Drawing.Point(742, 26);
            this.Cb_Critical.Name = "Cb_Critical";
            this.Cb_Critical.Size = new System.Drawing.Size(63, 19);
            this.Cb_Critical.TabIndex = 8;
            this.Cb_Critical.Text = "Critical";
            this.Cb_Critical.UseVisualStyleBackColor = true;
            this.Cb_Critical.CheckedChanged += new System.EventHandler(this.Cb_Critical_CheckedChanged);
            // 
            // Tb_Search
            // 
            this.Tb_Search.Location = new System.Drawing.Point(864, 24);
            this.Tb_Search.Name = "Tb_Search";
            this.Tb_Search.Size = new System.Drawing.Size(159, 23);
            this.Tb_Search.TabIndex = 10;
            // 
            // Btn_Search
            // 
            this.Btn_Search.Location = new System.Drawing.Point(1023, 24);
            this.Btn_Search.Name = "Btn_Search";
            this.Btn_Search.Size = new System.Drawing.Size(64, 23);
            this.Btn_Search.TabIndex = 11;
            this.Btn_Search.Text = "검색";
            this.Btn_Search.UseVisualStyleBackColor = true;
            this.Btn_Search.Click += new System.EventHandler(this.Btn_Search_Click);
            // 
            // Cb_Off
            // 
            this.Cb_Off.AutoSize = true;
            this.Cb_Off.Location = new System.Drawing.Point(807, 26);
            this.Cb_Off.Name = "Cb_Off";
            this.Cb_Off.Size = new System.Drawing.Size(43, 19);
            this.Cb_Off.TabIndex = 12;
            this.Cb_Off.Text = "Off";
            this.Cb_Off.UseVisualStyleBackColor = true;
            this.Cb_Off.CheckedChanged += new System.EventHandler(this.Cb_Off_CheckedChanged);
            // 
            // Cb_Trace
            // 
            this.Cb_Trace.AutoSize = true;
            this.Cb_Trace.Location = new System.Drawing.Point(427, 26);
            this.Cb_Trace.Name = "Cb_Trace";
            this.Cb_Trace.Size = new System.Drawing.Size(54, 19);
            this.Cb_Trace.TabIndex = 13;
            this.Cb_Trace.Text = "Trace";
            this.Cb_Trace.UseVisualStyleBackColor = true;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 609);
            this.Controls.Add(this.Cb_Trace);
            this.Controls.Add(this.Cb_Off);
            this.Controls.Add(this.Btn_Search);
            this.Controls.Add(this.Tb_Search);
            this.Controls.Add(this.Cb_Critical);
            this.Controls.Add(this.Cb_Error);
            this.Controls.Add(this.Cb_Warning);
            this.Controls.Add(this.Cb_Info);
            this.Controls.Add(this.Cb_Debug);
            this.Controls.Add(this.Lb_FromTo);
            this.Controls.Add(this.Dtp_EndTime);
            this.Controls.Add(this.Dtp_StartTime);
            this.Controls.Add(this.Dgv_Log);
            this.Name = "Form_Main";
            this.Text = "Log Viewer";
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
        private TextBox Tb_Search;
        private Button Btn_Search;
        private CheckBox Cb_Off;
        private CheckBox Cb_Trace;
    }
}