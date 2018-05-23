using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using CS.GetChapterUtil;
using System.Threading;

namespace ExtractChapter
{
    public partial class Chapters : DevExpress.XtraEditors.XtraForm
    {
        public Chapters()
        {
            InitializeComponent();

        }

        public Chapters(string ecbFile)
        {
            InitializeComponent();
            this.LoadBook(ecbFile);
            this.Address = this.book.NovelAddress;

            this.LocalSavedFolder = Directory.GetParent(this.book.LocalFolder).FullName;

            

        }

        #region 变量

        bool isGetCatalog = false;
        bool isGetChapterContent = false;
        bool isGetCatalogSuccess = false;
        Novel book;
        GetChapter getChapterForm;
        bool isHasSaved = true;

        #endregion

        /// <summary>
        /// 书籍目录所在网络地址
        /// </summary>
        public string Address
        { get; set; }

        /// <summary>
        /// 本地保存用文件夹
        /// </summary>
        public string LocalSavedFolder
        { get; set; }

        #region 线程间调用

        public delegate void CallBackSetDirectoryList(Novel novel);

        public void SetDirectoryList(Novel novel)
        {
            book = novel;

            if (this.treeListDirectory.InvokeRequired)
            {
                CallBackSetDirectoryList d = new CallBackSetDirectoryList(this.SetDirectoryList);
                this.Invoke(d, new object[] { novel });
            }
            else
                this.treeListDirectory.DataSource = novel.NovelChapters;
        }


        public delegate void CallBackUpdateList();

        public void UpdateList()
        {

            if (this.treeListDirectory.InvokeRequired)
            {
                if (this.treeListDirectory.IsDisposed) return;
                CallBackUpdateList d = new CallBackUpdateList(this.UpdateList);
                this.Invoke(d);
            }
            else
            {
                if (this.treeListDirectory.IsDisposed) return;
                this.treeListDirectory.RefreshDataSource();
            }
           
        }


        public delegate void CallBackSetNovelName(string name);

        public void SetNovelName(string name)
        {
            if (this.InvokeRequired)
            {
                CallBackSetNovelName d = new CallBackSetNovelName(this.SetNovelName);
                this.Invoke(d, new object[] { name });
            }
            else
                this.Text = name;
        }

        public delegate void CallBackSetTxtColor(Color c);

        public delegate void CallBackSetTxtFont(string f);

        public delegate void CallBackSetTxtSize(int f);

        public delegate void CallBackSetBgColor(Color c);


        public void SetTxtColor(Color c)
        {
            if (this.memoEditTxt.InvokeRequired)
            {
                CallBackSetTxtColor d = new CallBackSetTxtColor(this.SetTxtColor);
                this.Invoke(d, new object[] { c });
            }
            else
            {
                //this.memoEditTxt.Properties.Appearance.Options.UseForeColor = true;
                //this.memoEditTxt.Properties.AppearanceFocused.Options.UseForeColor = true;
                this.memoEditTxt.ForeColor = c;
            }
        }

        public void SetTxtFont(string f)
        {
            if (this.memoEditTxt.InvokeRequired)
            {
                CallBackSetTxtFont d = new CallBackSetTxtFont(this.SetTxtFont);
                this.Invoke(d, new object[] { f });
            }
            else
            {
                //this.memoEditTxt.Properties.Appearance.Options.UseFont = true;
                //this.memoEditTxt.Properties.AppearanceFocused.Options.UseFont = true;
                this.memoEditTxt.Font = new Font(f, this.memoEditTxt.Font.Size);
            }
        }

        public void SetTxtSize(int i)
        {
            if (this.memoEditTxt.InvokeRequired)
            {
                CallBackSetTxtSize d = new CallBackSetTxtSize(this.SetTxtSize);
                this.Invoke(d, new object[] { i });
            }
            else
            {
                //this.memoEditTxt.Properties.Appearance.Options.UseFont = true;
                //this.memoEditTxt.Properties.AppearanceFocused.Options.UseFont = true;
                this.memoEditTxt.Font = new Font(this.memoEditTxt.Font.FontFamily, (float)i);
            }
        }

