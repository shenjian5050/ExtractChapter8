using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CS.GetChapterUtil
{
    abstract public class GetChapter
    {


        #region 变量

        /// <summary>
        /// 同时开启的线程数
        /// </summary>
        protected int threadCount = 5;

        /// <summary>
        /// 获取网页或文件失败的重试的次数
        /// </summary>
        protected int repeatCount = 5;

        /// <summary>
        /// 线程锁[存取章节列表]
        /// </summary>
        protected Object thisLock = new Object();

        /// <summary>
        /// 线程锁[存取已停止的线程数]
        /// </summary>
        protected Object thisLock2 = new Object();

        /// <summary>
        /// 书籍目录地址
        /// </summary>
        protected string address;

        /// <summary>
        /// 书籍信息
        /// </summary>
        protected Novel novel;

        /// <summary>
        /// 指示是否停止下载
        /// </summary>
        protected bool isStopDownload = true;

        /// <summary>
        /// 保存的本地文件位置
        /// </summary>
        protected string savedFolder;

        /// <summary>
        /// 活动线程数
        /// </summary>
        protected int activatedThreadCount;

        Regex regHtmlEnd = new Regex(@"</html>(?:\s*<!--.*?-->)*\s*\Z", RegexOptions.IgnoreCase | RegexOptions.Singleline);


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

        #region 构造函数

        public GetChapter()
        {
            this.isStopDownload = false;
            this.activatedThreadCount = 0;
            this.savedFolder = System.Windows.Forms.Application.StartupPath + "\\";
        }

        #endregion


        #region 事件声明

        public event NovelDirectoryListGettedHandler NovelDirectoryGetted;
        public delegate void NovelDirectoryListGettedHandler(object sender, Novel novel);


        public event NovelDirectoryListUpdatedHandler NovelDirectoryListUpdated;
        public delegate void NovelDirectoryListUpdatedHandler(object sender);

        public event HasAllThreadStoppedHandler HasAllThreadStopped;
        public delegate void HasAllThreadStoppedHandler(object sender);

        public event GetCatalogFailedHandler GetCatalogFailed;
        public delegate void GetCatalogFailedHandler(object sender);

        #endregion

        #region 事件调用

        /// <summary>
        /// 通知事件获取目录失败
        /// </summary>
        /// <param name="sender"></param>
        protected void TellGetCatalogFail(object sender)
        {
            if (GetCatalogFailed != null) this.GetCatalogFailed(sender);
        }

        /// <summary>
        /// 通知事件获取目录成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_novel"></param>
        protected void TellNovelDirectoryGetted(object sender,Novel _novel)
        {
            if (this.NovelDirectoryGetted != null)
                this.NovelDirectoryGetted(sender, _novel);
        }

        /// <summary>
        /// 通知事件目录列表已更新
        /// </summary>
        /// <param name="sender"></param>
        protected void TellNovelDirectoryListUpdated(object sender)
        {
            if (this.NovelDirectoryListUpdated != null) this.NovelDirectoryListUpdated(sender);
        }

        /// <summary>
        /// 通知事件所有的线程已停止
        /// </summary>
        /// <param name="sender"></param>
        protected void TellHasAllThreadStopped(object sender)
        {
            this.HasAllThreadStopped?.Invoke(sender);
        }

        #endregion

        /// <summary>
        /// 获取章节列表
        /// </summary>
        public void GetDirectoryList()
        {
            Thread thread = new Thread(new ThreadStart(this.GetDirectoryListDelegate));
            thread.IsBackground = true;
            thread.Start();
        }

        private void StartGetContentThread()
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
        /// 开始获取内容
        /// </summary>
        public void StartGetContent()
        {
            foreach (NovelChapter nc in novel.NovelChapters)
            {
                nc.ChapterState = ChapterState.等待中;
            }
            this.StartGetContentThread();
        }

        /// <summary>
        /// 重新获取内容
        /// </summary>
        public void RegetContent()
        {
            foreach (NovelChapter nc in novel.NovelChapters)
            {
                nc.ChapterState = ChapterState.等待中;
            }
            this.StartGetContentThread();
        }

        /// <summary>
        /// 重新获取失败的章节
        /// </summary>
        public void RegetFailedContent()
        {
            foreach (NovelChapter nc in novel.NovelChapters)
            {
                if (nc.ChapterState == ChapterState.获取失败)

                    nc.ChapterState = ChapterState.等待中;
            }
            this.StartGetContentThread();
        }

        /// <summary>
        /// 设置活动线程
        /// </summary>
        /// <param name="count"></param>
        public void ChangeThreadCount(int count)
        {
            this.threadCount = count;
            if (this.isStopDownload) return;
            for (int i = this.activatedThreadCount; i < this.threadCount; i++)
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
            if(novel !=null)
                foreach (NovelChapter nc in novel.NovelChapters)
                {
                    if (nc.ChapterState == ChapterState.等待中)
                        nc.ChapterState = ChapterState.停止;
                }
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
            this.TellNovelDirectoryListUpdated(this);

            return needDownloadChapter;
        }

        /// <summary>
        /// 从网站中获取指定章节的内容,返回章节内容的字节
        /// </summary>
        /// <param name="needDownloadChapter"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private string GetResponseStringFromServer(NovelChapter needDownloadChapter, HttpWebRequest request, HttpWebResponse response)
        {
            Stream dataStream = null;
            StreamReader reader = null;
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
            this.TellNovelDirectoryListUpdated(this);
        }

        /// <summary>
        /// 从网页内容中取出章节图片地址并下载
        /// </summary>
        /// <param name="nc"></param>
        /// <param name="contentString"></param>
        private void DownloadAllChapterImg(NovelChapter nc, string contentString, int fileIndex)
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
                    if (str.ToLower().StartsWith("http://"))
                    {
                        imgPath = str;
                    }
                    else if (str.StartsWith("/"))
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

        private void SaveResponseString(string handledString, NovelChapter nc, int fileIndex)
        {
            FileStream fs = System.IO.File.Create(this.novel.LocalFolder + "\\" + GenerateSaveFileName(fileIndex));
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(handledString);
            sw.Flush();
            sw.Close();
            fs.Close();
            nc.FileName = GenerateSaveFileName(fileIndex);
            this.ChangeGetState(nc, ChapterState.完成);
            
        }

        /// <summary>
        /// 获取章节内容线线程调用
        /// </summary>
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


                if (responseFromServer == null || responseFromServer == "")
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
                            this.SaveResponseString(this.LastHandleResponseString(preHandleString),needDownloadChapter,fileIndex);
           
                        }
                    }


                }

                Thread.Sleep(500);


                //判断是否要退出当前线程
                string threadName = Thread.CurrentThread.Name;
                Trace.Write(threadName);
                Trace.Write(threadName.Substring(16, threadName.Length - 16));
                if (threadName.StartsWith("GetContentThread"))
                    if (int.Parse(threadName.Substring(16, threadName.Length - 16)) > this.threadCount - 1)
                    {
                        break;
                    }

                if (this.isStopDownload) break;

                //如果当前活动线程数量大于已改变线程数量，则当前线程退出
                if (this.activatedThreadCount > this.threadCount)
                    break;

                //继续下一个章节下载
                needDownloadChapter = this.GetNeedDownloadChapter(ref fileIndex);

            }
            lock (this.thisLock2)
            {
                this.activatedThreadCount--;

                if (this.activatedThreadCount == 0)
                    this.TellHasAllThreadStopped(this);
            }



        }

        #region 抽象方法

        /// <summary>
        /// 获取章节列表调用
        /// </summary>
        abstract protected void GetDirectoryListDelegate();
        /// <summary>
        /// 字符串前期处理
        /// </summary>
        /// <param name="responseString"></param>
        /// <returns></returns>
        abstract protected string PreHandleResponseString(string responseString);
        /// <summary>
        /// 字符串后期处理
        /// </summary>
        /// <param name="preHandleString"></param>
        /// <returns></returns>
        abstract protected string LastHandleResponseString(string preHandleString);

        #endregion

    }
}
