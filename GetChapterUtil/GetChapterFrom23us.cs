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
    public class GetChapterFrom23us:GetChapter
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
            string[] str1 = pageHtml.Split(new string[] { "</a> -&gt;<a href=", "</a></dt><dd><h1>", "<h3>作者：", "</h3></dd><dd>" }, StringSplitOptions.RemoveEmptyEntries);
            pageHtml = null;
            if (str1.Length < 5) { this.TellGetCatalogFail(this); return; }
            string[] str2 = str1[1].Split(new string[] { ">" }, StringSplitOptions.RemoveEmptyEntries);
            if(str2.Length>1)
                novel.NovelName = str2[1];
            //获取作者姓名
            novel.Author = str1[3];
            string[] str3 = str1[4].Split(new string[] { "<td class=\"L\"><a href=\"" }, StringSplitOptions.RemoveEmptyEntries);

            string[] str4 = null;
            for (int i = 0; i < str3.Length; i++)
            {
                str4 = str3[i].Split(new string[] { "\">", "</a>" }, StringSplitOptions.RemoveEmptyEntries);
                if (str4.Length > 2)
                {
                    if (novel.NovelAddress.EndsWith("/"))
                        novel.AddChapter(str4[1], novel.NovelAddress + str4[0]);
                    else
                        novel.AddChapter(str4[1], novel.NovelAddress + "/" + str4[0]);
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

        protected override string PreHandleResponseString(string responseString)
        {
            string[] temp = responseString.Split(new string[] { "<div id=\"content\">", "<div class=\"adhtml\">" }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length > 2)
            {
                string temp2 = temp[1];
                temp = null;
                temp2 = temp2.Replace("<br />", Environment.NewLine);
                temp2 = temp2.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "    ");
                temp2 = temp2.Replace("</dd>",string.Empty);
                return temp2;
            }
            else
                return "";


        }

        protected override string LastHandleResponseString(string preHandleString)
        {
            return preHandleString;
        }
    }
}
