namespace file_manage
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainerDirItem = new System.Windows.Forms.SplitContainer();
            this.treeViewDir = new System.Windows.Forms.TreeView();
            this.imageListDirView = new System.Windows.Forms.ImageList(this.components);
            this.listViewItem = new System.Windows.Forms.ListView();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.groupBoxHeader = new System.Windows.Forms.GroupBox();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDirItem)).BeginInit();
            this.splitContainerDirItem.Panel1.SuspendLayout();
            this.splitContainerDirItem.Panel2.SuspendLayout();
            this.splitContainerDirItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.groupBoxHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerDirItem
            // 
            this.splitContainerDirItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDirItem.Location = new System.Drawing.Point(0, 0);
            this.splitContainerDirItem.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainerDirItem.Name = "splitContainerDirItem";
            // 
            // splitContainerDirItem.Panel1
            // 
            this.splitContainerDirItem.Panel1.Controls.Add(this.treeViewDir);
            // 
            // splitContainerDirItem.Panel2
            // 
            this.splitContainerDirItem.Panel2.Controls.Add(this.listViewItem);
            this.splitContainerDirItem.Size = new System.Drawing.Size(1093, 583);
            this.splitContainerDirItem.SplitterDistance = 316;
            this.splitContainerDirItem.SplitterWidth = 5;
            this.splitContainerDirItem.TabIndex = 0;
            // 
            // treeViewDir
            // 
            this.treeViewDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDir.ImageIndex = 2;
            this.treeViewDir.ImageList = this.imageListDirView;
            this.treeViewDir.Location = new System.Drawing.Point(0, 0);
            this.treeViewDir.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewDir.Name = "treeViewDir";
            this.treeViewDir.SelectedImageIndex = 0;
            this.treeViewDir.Size = new System.Drawing.Size(316, 583);
            this.treeViewDir.TabIndex = 0;
            this.treeViewDir.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDir_AfterExpand);
            this.treeViewDir.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDir_AfterSelect);
            // 
            // imageListDirView
            // 
            this.imageListDirView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDirView.ImageStream")));
            this.imageListDirView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListDirView.Images.SetKeyName(0, "disk.png");
            this.imageListDirView.Images.SetKeyName(1, "imageres 004.png");
            this.imageListDirView.Images.SetKeyName(2, "imageres 006.png");
            this.imageListDirView.Images.SetKeyName(3, "txt-ico.jpg");
            // 
            // listViewItem
            // 
            this.listViewItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewItem.HideSelection = false;
            this.listViewItem.Location = new System.Drawing.Point(0, 0);
            this.listViewItem.Margin = new System.Windows.Forms.Padding(4);
            this.listViewItem.MultiSelect = false;
            this.listViewItem.Name = "listViewItem";
            this.listViewItem.Size = new System.Drawing.Size(772, 583);
            this.listViewItem.SmallImageList = this.imageListDirView;
            this.listViewItem.TabIndex = 0;
            this.listViewItem.UseCompatibleStateImageBehavior = false;
            this.listViewItem.View = System.Windows.Forms.View.List;
            this.listViewItem.DoubleClick += new System.EventHandler(this.listViewItem_DoubleClick);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.groupBoxHeader);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerDirItem);
            this.splitContainerMain.Size = new System.Drawing.Size(1093, 656);
            this.splitContainerMain.SplitterDistance = 69;
            this.splitContainerMain.TabIndex = 1;
            // 
            // groupBoxHeader
            // 
            this.groupBoxHeader.Controls.Add(this.textBoxPath);
            this.groupBoxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxHeader.Location = new System.Drawing.Point(0, 0);
            this.groupBoxHeader.Name = "groupBoxHeader";
            this.groupBoxHeader.Size = new System.Drawing.Size(1093, 69);
            this.groupBoxHeader.TabIndex = 1;
            this.groupBoxHeader.TabStop = false;
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(125, 3);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(890, 35);
            this.textBoxPath.TabIndex = 0;
            this.textBoxPath.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1093, 656);
            this.Controls.Add(this.splitContainerMain);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "FileMgr";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainerDirItem.Panel1.ResumeLayout(false);
            this.splitContainerDirItem.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDirItem)).EndInit();
            this.splitContainerDirItem.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.groupBoxHeader.ResumeLayout(false);
            this.groupBoxHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerDirItem;
        private System.Windows.Forms.TreeView treeViewDir;
        public System.Windows.Forms.ImageList imageListDirView;
        public System.Windows.Forms.ListView listViewItem;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.GroupBox groupBoxHeader;
        private System.Windows.Forms.TextBox textBoxPath;
    }
}

