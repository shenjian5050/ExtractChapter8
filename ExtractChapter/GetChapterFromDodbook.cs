using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ExtractChapter
{
    class GetChapterFromDodbook
    {

        #region 变量

        /// <summary>
        /// 同时开启的线程数
        /// </summary>
        private int threadCount = 1;

        /// <summary>
        /// 获取网页或文件失败的重试的次数
        /// </summary>
        private int repeatCount = 5;

        /// <summary>
        /// 线程锁[存取章节列表]
        /// </summary>
        private Object thisLock = new Object();

        /// <summary>
        /// 线程锁[存取已停止的线程数]
        /// </summary>
        private Object thisLock2 = new Object();

        /// <summary>
        /// 书籍目录地址
        /// </summary>
        string address;

        /// <summary>
        /// 书籍信息
        /// </summary>
        Novel novel;

        /// <summary>
        /// 指示是否停止下载
        /// </summary>
        bool isStopDownload = false;

        /// <summary>
        /// 保存的本地文件位置
        /// </summary>
        string savedFolder;

        /// <summary>
        /// 活动线程数
        /// </summary>
        int activatedThreadCount;

        #endregion

        #region 属性

        /// <summary>
        /// 获取书籍信息
        /// </summary>
        public Novel Novel
        { get { return this.novel; } }

        /// <summary>
        /// 获取或设置书籍目录的网络地址
        /// </summary>
        public string BookAddress
        {
            get { return this.address; }
            set { this.address = value; }
        }

        /// <summary>
        /// 设置同时启动的最大线程数
        /// </summary>
        public int ThreadCount
        {
            set { this.threadCount = value; }
        }

        /// <summary>
        /// 设置失败重试次数
        /// </summary>
        public int RepeatCount
        {
            set { this.repeatCount = value; }
        }

        /// <summary>
        /// 获取或设置保存的本地文件夹地址
        /// </summary>
        public string SavedFolder
        {
            set { this.savedFolder = value; }
            get { return this.savedFolder; }
        }

        #endregion

        #region 构造

        public GetChapterFromDodbook()
        {
            //novel = new Novel();
            this.isStopDownload = false;
            this.activatedThreadCount = 0;
            this.savedFolder = System.Windows.Forms.Application.StartupPath + "\\";
        }

        #endregion

        public event NovelDirectoryListGettedHandler NovelDirectoryGetted;
        public delegate void NovelDirectoryListGettedHandler(object sender,Novel novel);


        public event NovelDirectoryListUpdatedHandler NovelDirectoryListUpdated;
        public delegate void NovelDirectoryListUpdatedHandler(object sender);

        public event HasAllThreadStoppedHandler HasAllThreadStopped;
        public delegate void HasAllThreadStoppedHandler(object sender);

        public event GetCatalogFailedHandler GetCatalogFailed;
        public delegate void GetCatalogFailedHandler(object sender);

        #region 获取目录

        private void TellGetCatalogFail()
        {
            if (GetCatalogFailed != null) this.GetCatalogFailed(this);
        }

        private void GetDirectoryListDelegate()
        {
            novel = null;
            novel = new Novel();
            novel.NovelAddress = this.address;


            WebClient webclient = new WebClient();
            
            webclient.Credentials = CredentialCache.DefaultCredentials; //获取或设置用于对向Internet资源的请求进行身份验证的网络凭据
            Byte[] pageData;
            string pageHtml = null;
            for (int loop = 0; loop < this.repeatCount; loop++)
            {
                try 
                {
                    pageData = webclient.DownloadData(this.address);
                    pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句
                    break;
                }
                catch { }
                finally
                { webclient.Dispose(); }
                //获取失败
                if (loop == 4)
                {
                    this.TellGetCatalogFail();
                    return;
                }
            }

            pageData = null;
            //获取小说名称
            string[] str1 = pageHtml.Split(new string[] { "booktitle", "<ul>", "</ul>" }, StringSplitOptions.RemoveEmptyEntries);
            pageHtml = null;
            if (str1.Length < 1) { this.TellGetCatalogFail(); return; }
            int index = str1[1].IndexOf("</span");
            if (index < 2) { this.TellGetCatalogFail(); return; }
            novel.NovelName = str1[1].Substring(2, index - 2);
            if (novel.NovelName.Length <= 0) { this.TellGetCatalogFail(); return; }
            index = str1[1].IndexOf("aspx");
            int index1 = str1[1].IndexOf("</a>");
            if (index < 0 || index1 < 0 || index1 < index + 6) { this.TellGetCatalogFail(); return; }
            novel.Author = str1[1].Substring(index + 6, index1 - index - 6);

            StringBuilder sbChapters = new StringBuilder(str1[2], str1[2].Length);
            str1 = null;
            sbChapters.Replace("\r\n\t\t", string.Empty);
            //sbChapters.Replace("\x09\x09\x20\x20\x20\x20", string.Empty);

            //byte[] tempb = Encoding.Default.GetBytes(sbChapters.ToString());
            //Regex.Replace(str1[2],"[\r\n\t\x09]",
            //sbChapters.Replace("    ", string.Empty);

            string chapterAll = sbChapters.ToString();
            sbChapters = null;
            string[] chapterList = chapterAll.Split(new string[] { "<li>", "</li>" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < chapterList.Length; i++)
            {
                if (chapterList[i].StartsWith("<a href"))
                {
                    //NovelChapter nc = new NovelChapter(
                    string[] temp = chapterList[i].Split(new string[] { "\"", ">", "</a" }, StringSplitOptions.RemoveEmptyEntries);

                    novel.AddChapter(temp[4], novel.NovelAddress + temp[1]);
                    
                    temp = null;

                }
            }
            //创建以小说名为名称的文件夹
            //this.savedFolder = this.savedFolder + novel.NovelName;
            this.novel.LocalFolder = this.savedFolder + novel.NovelName;
            if (Directory.Exists(this.novel.LocalFolder))
            {  }
            else
            { Directory.CreateDirectory(this.novel.LocalFolder);  }

            
            if(this.NovelDirectoryGetted!=null)
                this.NovelDirectoryGetted(this,this.novel);

        }

        /// <summary>
        /// 获取章节列表
        /// </summary>
        public void GetDirectoryList()
        {

            Thread thread = new Thread(new ThreadStart(this.GetDirectoryListDelegate));
            thread.IsBackground = true;
            thread.Start();
        }

        #endregion

        #region 获取章节内容私有方法

        Regex regHtmlEnd = new Regex(@"</html>(?:\s*<!--.*?-->)*\s*\Z", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 产生要在本地保存的文本文件名
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GenerateSaveFileName(int index)
        {
            string formatStr = "{0:D" + this.novel.NovelChapters.Count.ToString().Length.ToString() + "}";
            return string.Format(formatStr, index) + ".txt";
        }

        /// <summary>
        /// 改变章节的获取状态
        /// </summary>
        /// <param name="nc"></param>
        /// <param name="cs"></param>
        private void ChangeGetState(NovelChapter nc, ChapterState cs)
        {
            lock (this.thisLock)
            {
                nc.ChapterState = cs;
            }
            if (this.NovelDirectoryListUpdated != null) this.NovelDirectoryListUpdated(this);
        }

        /// <summary>
        /// 从网站中获取指定章节的内容,返回章节内容的字节
        /// </summary>
        /// <param name="needDownloadChapter"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private string GetResponseStringFromServer(NovelChapter needDownloadChapter,HttpWebRequest request,HttpWebResponse response)
        {
            Stream dataStream = null; 
            StreamReader reader=null;
            string responseFromServer = "";

            for (int loop = 0; loop < this.repeatCount; loop++)
            {
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(needDownloadChapter.ChapterAddress);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    
                    request.Timeout = 60000;
                    response = (HttpWebResponse)request.GetResponse();
                    request.KeepAlive = false;
                    dataStream = response.GetResponseStream();
                    reader = new StreamReader(dataStream, Encoding.Default);
                    responseFromServer = reader.ReadToEnd();
                    if (responseFromServer == "") continue;
                    if (regHtmlEnd.Match(responseFromServer).Success)
                        break;
                    else
                        continue;
                }
                catch
                { 
                    responseFromServer = "";
                    Thread.Sleep(1000);
                }
                finally
                {
                    
                    if (reader != null)
                        reader.Close();
                    if (dataStream != null)
                        dataStream.Close();
                    if (response != null)
                        response.Close();
                    if (request != null)
                        request.Abort();
                }
            }
            return responseFromServer;
        }

        /// <summary>
        /// 对获取的网页源码进行初步的处理,去掉头尾,去掉网站的嵌入代码等
        /// </summary>
        /// <param name="responseString"></param>
        /// <returns></returns>
        private string PreHandleResponseString(string responseString)
        {
            //处理
            string[] temp = responseString.Split(new string[] { "newstitle" }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 2) return "";
            string[] temp2 = temp[1].Split(new string[] { "<a target=" }, StringSplitOptions.RemoveEmptyEntries);
            temp = null;
            //纵横书海www.dodbook.com 字大好看保护你的眼睛
            //string[] temp3 = temp2[0].Split(new string[] { "字大好看保护你的眼睛" }, StringSplitOptions.RemoveEmptyEntries);
            if (temp2.Length < 1) return "";
            string[] temp3 = temp2[0].Split(new string[] { "<div id=\"B00KText\"><br />" }, StringSplitOptions.RemoveEmptyEntries);
            if (temp3.Length < 2) return "";
            temp2 = null;
            string temp4;
            StringBuilder sb = new StringBuilder(temp3[1]);
            temp4 = sb.ToString();
            int index = temp4.IndexOf("<div style='display:none'>");
            int index2 = 0;
            while (index >= 0)
            {
                index2 = temp4.IndexOf("</div>");
                if (index2 > index)
                {
                    sb.Remove(index, index2 - index + 6);
                    temp4 = sb.ToString();
                    index = temp4.IndexOf("<div style='display:none'>");
                }
                else
                    index = -1;
            }
            sb.Replace("纵横书海www.dodbook.com 字大好看保护你的眼睛", string.Empty);
            sb.Replace("<br />", string.Empty);
            sb.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "\r\n    ");
            return sb.ToString();
        }

        private void LastHandleResponseStringAndSave(string preHandleString, NovelChapter nc, int fileIndex)
        {
            string[] temp = preHandleString.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries);
            string strForSave = temp[0].Replace("<div>", string.Empty);
            temp = null;
            strForSave = strForSave.Replace("纵横书海", string.Empty);
            strForSave = strForSave.Replace("www.dodbook.com", string.Empty);

            FileStream fs = System.IO.File.Create(this.novel.LocalFolder + "\\" + GenerateSaveFileName(fileIndex));
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(strForSave);
            temp = null;
            sw.Flush();
            sw.Close();
            fs.Close();
            nc.FileName = GenerateSaveFileName(fileIndex);
            this.ChangeGetState(nc, ChapterState.完成);

        }

        /// <summary>
        /// 获取要下载的章节对象
        /// </summary>
        /// <returns></returns>
        private NovelChapter GetNeedDownloadChapter(ref int fileIndex)
        {
            NovelChapter needDownloadChapter = null;
            lock (this.thisLock)
            {
                for (int i = 0; i < this.novel.NovelChapters.Count; i++)
                {
                    if (this.novel.NovelChapters[i].ChapterState == ChapterState.等待中)
                    {
                        fileIndex = i;
                        needDownloadChapter = this.novel.NovelChapters[i];
                        needDownloadChapter.ChapterState = ChapterState.获取中;
                        break;
                    }
                }
            }
            if (this.NovelDirectoryListUpdated != null) this.NovelDirectoryListUpdated(this);
            return needDownloadChapter;
        }

        /// <summary>
        /// 从网页内容中取出章节图片地址并下载
        /// </summary>
        /// <param name="nc"></param>
        /// <param name="contentString"></param>
        private void DownloadAllChapterImg(NovelChapter nc, string contentString,int fileIndex)
        {
            Uri uri = new Uri(nc.ChapterAddress);
            WebClient webclient = new WebClient();
            webclient.Credentials = CredentialCache.DefaultCredentials;

            string[] temp = contentString.Split(new string[] { "<img src=\"" }, StringSplitOptions.RemoveEmptyEntries);
            string imgFilesName = "";
            //当前文件的名称(带后缀)
            string filenamewithExt = "";
            string imgPath;
            bool downloadSuccess = false;

            for (int j = 0; j < temp.Length; j++)
            {
                imgPath = "";
                filenamewithExt = "";
                int index3 = temp[j].IndexOf('"');
                if (index3 < 5) continue;
                string str = temp[j].Substring(0, index3);
                if (str.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || str.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    if (str.StartsWith("/"))
                    {
                        imgPath = "http://" + uri.Host + str;
                    }
                    else
                    {
                        imgPath = nc.ChapterAddress.Substring(0, nc.ChapterAddress.LastIndexOf("/")) + "/" + str;
                    }
                    filenamewithExt = fileIndex.ToString() + "_" + j.ToString() + imgPath.Substring(imgPath.Length - 4, 4);
                    for (int loop = 0; loop < this.repeatCount; loop++)
                    {
                        try
                        {
                            webclient.DownloadFile(imgPath, this.novel.LocalFolder + "\\" + filenamewithExt);
                            downloadSuccess = true;
                            break;
                        }
                        catch
                        { Thread.Sleep(1000); }
                        finally
                        { }
                        if (loop == 4)
                        {
                            downloadSuccess = false;

                        }
                    }
                    if (imgFilesName == "")
                        imgFilesName = filenamewithExt;
                    else
                        imgFilesName = imgFilesName + "|" + filenamewithExt;
                    //如果其中一张图片下载失败,停止下载该章的其它图片
                    if (downloadSuccess == false)
                        break;
                }
            }
            webclient.Dispose();
            
            if (downloadSuccess)
            {
                nc.FileName = imgFilesName;
                this.ChangeGetState(nc, ChapterState.完成);
            }
            else
            {
                this.ChangeGetState(nc, ChapterState.获取失败);
            }
        }

        #endregion

        private void GetContentDelegate()
        {

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            int fileIndex = 0;

            //获取要下载的章节对象
            NovelChapter needDownloadChapter = this.GetNeedDownloadChapter(ref fileIndex);

            while (needDownloadChapter != null)
            {
                
                //开始下载html文件任务
                string responseFromServer = null;
                //获取指定章节页的网页源码
                responseFromServer = this.GetResponseStringFromServer(needDownloadChapter, request, response);

                
                if (responseFromServer == null || responseFromServer=="")
                {
                    this.ChangeGetState(needDownloadChapter, ChapterState.获取失败);
                }
                else
                {
                    //对获取的网页源码作预处理
                    string preHandleString = this.PreHandleResponseString(responseFromServer);
                    if (preHandleString == "")
                    {
                        this.ChangeGetState(needDownloadChapter, ChapterState.获取失败);
                    }
                    else
                    {
                        int index = preHandleString.IndexOf("<img");
                        if (index > 0)
                        {
                            this.DownloadAllChapterImg(needDownloadChapter, preHandleString, fileIndex);
                        }
                        else
                        {
                            this.LastHandleResponseStringAndSave(preHandleString, needDownloadChapter, fileIndex);
                        }
                    }


                }

                Thread.Sleep(500);


                //判断是否要退出当前线程
                string threadName = Thread.CurrentThread.Name;
                Trace.Write(threadName);
                Trace.Write(threadName.Substring(16, threadName.Length - 16));
                if(threadName.StartsWith("GetContentThread"))
                    if (int.Parse(threadName.Substring(16, threadName.Length - 16)) > this.threadCount - 1)
                    {
                        break;
                    }

                if (this.isStopDownload) break;

                //继续下一个章节下载
                needDownloadChapter = this.GetNeedDownloadChapter(ref fileIndex);
                
            }
            lock (this.thisLock2)
            {
                this.activatedThreadCount--;

                if (this.activatedThreadCount == 0)
                    if (this.HasAllThreadStopped != null)
                        this.HasAllThreadStopped(this);
            }

            

        }

        /// <summary>
        /// 开始获取内容
        /// </summary>
        public void StartGetContent()
        {
            this.isStopDownload = false;
            this.activatedThreadCount = 0;
            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(new ThreadStart(this.GetContentDelegate));
                thread.IsBackground = true;
                thread.Name = "GetContentThread" + i.ToString();
                thread.Start();
                this.activatedThreadCount++;
            }
        }

        /// <summary>
        /// 停止获取章节内容
        /// </summary>
        public void StopGetContent()
        {
            this.isStopDownload = true;
        }

        /// <summary>
        /// 设置活动线程
        /// </summary>
        /// <param name="count"></param>
        public void ChangeThreadCount(int count)
        {
            this.threadCount = count;
            for (int i = this.activatedThreadCount; i < this.threadCount; i++)
            {
                Thread thread = new Thread(new ThreadStart(this.GetContentDelegate));
                thread.IsBackground = true;
                thread.Name = "GetContentThread" + i.ToString();
                thread.Start();
                this.activatedThreadCount++;
            }
        }

    }
}