        public void SetBgColor(Color c)
        {
            if (this.memoEditTxt.InvokeRequired)
            {
                CallBackSetBgColor d = new CallBackSetBgColor(this.SetBgColor);
                this.Invoke(d, new object[] { c });
            }
            else
            {
                //this.memoEditTxt.Properties.Appearance.Options.UseBackColor = true;
                //this.memoEditTxt.Properties.AppearanceFocused.Options.UseBackColor = true;
                this.memoEditTxt.BackColor = c;
            }
        }

        #endregion

        #region 载入
        private void Chapters_Load(object sender, EventArgs e)
        {
            //this.Address = "http://www.dodbook.com/Html/Book/10/10285/Index.html";
            if (this.book == null)
                this.Text = this.Address;
            else
                this.Text = this.book.NovelName;
            string temp=this.Address.ToLower();
            if (temp.Contains("dodbook.com"))
            {

                GetChapterFromDodbook gcfd = new GetChapterFromDodbook();
                this.getChapterForm = gcfd;
                gcfd.SavedFolder = this.LocalSavedFolder;
                gcfd.BookAddress = this.Address;
                gcfd.NovelDirectoryGetted += new GetChapterFromDodbook.NovelDirectoryListGettedHandler(gcfd_NovelDirectoryGetted);
                gcfd.NovelDirectoryListUpdated += new GetChapterFromDodbook.NovelDirectoryListUpdatedHandler(gcfd_NovelDirectoryListUpdated);
                gcfd.GetCatalogFailed += new GetChapterFromDodbook.GetCatalogFailedHandler(gcfd_GetCatalogFailed);
                gcfd.HasAllThreadStopped += new GetChapterFromDodbook.HasAllThreadStoppedHandler(gcfd_HasAllThreadStopped);

            }
            else if (temp.Contains("ranwen.com"))
            {
                GetChapterFromRanwen gcfd = new GetChapterFromRanwen();
                this.getChapterForm = gcfd;
                gcfd.SavedFolder = this.LocalSavedFolder;
                gcfd.BookAddress = this.Address;
                gcfd.NovelDirectoryGetted += new GetChapterFromRanwen.NovelDirectoryListGettedHandler(gcfd_NovelDirectoryGetted);
                gcfd.NovelDirectoryListUpdated += new GetChapterFromRanwen.NovelDirectoryListUpdatedHandler(gcfd_NovelDirectoryListUpdated);
                gcfd.GetCatalogFailed += new GetChapterFromRanwen.GetCatalogFailedHandler(gcfd_GetCatalogFailed);
                gcfd.HasAllThreadStopped += new GetChapterFromRanwen.HasAllThreadStoppedHandler(gcfd_HasAllThreadStopped);
            }
            else if (temp.Contains("biquge.com"))
            {
                GetChapterFromBiquge gcfb = new GetChapterFromBiquge();
                this.getChapterForm = gcfb;
                gcfb.SavedFolder = this.LocalSavedFolder;
                gcfb.BookAddress = this.Address;
                gcfb.NovelDirectoryGetted += new GetChapter.NovelDirectoryListGettedHandler(gcfd_NovelDirectoryGetted);
                gcfb.NovelDirectoryListUpdated += new GetChapter.NovelDirectoryListUpdatedHandler(gcfd_NovelDirectoryListUpdated);
                gcfb.GetCatalogFailed += new GetChapter.GetCatalogFailedHandler(gcfd_GetCatalogFailed);
                gcfb.HasAllThreadStopped += new GetChapter.HasAllThreadStoppedHandler(gcfd_HasAllThreadStopped);
            }
            else if (temp.Contains("kenshuge.com"))
            {
                GetChapterFromKenshuge gcfk = new GetChapterFromKenshuge();
                this.getChapterForm = gcfk;
                gcfk.SavedFolder = this.LocalSavedFolder;
                gcfk.BookAddress = this.Address;
                gcfk.NovelDirectoryGetted += new GetChapter.NovelDirectoryListGettedHandler(gcfd_NovelDirectoryGetted);
                gcfk.NovelDirectoryListUpdated += new GetChapter.NovelDirectoryListUpdatedHandler(gcfd_NovelDirectoryListUpdated);
                gcfk.GetCatalogFailed += new GetChapter.GetCatalogFailedHandler(gcfd_GetCatalogFailed);
                gcfk.HasAllThreadStopped += new GetChapter.HasAllThreadStoppedHandler(gcfd_HasAllThreadStopped);
            }
            else if (temp.ToLower().Contains("23us.com"))
            {
                GetChapterFrom23us gcfb = new GetChapterFrom23us();
                this.getChapterForm = gcfb;
                gcfb.SavedFolder = this.LocalSavedFolder;
                gcfb.BookAddress = this.Address;
                gcfb.NovelDirectoryGetted += new GetChapter.NovelDirectoryListGettedHandler(gcfd_NovelDirectoryGetted);
                gcfb.NovelDirectoryListUpdated += new GetChapter.NovelDirectoryListUpdatedHandler(gcfd_NovelDirectoryListUpdated);
                gcfb.GetCatalogFailed += new GetChapter.GetCatalogFailedHandler(gcfd_GetCatalogFailed);
                gcfb.HasAllThreadStopped += new GetChapter.HasAllThreadStoppedHandler(gcfd_HasAllThreadStopped);
            }
            else if (temp.ToLower().Contains("31xs.net"))
            {
                GetChapterFrom31xs gcfx = new GetChapterFrom31xs();
                this.getChapterForm = gcfx;
                gcfx.SavedFolder = this.LocalSavedFolder;
                gcfx.BookAddress = this.Address;
                gcfx.NovelDirectoryGetted += new GetChapter.NovelDirectoryListGettedHandler(gcfd_NovelDirectoryGetted);
                gcfx.NovelDirectoryListUpdated += new GetChapter.NovelDirectoryListUpdatedHandler(gcfd_NovelDirectoryListUpdated);
                gcfx.GetCatalogFailed += new GetChapter.GetCatalogFailedHandler(gcfd_GetCatalogFailed);
                gcfx.HasAllThreadStopped += new GetChapter.HasAllThreadStoppedHandler(gcfd_HasAllThreadStopped);

            }
            else
            {
                MessageBox.Show("暂不支持从此网站获取书籍内容");
                this.Close();
            }
            this.memoEditTxt.BackColor = Properties.Settings.Default.BgColor;
            this.memoEditTxt.ForeColor = Properties.Settings.Default.FgColor;
            this.memoEditTxt.Font = Properties.Settings.Default.TxtFont;

        }

