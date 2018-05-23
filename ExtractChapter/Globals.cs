using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ExtractChapter
{
    

    public static class Globals
    {
        public static MysqlHelper MysqlHelper = new MysqlHelper();

        public static MainForm mainForm;

        public static List<string> FolderList = new List<string>();

        public static string SavedFolderPath = "";

        public static void SaveSettingToXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement elem = xmlDoc.CreateElement("FolderList");
            elem.InnerText = "test";
            xmlDoc.DocumentElement.AppendChild(elem);
            xmlDoc.Save("Setting.xml");
        }

    }

    
    

}
