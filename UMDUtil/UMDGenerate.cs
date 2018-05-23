using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UMDUtil
{




    public class UMDGenerate
    {
        #region 变量

        /*
         * UMD文件总体上是由一组连续的块组成的，每一块按照约定的顺序先后排列在一起构成了UMD文件的结构。
         * 根据块的职责，我将其分成两类：功能块和数据块。有的功能块自身就可以完全的描述信息，而有的一些
         * 由于信息量大，特别地将数据放在别处（即数据块），如正文、章节偏移和章节标题，它们使用一个功能
         * 块和若干个数据块，通常数据块都紧接在相应的功能块之后出现
         */


        #region 块标识

        /// <summary>
        /// umd文件头
        /// </summary>
        private const short DCTS_CMD_ID_VERSION = 0x01;
        /// <summary>
        /// 文件标题
        /// </summary>
        private const short DCTS_CMD_ID_TITLE = 0x02;
        /// <summary>
        /// 作者
        /// </summary>
        private const short DCTS_CMD_ID_AUTHOR = 0x03;
        /// <summary>
        /// 年
        /// </summary>
        private const short DCTS_CMD_ID_YEAR = 0x04;
        /// <summary>
        /// 月
        /// </summary>
        private const short DCTS_CMD_ID_MONTH = 0x05;
        /// <summary>
        /// 日
        /// </summary>
        private const short DCTS_CMD_ID_DAY = 0x06;
        /// <summary>
        /// 小说类型
        /// </summary>
        private const short DCTS_CMD_ID_GENDER = 0x07;
        /// <summary>
        /// 出版商
        /// </summary>
        private const short DCTS_CMD_ID_PUBLISHER = 0x08;
        /// <summary>
        /// 零售商
        /// </summary>
        private const short DCTS_CMD_ID_VENDOR = 0x09;
        /// <summary>
        /// CONTENT ID
        /// </summary>
        private const short DCTS_CMD_ID_CONTENT_ID = 10;
        /// <summary>
        /// 内容长度
        /// </summary>
        private const short DCTS_CMD_ID_FILE_LENGTH = 11;
        /// <summary>
        /// 文件结束
        /// </summary>
        private const short DCTS_CMD_ID_FIXED_LEN = 12;
        /// <summary>
        /// 正文
        /// </summary>
        private const short DCTS_CMD_ID_REF_CONTENT = 0x81;
        /// <summary>
        /// 封面
        /// </summary>
        private const short DCTS_CMD_ID_COVER_PAGE = 0x82;
        /// <summary>
        /// 章节偏移
        /// </summary>
        private const short DCTS_CMD_ID_CHAP_OFF = 0x83;
        /// <summary>
        /// 章节标题，正文
        /// </summary>
        private const short DCTS_CMD_ID_CHAP_STR = 0x84;
        /// <summary>
        /// 页面偏移（Page Offset）
        /// </summary>
        private const short DCTS_CMD_ID_PAGE_OFFSET = 0x87;
        /// <summary>
        /// CDS KEY
        /// </summary>
        private const short DCTS_CMD_ID_CDS_KEY = 0xf0;
        /// <summary>
        /// 许可证(LICENCE KEY)
        /// </summary>
        private const short DCTS_CMD_ID_LICENSE_KEY = 0xf1;

        #endregion






        #endregion

        #region 构造


        #endregion

        #region 私有方法

        /// <summary>
        /// 写入文件头[四个字节]
        /// </summary>
        /// <param name="writer"></param>
        private void Write_UMD_FileHeader(ref BinaryWriter writer)
        {
            writer.Write((uint)0xde9a9b89);
        }

        /// <summary>
        /// 块标记0x01
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="isTxt"></param>
        private void Write_UMD_FileVersion(ref BinaryWriter writer, bool isTxt)
        {
            writer.Write('#');
            writer.Write((short)1);
            writer.Write((byte)8);
            writer.Write((byte)1);

            //UMD文件类型（1-文本，2-漫画）
            if (isTxt) 
                writer.Write((byte)1);//文本
            else
                writer.Write((byte)2);//漫画书
            Random random1 = new Random();
            writer.Write((short)(random1.Next(0x401, 0x7fff) % 0xffff));//两字节的随机数
        }

        /// <summary>
        /// 文件标题0x02
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="title"></param>
        private void Write_UMD_Title(ref BinaryWriter writer,string title)
        {
            writer.Write('#');
            writer.Write((short)2);
            writer.Write((byte)0);
            writer.Write((byte)(5 + (title.Length * 2)));
            writer.Write(Encoding.Unicode.GetBytes(title));
        }

        private void Write_UMD_Author(ref BinaryWriter writer, string author)
        {
 
        }



        #endregion

        #region 公共方法


        #endregion

    }
}
