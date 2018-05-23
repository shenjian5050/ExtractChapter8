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
    public partial class Settings : DevExpress.XtraEditors.XtraForm
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void sbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sbOK_Click(object sender, EventArgs e)
        {
            ((MainForm)(this.MdiParent)).ChangeThreadCount((int)(this.spinEditThreadCount.Value));
            this.Close();
        }
    }
}