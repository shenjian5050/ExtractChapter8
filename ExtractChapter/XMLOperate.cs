using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using CS.GetChapterUtil;

namespace ExtractChapter
{
    internal class XMLOperate
    {
        public void CreateXML(string filename)
        {
            XmlTextWriter write = new XmlTextWriter(filename, Encoding.Default);
            //使用自动缩进
            write.Formatting = Formatting.Indented;
            //写入根元素
            write.WriteStartElement("Chapters");

            //关闭根元素,并写入结束标签
            write.WriteEndElement();

            write.Close();

        }

        public void AddChapter(string name, ChapterState state, string localfilename)
        {
            
        }
    }
}
