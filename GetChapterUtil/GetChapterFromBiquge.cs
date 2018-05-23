using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace CS.GetChapterUtil
{
    public class GetChapterFromBiquge:GetChapter
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
            string[] str1 = pageHtml.Split(new string[] { "<h1>", "</h1>", "作&nbsp;&nbsp;&nbsp;&nbsp;者：" }, StringSplitOptions.RemoveEmptyEntries);
            pageHtml = null;
            if (str1.Length < 4) { this.TellGetCatalogFail(this); return; }
            novel.NovelName = str1[1];
            //获取作者姓名
            int tempIndex = str1[3].IndexOf("</p>", 0);
            novel.Author = str1[3].Substring(0, tempIndex);
            string[] str2 = str1[3].Split(new string[] { "<dl>", "</dl>" }, StringSplitOptions.RemoveEmptyEntries);
            str1 = null;
            if (str2.Length < 3) { this.TellGetCatalogFail(this); return; }
            string[] str3 = str2[1].Split(new string[] { "<dd>", "</dd>" }, StringSplitOptions.RemoveEmptyEntries);
            str2 = null;
            if (str3.Length < 1) { this.TellGetCatalogFail(this); return; }
            for (int i = 0; i < str3.Length; i++)
            {
                if (str3[i].StartsWith("<a href=") && str3[i].EndsWith("</a>"))
                {
                    string[] str4 = str3[i].Split(new string[] { "href=", "title=", ">" }, StringSplitOptions.RemoveEmptyEntries);
                    novel.AddChapter(str4[2].Substring(1, str4[2].Length - 2), "http://www.biquge.com" + str4[1].Substring(1, str4[1].Length - 3));
                    str4 = null;
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

        protected override string PreHandleResponseString(string responseString)
        {
            string[] temp = responseString.Split(new string[] { "<div id=\"content\">", "<script>$('.divimage img'" }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length > 2)
            {
                string temp2 = temp[1];
                temp = null;
                temp2 = temp2.Replace("<br />", Environment.NewLine);
                temp2 = temp2.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "    ");
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
