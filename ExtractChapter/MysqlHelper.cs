using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace ExtractChapter
{
    public class MysqlHelper
    {
        public static MySqlConnection mysqlConn;
        public static bool MySqlConnect()
        {
            if (mysqlConn == null)
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                try
                {
                    builder.Server = "rm-bp1me5qr49k7i0b7fo.mysql.rds.aliyuncs.com";
                    builder.UserID = "shenjian5050";
                    builder.Password = "Sjloveplf2005";
                    builder.Database = "nvl";
                    builder.Port = 3306;
                    mysqlConn = new MySqlConnection(builder.ConnectionString);
                    mysqlConn.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                if (mysqlConn.State == System.Data.ConnectionState.Open)
                    return true;
                else
                {
                    mysqlConn.Open();
                    return true;
                }
            }
        }

        public bool InsertNewNovel(string novelName, string novelAuthor)
        {
            if (MySqlConnect())
            {
                MySqlCommand cmd = mysqlConn.CreateCommand();
                cmd.CommandText = "Insert Into `novels` (`NovelName`,`NovelAuthor`) values ('" + novelName + "','" + novelAuthor + "');";
                if (cmd.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;

        }



        public int GetNovelID(string novelName, string novelAuthor)
        {
            if (MySqlConnect())
            {
                MySqlCommand cmd = mysqlConn.CreateCommand();
                cmd.CommandText = "Select NovelId From `novels` Where `NovelName`='" + novelName + "' and `NovelAuthor`= '" + novelAuthor + "';";
                object obj = cmd.ExecuteScalar();
                if (obj != null)

                    return Convert.ToInt32(obj);
                else
                    return -1;
            }
            else
                return -1;
        }

        public bool IsExistChapterID(int novelID, int chapterID)
        {
            if (MySqlConnect())
            {
                MySqlCommand cmd = mysqlConn.CreateCommand();
                cmd.CommandText = "Select count(*) From `Chapters` Where `NovelId`=" + novelID + " and `ChapterID`= " + chapterID + ";";
                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    return true;
            }
            return false;
        }


        public bool UpdateChapter(int novelID, int chapterID, string chapterName, string chapterContent)
        {
            if (MySqlConnect())
            {
                MySqlCommand cmd = mysqlConn.CreateCommand();
                cmd.CommandText = "Update `Chapters` Set `ChapterName`='" + chapterName + "',`ChapterContent`='" + chapterContent + "' where `NovelId`=" + novelID + " and `ChapterID`=" + chapterID + ";";
                if (cmd.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public bool InsertNewChapter(int novelID, int chapterID,string chapterName, string chapterContent)
        {

            if (MySqlConnect())
            {
                MySqlCommand cmd = mysqlConn.CreateCommand();
                cmd.CommandText = "Insert Into `Chapters` (`NovelId`,`ChapterID`,`ChapterName`,`ChapterContent`) values (" + novelID + "," + chapterID + ",'" + chapterName + "','" + chapterContent + "');";
                if (cmd.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;

        }

    }
}
