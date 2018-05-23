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
    public partial class YesOrNoDialog : DevExpress.XtraEditors.XtraForm
    {

        public string Note
        {
            set { this.lcNote.Text = value; }
        }

        public YesOrNoDialog()
        {
            InitializeComponent();
        }
    }
}