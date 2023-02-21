namespace LogViewer
{
    partial class Form_IssueReporter
    {
        private System.ComponentModel.IContainer components = null;

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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_IssueReporter));
            this.btnMakeIssue = new System.Windows.Forms.Button();
            this.Tb_IssueTitle = new System.Windows.Forms.TextBox();
            this.Lb_IssueTitle = new System.Windows.Forms.Label();
            this.Lb_IssueContent = new System.Windows.Forms.Label();
            this.Tb_IssueContent = new System.Windows.Forms.TextBox();
            this.Lb_IssueLog = new System.Windows.Forms.Label();
            this.Tb_IssueLog = new System.Windows.Forms.TextBox();
            this.Lb_IssueAttachment = new System.Windows.Forms.Label();
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.Tb_Attachment = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Btn_AttachFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMakeIssue
            // 
            this.btnMakeIssue.Location = new System.Drawing.Point(194, 445);
            this.btnMakeIssue.Margin = new System.Windows.Forms.Padding(2);
            this.btnMakeIssue.Name = "btnMakeIssue";
            this.btnMakeIssue.Size = new System.Drawing.Size(83, 24);
            this.btnMakeIssue.TabIndex = 0;
            this.btnMakeIssue.Text = "이슈 전송";
            this.btnMakeIssue.UseVisualStyleBackColor = true;
            this.btnMakeIssue.Click += new System.EventHandler(this.Btn_ReportIssue_Click);
            // 
            // Tb_IssueTitle
            // 
            this.Tb_IssueTitle.Location = new System.Drawing.Point(70, 23);
            this.Tb_IssueTitle.Margin = new System.Windows.Forms.Padding(2);
            this.Tb_IssueTitle.Name = "Tb_IssueTitle";
            this.Tb_IssueTitle.Size = new System.Drawing.Size(465, 23);
            this.Tb_IssueTitle.TabIndex = 1;
            // 
            // Lb_IssueTitle
            // 
            this.Lb_IssueTitle.AutoSize = true;
            this.Lb_IssueTitle.Location = new System.Drawing.Point(29, 26);
            this.Lb_IssueTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_IssueTitle.Name = "Lb_IssueTitle";
            this.Lb_IssueTitle.Size = new System.Drawing.Size(31, 15);
            this.Lb_IssueTitle.TabIndex = 2;
            this.Lb_IssueTitle.Text = "제목";
            // 
            // Lb_IssueContent
            // 
            this.Lb_IssueContent.AutoSize = true;
            this.Lb_IssueContent.Location = new System.Drawing.Point(29, 64);
            this.Lb_IssueContent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_IssueContent.Name = "Lb_IssueContent";
            this.Lb_IssueContent.Size = new System.Drawing.Size(31, 15);
            this.Lb_IssueContent.TabIndex = 3;
            this.Lb_IssueContent.Text = "내용";
            // 
            // Tb_IssueContent
            // 
            this.Tb_IssueContent.Location = new System.Drawing.Point(70, 64);
            this.Tb_IssueContent.Margin = new System.Windows.Forms.Padding(2);
            this.Tb_IssueContent.Multiline = true;
            this.Tb_IssueContent.Name = "Tb_IssueContent";
            this.Tb_IssueContent.Size = new System.Drawing.Size(465, 174);
            this.Tb_IssueContent.TabIndex = 4;
            // 
            // Lb_IssueLog
            // 
            this.Lb_IssueLog.AutoSize = true;
            this.Lb_IssueLog.Location = new System.Drawing.Point(29, 254);
            this.Lb_IssueLog.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_IssueLog.Name = "Lb_IssueLog";
            this.Lb_IssueLog.Size = new System.Drawing.Size(31, 15);
            this.Lb_IssueLog.TabIndex = 5;
            this.Lb_IssueLog.Text = "로그";
            // 
            // Tb_IssueLog
            // 
            this.Tb_IssueLog.Location = new System.Drawing.Point(70, 254);
            this.Tb_IssueLog.Margin = new System.Windows.Forms.Padding(2);
            this.Tb_IssueLog.Multiline = true;
            this.Tb_IssueLog.Name = "Tb_IssueLog";
            this.Tb_IssueLog.ReadOnly = true;
            this.Tb_IssueLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Tb_IssueLog.Size = new System.Drawing.Size(465, 138);
            this.Tb_IssueLog.TabIndex = 6;
            this.Tb_IssueLog.WordWrap = false;
            // 
            // Lb_IssueAttachment
            // 
            this.Lb_IssueAttachment.AutoSize = true;
            this.Lb_IssueAttachment.Location = new System.Drawing.Point(29, 409);
            this.Lb_IssueAttachment.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_IssueAttachment.Name = "Lb_IssueAttachment";
            this.Lb_IssueAttachment.Size = new System.Drawing.Size(31, 15);
            this.Lb_IssueAttachment.TabIndex = 7;
            this.Lb_IssueAttachment.Text = "첨부";
            // 
            // openFileDlg
            // 
            this.openFileDlg.FileName = "openFileDlg";
            // 
            // Tb_Attachment
            // 
            this.Tb_Attachment.Location = new System.Drawing.Point(70, 405);
            this.Tb_Attachment.Margin = new System.Windows.Forms.Padding(2);
            this.Tb_Attachment.Multiline = true;
            this.Tb_Attachment.Name = "Tb_Attachment";
            this.Tb_Attachment.ReadOnly = true;
            this.Tb_Attachment.Size = new System.Drawing.Size(423, 24);
            this.Tb_Attachment.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(299, 445);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 24);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // Btn_AttachFile
            // 
            this.Btn_AttachFile.Location = new System.Drawing.Point(497, 405);
            this.Btn_AttachFile.Margin = new System.Windows.Forms.Padding(2);
            this.Btn_AttachFile.Name = "Btn_AttachFile";
            this.Btn_AttachFile.Size = new System.Drawing.Size(38, 24);
            this.Btn_AttachFile.TabIndex = 10;
            this.Btn_AttachFile.Text = ". . .";
            this.Btn_AttachFile.UseVisualStyleBackColor = true;
            this.Btn_AttachFile.Click += new System.EventHandler(this.Btn_OpenFile_Click);
            // 
            // Form_ReportIssue
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(583, 489);
            this.Controls.Add(this.Btn_AttachFile);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.Tb_Attachment);
            this.Controls.Add(this.Lb_IssueAttachment);
            this.Controls.Add(this.Tb_IssueLog);
            this.Controls.Add(this.Lb_IssueLog);
            this.Controls.Add(this.Tb_IssueContent);
            this.Controls.Add(this.Lb_IssueContent);
            this.Controls.Add(this.Lb_IssueTitle);
            this.Controls.Add(this.Tb_IssueTitle);
            this.Controls.Add(this.btnMakeIssue);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MdiChildrenMinimizedAnchorBottom = false;
            this.MinimizeBox = false;
            this.Name = "Form_ReportIssue";
            this.Text = "이슈 생성 - CLE Inc.";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnMakeIssue;
        private TextBox Tb_IssueTitle;
        private Label Lb_IssueTitle;
        private Label Lb_IssueContent;
        private TextBox Tb_IssueContent;
        private Label Lb_IssueLog;
        private TextBox Tb_IssueLog;
        private Label Lb_IssueAttachment;
        private OpenFileDialog openFileDlg;
        private TextBox Tb_Attachment;
        private Button btnCancel;
        private Button Btn_AttachFile;
    }
}