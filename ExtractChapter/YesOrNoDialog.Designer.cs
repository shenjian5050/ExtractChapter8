namespace ExtractChapter
{
    partial class YesOrNoDialog
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
            this.sbYes = new DevExpress.XtraEditors.SimpleButton();
            this.sbNo = new DevExpress.XtraEditors.SimpleButton();
            this.lcNote = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // sbYes
            // 
            this.sbYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.sbYes.Location = new System.Drawing.Point(69, 73);
            this.sbYes.Name = "sbYes";
            this.sbYes.Size = new System.Drawing.Size(75, 23);
            this.sbYes.TabIndex = 0;
            this.sbYes.Text = "是";
            // 
            // sbNo
            // 
            this.sbNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.sbNo.Location = new System.Drawing.Point(190, 73);
            this.sbNo.Name = "sbNo";
            this.sbNo.Size = new System.Drawing.Size(75, 23);
            this.sbNo.TabIndex = 1;
            this.sbNo.Text = "否";
            // 
            // lcNote
            // 
            this.lcNote.Location = new System.Drawing.Point(40, 36);
            this.lcNote.Name = "lcNote";
            this.lcNote.Size = new System.Drawing.Size(70, 14);
            this.lcNote.TabIndex = 2;
            this.lcNote.Text = "labelControl1";
            // 
            // YesOrNoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 113);
            this.Controls.Add(this.lcNote);
            this.Controls.Add(this.sbNo);
            this.Controls.Add(this.sbYes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YesOrNoDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "YesOrNoDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbYes;
        private DevExpress.XtraEditors.SimpleButton sbNo;
        private DevExpress.XtraEditors.LabelControl lcNote;
    }
}