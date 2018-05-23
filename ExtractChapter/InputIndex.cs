using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ExtractChapter
{
    public partial class InputIndex : DevExpress.XtraEditors.XtraForm
    {

        public string BookCatalogUrl
        {
            get;
            set;
        }

        public string SelectedFolder
        { get; set; }

        public InputIndex()
        {
            InitializeComponent();
        }

        private void simpleButtonSelectFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Globals.FolderList.Add(this.folderBrowserDialog1.SelectedPath);
                this.comboBoxEditFolderList.Properties.Items.Add(this.folderBrowserDialog1.SelectedPath);
                this.comboBoxEditFolderList.SelectedIndex = 0;
                Properties.Settings.Default.LastFolder = this.folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void InputIndex_Load(object sender, EventArgs e)
        {

            foreach (string s in Globals.FolderList)
            {
                this.comboBoxEditFolderList.Properties.Items.Add(s);
            }
            this.comboBoxEditFolderList.Text = Properties.Settings.Default.LastFolder;
        }

        private void sbOK_Click(object sender, EventArgs e)
        {
            if (this.textEdit1.Text.Trim() == "") return;
            this.BookCatalogUrl = this.textEdit1.Text.Trim();
            if (this.comboBoxEditFolderList.SelectedIndex < 0)
            {
                this.SelectedFolder = System.Windows.Forms.Application.StartupPath;
                Globals.SavedFolderPath = this.SelectedFolder;
            }
            else
            {
                if (System.IO.Directory.Exists(this.comboBoxEditFolderList.SelectedItem.ToString()))
                {
                    this.SelectedFolder = this.comboBoxEditFolderList.SelectedItem.ToString();
                    Globals.SavedFolderPath = this.SelectedFolder;
                }
                else
                {
                    this.SelectedFolder = System.Windows.Forms.Application.StartupPath;
                    Globals.SavedFolderPath = this.SelectedFolder;
                }
                
            }
            
            this.DialogResult = DialogResult.OK;
        }

        private void sbCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}