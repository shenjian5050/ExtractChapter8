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
    public class GetChapterFromDodbook : GetChapter
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
            string[] str1 = pageHtml.Split(new string[] { "booktitle", "<ul>", "</ul>" }, StringSplitOptions.RemoveEmptyEntries);
            pageHtml = null;
            if (str1.Length < 1) { this.TellGetCatalogFail(this); return; }
            int index = str1[1].IndexOf("</span");
            if (index < 2) { this.TellGetCatalogFail(this); return; }
            novel.NovelName = str1[1].Substring(2, index - 2).Trim();
            if (novel.NovelName.Length <= 0) { this.TellGetCatalogFail(this); return; }
            index = str1[1].IndexOf("aspx");
            int index1 = str1[1].IndexOf("</a>");
            if (index < 0 || index1 < 0 || index1 < index + 6) { this.TellGetCatalogFail(this); return; }
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

        /// <summary>
        /// 后期再处理
        /// </summary>
        /// <param name="preHandleString"></param>
        /// <returns></returns>
        protected override string LastHandleResponseString(string preHandleString)
        {
            string[] temp = preHandleString.Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries);
            string strForSave = temp[0].Replace("<div>", string.Empty);
            temp = null;
            strForSave = strForSave.Replace("纵横书海", string.Empty);
            strForSave = strForSave.Replace("www.dodbook.com", string.Empty);
            return strForSave;
        }

        #endregion




    }
}
