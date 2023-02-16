namespace LogViewer
{
    partial class FormMakeIssue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnMakeIssue = new System.Windows.Forms.Button();
            this.textBoxIssueTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxContents = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLogs = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.textBoxAttachedFile = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAttachFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMakeIssue
            // 
            this.btnMakeIssue.Location = new System.Drawing.Point(238, 677);
            this.btnMakeIssue.Name = "btnMakeIssue";
            this.btnMakeIssue.Size = new System.Drawing.Size(152, 32);
            this.btnMakeIssue.TabIndex = 0;
            this.btnMakeIssue.Text = "이슈만들기";
            this.btnMakeIssue.UseVisualStyleBackColor = true;
            // 
            // textBoxIssueTitle
            // 
            this.textBoxIssueTitle.Location = new System.Drawing.Point(130, 46);
            this.textBoxIssueTitle.Name = "textBoxIssueTitle";
            this.textBoxIssueTitle.Size = new System.Drawing.Size(597, 27);
            this.textBoxIssueTitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "제목";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "내용";
            // 
            // textBoxContents
            // 
            this.textBoxContents.Location = new System.Drawing.Point(130, 95);
            this.textBoxContents.Multiline = true;
            this.textBoxContents.Name = "textBoxContents";
            this.textBoxContents.Size = new System.Drawing.Size(597, 230);
            this.textBoxContents.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(67, 351);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "로그";
            // 
            // textBoxLogs
            // 
            this.textBoxLogs.Location = new System.Drawing.Point(130, 348);
            this.textBoxLogs.Multiline = true;
            this.textBoxLogs.Name = "textBoxLogs";
            this.textBoxLogs.ReadOnly = true;
            this.textBoxLogs.Size = new System.Drawing.Size(597, 183);
            this.textBoxLogs.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 566);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "첨부";
            // 
            // openFileDlg
            // 
            this.openFileDlg.FileName = "openFileDlg";
            // 
            // textBoxAttachedFile
            // 
            this.textBoxAttachedFile.Location = new System.Drawing.Point(130, 563);
            this.textBoxAttachedFile.Multiline = true;
            this.textBoxAttachedFile.Name = "textBoxAttachedFile";
            this.textBoxAttachedFile.ReadOnly = true;
            this.textBoxAttachedFile.Size = new System.Drawing.Size(507, 76);
            this.textBoxAttachedFile.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(420, 677);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 32);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAttachFile
            // 
            this.btnAttachFile.Location = new System.Drawing.Point(675, 563);
            this.btnAttachFile.Name = "btnAttachFile";
            this.btnAttachFile.Size = new System.Drawing.Size(49, 32);
            this.btnAttachFile.TabIndex = 10;
            this.btnAttachFile.Text = ". . .";
            this.btnAttachFile.UseVisualStyleBackColor = true;
            this.btnAttachFile.Click += new System.EventHandler(this.btnAttachFile_Click);
            // 
            // FormMakeIssue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 734);
            this.Controls.Add(this.btnAttachFile);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.textBoxAttachedFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxLogs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxContents);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxIssueTitle);
            this.Controls.Add(this.btnMakeIssue);
            this.MaximizeBox = false;
            this.MdiChildrenMinimizedAnchorBottom = false;
            this.MinimizeBox = false;
            this.Name = "FormMakeIssue";
            this.Text = "IssueMaker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnMakeIssue;
        private TextBox textBoxIssueTitle;
        private Label label1;
        private Label label2;
        private TextBox textBoxContents;
        private Label label3;
        private TextBox textBoxLogs;
        private Label label4;
        private OpenFileDialog openFileDlg;
        private TextBox textBoxAttachedFile;
        private Button btnCancel;
        private Button btnAttachFile;
    }
}