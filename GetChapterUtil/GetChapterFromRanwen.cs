using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CS.GetChapterUtil
{
    public class GetChapterFromRanwen : GetChapter
    {


        #region 获取目录

        protected override void GetDirectoryListDelegate()
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
                    this.TellGetCatalogFail(this);
                    return;
                }
            }

            pageData = null;
            //获取小说名称
            string[] str1 = pageHtml.Split(new string[] { "bname", "</h1>", "<h3>", "</h3>", "defaulthtml4" }, StringSplitOptions.RemoveEmptyEntries);
            pageHtml = null;
            if (str1.Length < 6) { this.TellGetCatalogFail(this); return; }


            if (str1[1].Length < 3) { this.TellGetCatalogFail(this); return; }
            novel.NovelName = str1[1].Substring(2, str1[1].Length - 2).Trim();
            if (novel.NovelName.Length <= 0) { this.TellGetCatalogFail(this); return; }

            //作者
            novel.Author = str1[3];

            //抽取章节名称和章节地址
            string[] str2 = str1[5].Split(new string[] { "<table", "</table>" }, StringSplitOptions.RemoveEmptyEntries);
            str1 = null;
            if (str2.Length < 3) { this.TellGetCatalogFail(this); return; }
            string chapterAll = str2[1];
            str2 = null;

            //StringBuilder sbChapters = new StringBuilder(str1[2], str1[2].Length);
            //str1 = null;
            //sbChapters.Replace("\r\n\t\t", string.Empty);
            ////sbChapters.Replace("\x09\x09\x20\x20\x20\x20", string.Empty);

            ////byte[] tempb = Encoding.Default.GetBytes(sbChapters.ToString());
            ////Regex.Replace(str1[2],"[\r\n\t\x09]",
            ////sbChapters.Replace("    ", string.Empty);

            //string chapterAll = sbChapters.ToString();
            //sbChapters = null;
            string[] chapterList = chapterAll.Split(new string[] { "dccss\">" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < chapterList.Length; i++)
            {
                if (chapterList[i].StartsWith("<a href"))
                {
                    //NovelChapter nc = new NovelChapter(
                    string[] temp = chapterList[i].Split(new string[] { "href=\"", "\" alt=\"", "\">" }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length < 3) continue;
                    novel.AddChapter(temp[2], novel.NovelAddress + temp[1]);

                    temp = null;

                }
            }
            //创建以小说名为名称的文件夹
            //this.savedFolder = this.savedFolder + novel.NovelName;
            if (this.savedFolder.EndsWith(@"\"))
                this.novel.LocalFolder = this.savedFolder + novel.NovelName;
            else
                this.novel.LocalFolder = this.savedFolder + "\\" + novel.NovelName;
            if (Directory.Exists(this.novel.LocalFolder))
            { }
            else
            { Directory.CreateDirectory(this.novel.LocalFolder); }

            this.TellNovelDirectoryGetted(this, this.novel);


        }

        #endregion

        #region 章节内容处理方法

        /// <summary>
        /// 对获取的网页源码进行初步的处理,去掉头尾,去掉网站的嵌入代码等
        /// </summary>
        /// <param name="responseString"></param>
        /// <returns></returns>
        protected override string PreHandleResponseString(string responseString)
        {
            //处理
            string[] temp = responseString.Split(new string[] { "<p>", "</p>" }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 3) return "";
            string txt = temp[1].Replace("<br /><br />", Environment.NewLine).Trim();
            txt = txt.Replace("<br />", Environment.NewLine);
            txt = txt.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "　　");
            return txt;
        }

        /// <summary>
        /// 字符串后期处理
        /// </summary>
        /// <param name="preHandleString"></param>
        /// <returns></returns>
        protected override string LastHandleResponseString(string preHandleString)
        {
            return preHandleString;
        }

        #endregion


    }
}
