namespace ExtractChapter
{
    partial class Chapters
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.treeListDirectory = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumnChapterName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumnState = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumnFileName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.memoEditTxt = new DevExpress.XtraEditors.MemoEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barTool = new DevExpress.XtraBars.Bar();
            this.bbiSaveModifiedTxt = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.xtraScrollableControlImg = new DevExpress.XtraEditors.XtraScrollableControl();
            this.styleController1 = new DevExpress.XtraEditors.StyleController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListDirectory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraTabControl1.Location = new System.Drawing.Point(15, 15);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage2;
            this.xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControl1.Size = new System.Drawing.Size(1170, 623);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage2});
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.splitContainerControl1);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1164, 617);
            this.xtraTabPage2.Text = "xtraTabPage2";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.treeListDirectory);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.memoEditTxt);
            this.splitContainerControl1.Panel2.Controls.Add(this.xtraScrollableControlImg);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1164, 617);
            this.splitContainerControl1.SplitterPosition = 331;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // treeListDirectory
            // 
            this.treeListDirectory.Appearance.FocusedRow.BackColor = System.Drawing.Color.MediumTurquoise;
            this.treeListDirectory.Appearance.FocusedRow.Options.UseBackColor = true;
            this.treeListDirectory.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.MediumTurquoise;
            this.treeListDirectory.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.treeListDirectory.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.treeListDirectory.Appearance.SelectedRow.Options.UseBackColor = true;
            this.treeListDirectory.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumnChapterName,
            this.treeListColumnState,
            this.treeListColumnFileName});
            this.treeListDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListDirectory.Location = new System.Drawing.Point(0, 0);
            this.treeListDirectory.Name = "treeListDirectory";
            this.treeListDirectory.OptionsBehavior.Editable = false;
            this.treeListDirectory.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeListDirectory.Size = new System.Drawing.Size(331, 617);
            this.treeListDirectory.TabIndex = 0;
            this.treeListDirectory.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListDirectory_FocusedNodeChanged);
            this.treeListDirectory.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeListDirectory_MouseClick);
            // 
            // treeListColumnChapterName
            // 
            this.treeListColumnChapterName.Caption = "章节名";
            this.treeListColumnChapterName.FieldName = "章节名";
            this.treeListColumnChapterName.Name = "treeListColumnChapterName";
            this.treeListColumnChapterName.Visible = true;
            this.treeListColumnChapterName.VisibleIndex = 0;
            this.treeListColumnChapterName.Width = 185;
            // 
            // treeListColumnState
            // 
            this.treeListColumnState.Caption = "状态";
            this.treeListColumnState.FieldName = "状态";
            this.treeListColumnState.Name = "treeListColumnState";
            this.treeListColumnState.Visible = true;
            this.treeListColumnState.VisibleIndex = 1;
            this.treeListColumnState.Width = 74;
            // 
            // treeListColumnFileName
            // 
            this.treeListColumnFileName.Caption = "文件名";
            this.treeListColumnFileName.FieldName = "文件名";
            this.treeListColumnFileName.Name = "treeListColumnFileName";
            this.treeListColumnFileName.Visible = true;
            this.treeListColumnFileName.VisibleIndex = 2;
            // 
            // memoEditTxt
            // 
            this.memoEditTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoEditTxt.Location = new System.Drawing.Point(0, 0);
            this.memoEditTxt.MenuManager = this.barManager1;
            this.memoEditTxt.Name = "memoEditTxt";
            this.barManager1.SetPopupContextMenu(this.memoEditTxt, this.popupMenu1);
            this.memoEditTxt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoEditTxt.Properties.Appearance.Options.UseFont = true;
            this.memoEditTxt.Size = new System.Drawing.Size(828, 617);
            this.memoEditTxt.TabIndex = 0;
            this.memoEditTxt.EditValueChanged += new System.EventHandler(this.memoEditTxt_EditValueChanged);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barTool});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.bbiSaveModifiedTxt});
            this.barManager1.MainMenu = this.barTool;
            this.barManager1.MaxItemId = 1;
            // 
            // popupMenu1
            // 
            this.popupMenu1.Name = "popupMenu1";
            // 
            // barTool
            // 
            this.barTool.BarName = "Main menu";
            this.barTool.DockCol = 0;
            this.barTool.DockRow = 0;
            this.barTool.FloatLocation = new System.Drawing.Point(620, 219);
            this.barTool.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiSaveModifiedTxt)});
            this.barTool.OptionsBar.MultiLine = true;
            this.barTool.OptionsBar.UseWholeRow = true;
            this.barTool.Text = "工具栏";
            this.barTool.Visible = false;
            // 
            // bbiSaveModifiedTxt
            // 
            this.bbiSaveModifiedTxt.Caption = "保存";
            this.bbiSaveModifiedTxt.Id = 0;
            this.bbiSaveModifiedTxt.Name = "bbiSaveModifiedTxt";
            this.bbiSaveModifiedTxt.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiSaveModifiedTxt_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1199, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 652);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1199, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 652);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1199, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 652);
            // 
            // xtraScrollableControlImg
            // 
            this.xtraScrollableControlImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraScrollableControlImg.Location = new System.Drawing.Point(0, 0);
            this.xtraScrollableControlImg.Name = "xtraScrollableControlImg";
            this.xtraScrollableControlImg.Size = new System.Drawing.Size(828, 617);
            this.xtraScrollableControlImg.TabIndex = 1;
            // 
            // Chapters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 652);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "Chapters";
            this.Text = "Chapters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Chapters_FormClosing);
            this.Load += new System.EventHandler(this.Chapters_Load);
            this.Shown += new System.EventHandler(this.Chapters_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Chapters_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListDirectory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraTreeList.TreeList treeListDirectory;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumnChapterName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumnState;
        private DevExpress.XtraEditors.MemoEdit memoEditTxt;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumnFileName;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraEditors.StyleController styleController1;
        private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControlImg;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar barTool;
        private DevExpress.XtraBars.BarButtonItem bbiSaveModifiedTxt;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}