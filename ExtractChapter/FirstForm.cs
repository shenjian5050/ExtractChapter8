using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.OleDb;

namespace ExtractChapter
{
    public partial class FirstForm : DevExpress.XtraEditors.XtraForm
    {


        public struct LocalNovel
        {
            public string NovelName;
            public string NovelLocalFolder;
            public string NovelWebAddress;
            public DateTime UpdateTime;
        }

        private List<LocalNovel> localNovels;
        private int buttonWidth;  //按钮宽度
        private int currentX;     //当前行的位置
        private int currentY;     //当前列的位置
        private int countOfEveryRow;  //每行列的个数
        private int padingTop = 20;
        private int padingLeft = 20;
        private int intervalButton = 20;  //按钮间隔

        public FirstForm()
        {
            InitializeComponent();
            localNovels = new List<LocalNovel>();

        }

        private void FirstForm_Load(object sender, EventArgs e)
        {
            UpdateNovelLink();
        }

        public void UpdateNovelLink()
        {
            this.Controls.Clear();
            GetLocalNovelInfo();
            showAllLocalNovelLinkButton();
        }


        private void buttonClicked(object sender, EventArgs e)
        {
            SimpleButton sb = sender as SimpleButton;
            if (System.IO.File.Exists(sb.ToolTip))
            {
                Chapters ch = new Chapters(sb.ToolTip);
                ch.MdiParent = Globals.mainForm;
                ch.Show();
            }
            

        }

        private void GetLocalNovelInfo()
        {
            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.novelConnectionString);
            try
            {
                conn.Open();
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select novelName,novelFileName,novelWebAddress,updateTime From novel order by updateTime Desc";
                    using (OleDbDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            LocalNovel ln = new LocalNovel();
                            ln.NovelName = rd.GetString(0);
                            ln.NovelLocalFolder = rd.GetString(1);
                            ln.NovelWebAddress = rd.GetString(2);
                            ln.UpdateTime = rd.GetDateTime(3);
                            
                            this.localNovels.Add(ln);
                        }

                        rd.Close();
                    }
                    cmd.Dispose();
                }
            }
            finally
            { conn.Close(); }
        }


        private void showAllLocalNovelLinkButton()
        {
            float length = 0;
            float temp;
            Graphics g = this.CreateGraphics();
            foreach (LocalNovel ln in localNovels)
            {
                temp = g.MeasureString(ln.NovelName, this.Font).Width;
                if (length < temp) length = temp;
            }
            buttonWidth = (int)length;

            this.currentX = 0;
            this.currentY = 0;

            this.countOfEveryRow = (int)((this.Width-this.padingLeft) / (this.buttonWidth + intervalButton));

            foreach (LocalNovel ln in localNovels)
            {
                SimpleButton sb = new SimpleButton();
                sb.Size = new Size((int)length + 10, sb.Height);
                sb.Text = ln.NovelName;
                sb.ToolTip = ln.NovelLocalFolder;
                sb.Location = new Point(this.padingLeft + intervalButton * this.currentX, this.padingTop + this.currentY * (sb.Height + this.intervalButton));
                sb.Click += new System.EventHandler(this.buttonClicked);
                this.Controls.Add(sb);
            }


        }

    }
}