        #endregion

        #region 事件处理

        private void gcfd_NovelDirectoryGetted(object sender, Novel novel)
        {
            this.SetNovelName(novel.NovelName);
            this.SetDirectoryList(novel);
            this.isGetCatalog = false;
            this.isGetCatalogSuccess = true;
            
        }

        private void gcfd_NovelDirectoryListUpdated(object sender)
        {
            isHasSaved = false;
            this.UpdateList();
        }

        private void gcfd_GetCatalogFailed(object sender)
        {
            this.SetNovelName(this.Address);
            //this.Text = this.Address;
            isGetCatalogSuccess = false;
            isGetCatalog = false;
            MessageBox.Show("获取章节目录失败");
        }

        private void gcfd_HasAllThreadStopped(object sender)
        {
            this.isGetChapterContent = false;
            this.SetNovelName(this.book.NovelName);
            //this.Text = this.book.NovelName;
        }

        #endregion


        public void StartSaveToSql()
        {
            Thread thread = new Thread(new ThreadStart(SaveToSql))
            { IsBackground = true };
            thread.Start();
        }

        int novelID = -1;


        private void UpdateCaption(string text)
        {
            this.Invoke((EventHandler)delegate {
                this.Text = text;
            });

        }

        private void SaveToSql()
        {
            
            novelID = Globals.MysqlHelper.GetNovelID(book.NovelName, book.Author);
            if (novelID < 0)
            {
                if (Globals.MysqlHelper.InsertNewNovel(book.NovelName, book.Author))
                {
                    novelID = Globals.MysqlHelper.GetNovelID(book.NovelName, book.Author);
                }
            }

            if (novelID >= 0)
            {
                for (int i = 0; i < this.book.NovelChapters.Count; i++)
                {
                    UpdateCaption(book.NovelName + ":正在保存到数据库" + i.ToString() + "/" + book.NovelChapters.Count.ToString());
                    NovelChapter nc = this.book.NovelChapters[i];
                    if (nc != null)
                    {
                        if (nc.FileName != null && nc.FileName != string.Empty && nc.FileName != "")
                        {

                            if (nc.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                            {
                                if (File.Exists(this.book.LocalFolder + "\\" + nc.FileName))
                                {
                                    int chapterID = Convert.ToInt32(nc.FileName.Substring(0, nc.FileName.IndexOf('.')));
                                    StreamReader sr = new StreamReader(this.book.LocalFolder + "\\" + nc.FileName);
                                    string s = sr.ReadToEnd();
                                    sr.Close();

                                    if (Globals.MysqlHelper.IsExistChapterID(novelID, chapterID))
                                        Globals.MysqlHelper.UpdateChapter(novelID, chapterID, this.book.NovelChapters[i].ChapterName, s);
                                    else
                                        Globals.MysqlHelper.InsertNewChapter(novelID, chapterID, this.book.NovelChapters[i].ChapterName, s);
                                }

                            }
                            else if (nc.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) || nc.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                            {

                            }


                        }
                    }
                    

                }
            }
            UpdateCaption(book.NovelName);

        }


        private void Chapters_Shown(object sender, EventArgs e)
        {
        }

        public void GetCatalog()
        {

            if (isGetCatalog)
            { MessageBox.Show("正在获取目录,请稍候..."); return; }
            if(isGetChapterContent)
            { MessageBox.Show("正在获取章节内容,请稍候..."); return; }
            isGetCatalog = true;
            isGetCatalogSuccess = false;
            this.Text += "->正在获取目录...";
            isHasSaved = false;
            getChapterForm.GetDirectoryList();
        }

        public void GetChaptersContent()
        {
            if (isGetCatalog)
            { MessageBox.Show("正在获取目录,请稍候..."); return; }
            if (isGetChapterContent)
            { MessageBox.Show("正在获取章节内容,请稍候..."); return; }
            if(!isGetCatalogSuccess)
            { MessageBox.Show("还未获取目录,请先获取目录"); return; }
            isGetChapterContent = true;
            this.Text += "->正在获取章节内容...";
            isHasSaved = false;
            getChapterForm.StartGetContent();
        }

        public void RegetChaptersContent()
        {
            if (isGetCatalog)
            { MessageBox.Show("正在获取目录,请稍候..."); return; }
            if (isGetChapterContent)
            { MessageBox.Show("正在获取章节内容,请稍候..."); return; }
            if (!isGetCatalogSuccess)
            { MessageBox.Show("还未获取目录,请先获取目录"); return; }
            isGetChapterContent = true;
            this.Text += "->正在获取章节内容...";
            isHasSaved = false;
            getChapterForm.RegetContent();
        }

        public void RegetFailedChapterContent()
        {
            if (isGetCatalog)
            { MessageBox.Show("正在获取目录,请稍候..."); return; }
            if (isGetChapterContent)
            { MessageBox.Show("正在获取章节内容,请稍候..."); return; }
            if (!isGetCatalogSuccess)
            { MessageBox.Show("还未获取目录,请先获取目录"); return; }
            isGetChapterContent = true;
            this.Text += "->正在获取章节内容...";
            isHasSaved = false;
            getChapterForm.RegetFailedContent();
        }

        public void ChangeThreadCount(int count)
        {
            this.getChapterForm.ChangeThreadCount(count);
        }

        public void StopDownload()
        {
            this.getChapterForm.StopGetContent();
        }

        public bool SaveBook()
        {
            if (this.isHasSaved) return true;
            if (this.book == null) return false ;
            try
            {
                Stream stream = File.Open(this.book.LocalFolder + "\\" + this.book.NovelName + ".ECB", FileMode.Create, FileAccess.ReadWrite);
                //SoapFormatter formatter = new SoapFormatter();
                //formatter.Serialize(stream, this.book);
                IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(stream, this.book);
                stream.Close(); 
                return true;
            }
            catch { return false; }
            finally
            { }


        }

        public void LoadBook(string fileName)
        {
            
            if (File.Exists(fileName))
            {
                Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                book = (Novel)formatter.Deserialize(stream);
                stream.Close();
                this.treeListDirectory.DataSource = book.NovelChapters;

                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.novelConnectionString);
                try
                {
                    conn.Open();
                    using (OleDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select novelName from novel where novelName='" + this.book.NovelName + "'";
                        if (cmd.ExecuteNonQuery() < 1)
                        {
                            cmd.CommandText = "insert into novel (novelName,novelFileName,novelWebAddress,updateTime) values ('" + this.book.NovelName + "','" + fileName + "','" + this.book.NovelAddress + "','" + DateTime.Now + "')";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    foreach (Form f in Globals.mainForm.MdiChildren)
                    {
                        if (f.GetType() == typeof(FirstForm))
                        {
                            FirstForm ff = f as FirstForm;
                            ff.UpdateNovelLink();
                        }
                    }

                }
                finally
                { conn.Close(); }
            }
        }

        #region 目录栏操作

        private void treeListDirectory_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

            //this.barTool.Visible = false;
            //this.isChangeByLoad = true;
            //if (e.Node == null) return;
            //if (e.Node.Equals(e.OldNode)) { MessageBox.Show("test"); return; }
            ////MessageBox.Show("test");
            //NovelChapter nc = this.treeListDirectory.GetDataRecordByNode(e.Node) as NovelChapter;
            //if (nc != null)
            //{
            //    if (nc.FileName != null && nc.FileName != string.Empty && nc.FileName != "")
            //    {

            //        if(nc.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            //        {
            //            if (File.Exists(this.book.LocalFolder + "\\" + nc.FileName))
            //            {

            //                //FileStream fs = new FileStream(System.Windows.Forms.Application.StartupPath + "\\" + nc.FileName, FileMode.Open);
            //                StreamReader sr = new StreamReader(this.book.LocalFolder + "\\" + nc.FileName);
            //                string s = sr.ReadToEnd();

            //                sr.Close();

            //                //fs.Close();
            //                this.memoEditTxt.Text = s;
            //                this.memoEditTxt.Tag = nc.FileName;
            //            }
            //            this.xtraScrollableControlImg.Visible = false;
            //            this.memoEditTxt.Visible = true;
            //        }
            //        else if (nc.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) || nc.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
            //        {
            //            this.xtraScrollableControlImg.Controls.Clear();
            //            string[] filelist = nc.FileName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            //            int imglocationheight = 10;
                        
            //            for (int i = 0; i < filelist.Length; i++)
            //            {
            //                if (File.Exists(this.book.LocalFolder + "\\" + filelist[i]))
            //                {
            //                    PictureEdit pe = new PictureEdit();
            //                    pe.Image = Image.FromFile(this.book.LocalFolder + "\\" + filelist[i]);
            //                    pe.Size = new Size(pe.Image.Width, pe.Image.Height);
            //                    pe.Location = new Point(0, imglocationheight);
            //                    imglocationheight = imglocationheight + pe.Image.Height + 10;
            //                    this.xtraScrollableControlImg.Controls.Add(pe);
                                

            //                }
            //            }
            //            this.xtraScrollableControlImg.Visible = true;
            //            this.memoEditTxt.Visible = false;
            //        }


            //    }
            //}



            //if (this.treeListDirectory.FocusedNode != null)
            //{
            //    string name = this.treeListDirectory.FocusedNode["文件名"].ToString();
            //    //if (name != null || name != string.Empty || name != "")
            //    //{
            //    //    if (File.Exists(System.Windows.Forms.Application.StartupPath + "\\" + name))
            //    //    {

            //    //        FileStream fs = new FileStream(System.Windows.Forms.Application.StartupPath + "\\" + name, FileMode.Open);
            //    //        StreamReader sr = new StreamReader(fs, Encoding.Default);
            //    //        string s = sr.ReadToEnd();

            //    //        sr.Close();

            //    //        fs.Close();
            //    //        this.memoEdit1.Text = s;
            //    //    }
            //    //}
            //}
        }

        #endregion

        private void bbiSaveModifiedTxt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FileStream fs = new FileStream(this.book.LocalFolder + "\\" + this.memoEditTxt.Tag, FileMode.Open, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(this.memoEditTxt.Text);

            sw.Flush();
            sw.Close();
            fs.Close();
            this.barTool.Visible = false;
        }

        /// <summary>
        /// 指示memoEditTxt的内容改变是因为载入内容
        /// </summary>
        bool isChangeByLoad = true;

        private void memoEditTxt_EditValueChanged(object sender, EventArgs e)
        {
            if (isChangeByLoad)
                isChangeByLoad = false;
            else
                this.barTool.Visible = true;
        }

        private void treeListDirectory_MouseClick(object sender, MouseEventArgs e)
        {
            DevExpress.XtraTreeList.TreeListHitInfo hi = this.treeListDirectory.CalcHitInfo(new Point(e.X, e.Y));
            if (hi.Node == null) return;

            this.barTool.Visible = false;
            this.isChangeByLoad = true;
            //MessageBox.Show("test");
            NovelChapter nc = this.treeListDirectory.GetDataRecordByNode(hi.Node) as NovelChapter;
            if (nc != null)
            {
                if (nc.FileName != null && nc.FileName != string.Empty && nc.FileName != "")
                {

                    if (nc.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        if (File.Exists(this.book.LocalFolder + "\\" + nc.FileName))
                        {

                            //FileStream fs = new FileStream(System.Windows.Forms.Application.StartupPath + "\\" + nc.FileName, FileMode.Open);
                            StreamReader sr = new StreamReader(this.book.LocalFolder + "\\" + nc.FileName);
                            string s = sr.ReadToEnd();

                            sr.Close();

                            //fs.Close();
                            this.memoEditTxt.Text = s;
                            this.memoEditTxt.Tag = nc.FileName;
                        }
                        this.xtraScrollableControlImg.Visible = false;
                        this.memoEditTxt.Visible = true;
                    }
                    else if (nc.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) || nc.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                    {
                        this.xtraScrollableControlImg.Controls.Clear();
                        string[] filelist = nc.FileName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        int imglocationheight = 10;

                        for (int i = 0; i < filelist.Length; i++)
                        {
                            //MessageBox.Show(Application.ExecutablePath);
                            //MessageBox.Show(this.book.LocalFolder + "\\" + filelist[i]);//http://www.biquge.com/0_46/
                            if (File.Exists(this.book.LocalFolder + "\\" + filelist[i]))
                            {
                                PictureEdit pe = new PictureEdit();
                                pe.Image = Image.FromFile(this.book.LocalFolder + "\\" + filelist[i]);
                                pe.Size = new Size(pe.Image.Width, pe.Image.Height);
                                pe.Location = new Point(0, imglocationheight);
                                imglocationheight = imglocationheight + pe.Image.Height + 10;
                                this.xtraScrollableControlImg.Controls.Add(pe);


                            }
                            else
                            {
                                
                            }
                        }
                        this.xtraScrollableControlImg.Visible = true;
                        this.memoEditTxt.Visible = false;
                    }


                }
            }
        }

        private void Chapters_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.isHasSaved == false)
            {
                YesOrNoDialog yn = new YesOrNoDialog();
                yn.Note = "[" + this.book.NovelName + "]还未保存，是否保存？";
                if (yn.ShowDialog() == DialogResult.Yes)
                    this.SaveBook();
            }
            this.getChapterForm.StopGetContent();
            e.Cancel = false;
        }

        private void Chapters_KeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show("1");
        }
    }




}