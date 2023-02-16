using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogViewer
{
    public partial class FormMakeIssue : Form
    {
        public FormMakeIssue()
        {
            InitializeComponent();
        }

        private void btnAttachFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files | *.*";
            dlg.ShowDialog();
            if(dlg.FileName.Length > 0)
            {
                foreach(string s in dlg.FileNames)
                {
                    this.textBoxAttachedFile.Text = s;
                }
            }
        }
    }
}
