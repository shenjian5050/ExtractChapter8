using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace CS.GetChapterUtil
{
    public class GetChapterFrom31xs : GetChapter
    {
        #region 获取目录
        protected override void GetDirectoryListDelegate()
        {
            novel = null;
            novel = new Novel
            {
                NovelAddress = this.address
            };


            WebClient webclient = new WebClient
            {
                Credentials = CredentialCache.DefaultCredentials //获取或设置用于对向Internet资源的请求进行身份验证的网络凭据
            };
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
            string[] str1 = pageHtml.Split(new string[] { "<h1>", "</h1>", "者：", "<dl>", "</dl>"}, StringSplitOptions.RemoveEmptyEntries);
            pageHtml = null;
            if (str1.Length < 5) { this.TellGetCatalogFail(this); return; }
            novel.NovelName = str1[1];
            //获取作者姓名
            string[] str2=str1[3].Split(new string[] { "</p" }, StringSplitOptions.RemoveEmptyEntries);
            if (str2.Length > 1)
                novel.Author = str2[0];

            //获取章节
            string[] str3=str1[4].Split(new string[] { "正文卷" }, StringSplitOptions.RemoveEmptyEntries);
            if (str3.Length < 2) { this.TellGetCatalogFail(this); return; }

            string[] str5 = str3[1].Split(new string[] { "<dd>" }, StringSplitOptions.RemoveEmptyEntries);
            string[] str4 = null;
            for (int i = 1; i < str5.Length; i++)
            {
                str4 = str5[i].Split(new string[] { "href=\"","\">", "</a>" }, StringSplitOptions.RemoveEmptyEntries);
                if (str4.Length > 3)
                {
                    if (!str4[1].StartsWith("javascript"))
                    {
                        novel.AddChapter(str4[2], "http://www.31xs.net" + str4[1]);
                    }
                }
                str4 = null;
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

        protected override string LastHandleResponseString(string preHandleString)
        {
            Regex regex = new Regex(@"www.*[\x00-\xff]");
            return regex.Replace(preHandleString, "");

        }

        protected override string PreHandleResponseString(string responseString)
        {
            //处理
            string[] temp = responseString.Split(new string[] { "<div id=\"content\">" }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 2) return "";

            string[] temp2 = temp[1].Split(new string[] { "</div>" }, StringSplitOptions.RemoveEmptyEntries);
            temp = null;
            //纵横书海www.dodbook.com 字大好看保护你的眼睛
            //string[] temp3 = temp2[0].Split(new string[] { "字大好看保护你的眼睛" }, StringSplitOptions.RemoveEmptyEntries);
            if (temp2.Length < 1) return "";

            string repstr = Environment.NewLine + "　　";
            string result = temp2[0].Replace("&nbsp;", ""); 
            result = result.Replace("<p>    ", repstr);
            result =result.Replace("</p>", ""); 
            return result;
        }
    }
}
