using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace ExtractChapter
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public MainForm()
        {
            InitializeComponent();
            Globals.mainForm = this;

            //string sss = "3333<p>    浙江省";
            //sss = sss.Replace("<p>    ", "　　" + Environment.NewLine);
            //if (sss.Length > 5)
            //{ System.Diagnostics.Debug.WriteLine("ddd"); }

        }



        private void barEditItemTxtColor_ItemClick(object sender, ItemClickEventArgs e)
        {
            Color c = (Color)this.barEditItemTxtColor.EditValue;
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(Chapters))
                {
                    Chapters ch = f as Chapters;
                    ch.SetTxtColor(c);
                }
            }

        }

        private void barEditItemFont_ItemClick(object sender, ItemClickEventArgs e)
        {

        }




        private void barEditItemTxtColor_EditValueChanged(object sender, EventArgs e)
        {
            Color c = (Color)this.barEditItemTxtColor.EditValue;
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(Chapters))
                {
                    Chapters ch = f as Chapters;
                    ch.SetTxtColor(c);
                }
            }
        }

        private void barEditItemBgColor_EditValueChanged(object sender, EventArgs e)
        {
            Color c = (Color)(this.barEditItemBgColor.EditValue);
            Properties.Settings.Default.BgColor = c;
            Properties.Settings.Default.Save();
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(Chapters))
                {
                    Chapters ch = f as Chapters;
                    ch.SetBgColor(c);
                }
            }
        }

        private void barEditItemFont_EditValueChanged(object sender, EventArgs e)
        {

            string fontString = (string)(this.barEditItemFont.EditValue);
            Properties.Settings.Default.TxtFont = new Font(fontString, Properties.Settings.Default.TxtFont.Size);
            Properties.Settings.Default.Save();
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(Chapters))
                {
                    Chapters ch = f as Chapters;
                    ch.SetTxtFont(fontString);
                }
            }
        }

        private void barEditItemFontSize_EditValueChanged(object sender, EventArgs e)
        {
            int i = int.Parse(this.barEditItemFontSize.EditValue.ToString());
            Properties.Settings.Default.TxtFont = new Font(Properties.Settings.Default.TxtFont.FontFamily, (float)i);
            Properties.Settings.Default.Save();
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(Chapters))
                {
                    Chapters ch = f as Chapters;
                    ch.SetTxtSize(i);
                }
            }
        }

        #region 工具栏-操作

        //输入地址
        private void barButtonItemInputUri_ItemClick(object sender, ItemClickEventArgs e)
        {
            InputIndex ii = new InputIndex();
            if (ii.ShowDialog() == DialogResult.OK)
            {
                Chapters ch = new Chapters();
                
                ch.Address = ii.BookCatalogUrl;
                ch.LocalSavedFolder = ii.SelectedFolder;
                ch.MdiParent = this;
                ch.Show();
                //this.bbiDownloadChapters.Enabled = true;
                //Globals.SaveSettingToXml();
            }
            else
            {
                this.bbiDownloadChapters.Enabled = false;
            }
        }

        //获取目录
        private void bbiGetCatalog_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ActiveMdiChild.GetType() == typeof(Chapters))
            {
                Chapters ch = this.ActiveMdiChild as Chapters;
                ch.GetCatalog();
            }
        }

        //全部章节
        private void bbiShowChapters_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ActiveMdiChild.GetType() == typeof(Chapters))
            {
                Chapters ch = this.ActiveMdiChild as Chapters;
                ch.GetChaptersContent();
            }
        }



        //获取当前章节
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ActiveMdiChild.GetType() == typeof(Chapters))
            {
                Chapters ch = this.ActiveMdiChild as Chapters;

            }
        }

        //获取失败章节
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ActiveMdiChild.GetType() == typeof(Chapters))
            {
                Chapters ch = this.ActiveMdiChild as Chapters;

            }

        }

        //重获全部章节
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ActiveMdiChild.GetType() == typeof(Chapters))
            {
                Chapters ch = this.ActiveMdiChild as Chapters;
                ch.RegetChaptersContent();
            }
        }

        #endregion

        //设置
        private void bbiSetting_ItemClick(object sender, ItemClickEventArgs e)
        {
            Settings set = new Settings();
            set.MdiParent = this;
            set.Show();
        }

        public void ChangeThreadCount(int count)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(Chapters))
                {
                    Chapters ch = f as Chapters;
                    ch.ChangeThreadCount(count);
                }
            }
        }

        private void bbiStopDownContent_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(Chapters))
                {
                    Chapters ch = f as Chapters;
                    ch.StopDownload();
                }
            }
        }

        private void bbiContinueGet_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ActiveMdiChild.GetType() == typeof(Chapters))
            {
                Chapters ch = this.ActiveMdiChild as Chapters;
                ch.GetChaptersContent();
            }
        }

        private void bbiQuit_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(Chapters))
                {
                    Chapters ch = f as Chapters;
                    ch.Close();
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FirstForm ff = new FirstForm();
            ff.MdiParent = this;
            ff.Show();
        }

        private void bbiSaveBook_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ActiveMdiChild.GetType() == typeof(Chapters))
            {
                Chapters ch = this.ActiveMdiChild as Chapters;
                ch.SaveBook();
            }
        }

        private void bbiOpenBook_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "ecb";
            ofd.Filter = "ECB文件(*.ECB)|*.ECB";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Chapters ch = new Chapters(ofd.FileName);
                ch.MdiParent = this;
                ch.Show();
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ActiveMdiChild.GetType() == typeof(Chapters))
            {
                Chapters ch = this.ActiveMdiChild as Chapters;
                ch.StartSaveToSql();
            }


            //if (System.IO.Directory.Exists(@"C:\Downloads"))
            //{
            //    MessageBox.Show(System.IO.Directory.GetParent(@"C:\Downloads\").FullName);
            //    MessageBox.Show(Environment.CurrentDirectory);
            //    MessageBox.Show("存在");
                
            //}
            //else
            //{
            //    MessageBox.Show(Environment.CurrentDirectory);
            //    MessageBox.Show("不存在");
            //}
        }

        private void bbiStopGet_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ActiveMdiChild.GetType() == typeof(Chapters))
            {
                Chapters ch = this.ActiveMdiChild as Chapters;
                ch.StopDownload();
            }
        }
    }
}