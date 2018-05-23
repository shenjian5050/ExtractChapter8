using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CS.GetChapterUtil
{
    public enum ChapterState
    {
        停止,
        等待中,
        获取中,
        完成,
        获取失败

    }

    [Serializable]
    public class NovelChapter
    {
        string chapterName;
        ChapterState chapterState;
        NovelChapters novelChapters;
        string address;
        string fileName;

        public NovelChapter(string name)
        {
            this.chapterName = name;
            ChapterState = ChapterState.停止;
            novelChapters = new NovelChapters();
        }

        public string ChapterName
        {
            get { return this.chapterName; }
            set { this.chapterName = value; }
        }

        public ChapterState ChapterState
        {
            get { return this.chapterState; }
            set { this.chapterState = value; }
        }

        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        public NovelChapters NovelChapters
        { get { return this.novelChapters; } }

        public string ChapterAddress
        {
            get { return this.address; }
            set { this.address = value; }
        }
    }

    [Serializable]
    public class NovelChapters : BindingList<NovelChapter>, DevExpress.XtraTreeList.TreeList.IVirtualTreeListData
    {

        #region IVirtualTreeListData 成员

        public void VirtualTreeGetCellValue(DevExpress.XtraTreeList.VirtualTreeGetCellValueInfo info)
        {
            NovelChapter nc = info.Node as NovelChapter;
            switch (info.Column.Caption)
            {
                case "章节名":
                    info.CellData = nc.ChapterName;
                    break;
                case "状态":
                    info.CellData = nc.ChapterState.ToString();
                    break;
                case "文件名":
                    info.CellData = nc.FileName;
                    break;

            }
        }

        public void VirtualTreeGetChildNodes(DevExpress.XtraTreeList.VirtualTreeGetChildNodesInfo info)
        {
            NovelChapter nc = info.Node as NovelChapter;
            info.Children = nc.NovelChapters;
        }

        public void VirtualTreeSetCellValue(DevExpress.XtraTreeList.VirtualTreeSetCellValueInfo info)
        {
            NovelChapter nc = info.Node as NovelChapter;
            switch (info.Column.Caption)
            {
                case "章节名":
                    nc.ChapterName = (string)info.NewCellData;
                    break;
                case "状态":
                    switch (info.NewCellData.ToString())
                    {
                        case "等待中":
                            nc.ChapterState = ChapterState.等待中;
                            break;
                        case "获取中":
                            nc.ChapterState = ChapterState.获取中;
                            break;
                        case "完成":
                            nc.ChapterState = ChapterState.完成;
                            break;
                        case "获取失败":
                            nc.ChapterState = ChapterState.获取失败;
                            break;
                    }
                    break;
                case "文件名":
                    nc.FileName = (string)info.NewCellData;
                    break;
            }
        }

        protected override void InsertItem(int index, NovelChapter item)
        {

            base.InsertItem(index, item);
        }

        #endregion
    }

    [Serializable]
    public class Novel
    {
        string novelName;

        string author;

        string address;

        string localFolder;

        NovelChapters novelChapters;

        public string NovelName
        {
            get { return this.novelName; }
            set { this.novelName = value; }
        }

        public string Author
        {
            get { return this.author; }
            set { this.author = value; }
        }

        public NovelChapters NovelChapters
        { get { return this.novelChapters; } }

        public Novel()
        {
            novelChapters = new NovelChapters();
        }

        public string NovelAddress
        {
            get { return this.address; }
            set
            {
                int index = value.LastIndexOf('/');
                if (index < 0)
                {
                    this.address = "";
                }
                else
                {
                    this.address = value.Substring(0, index + 1);
                }
            }
        }

        public string LocalFolder
        {
            get { return this.localFolder; }
            set { this.localFolder = value; }
        }

        public void AddChapter(string chapterName, string address)
        {
            NovelChapter nc = new NovelChapter(chapterName);
            nc.ChapterAddress = address;
            this.novelChapters.Add(nc);
        }

        

    }
}
