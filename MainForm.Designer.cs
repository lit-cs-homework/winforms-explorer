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
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listViewItem = new System.Windows.Forms.ListView();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDirItem)).BeginInit();
            this.splitContainerDirItem.Panel1.SuspendLayout();
            this.splitContainerDirItem.Panel2.SuspendLayout();
            this.splitContainerDirItem.SuspendLayout();
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
            this.splitContainerDirItem.Size = new System.Drawing.Size(1067, 600);
            this.splitContainerDirItem.SplitterDistance = 309;
            this.splitContainerDirItem.SplitterWidth = 5;
            this.splitContainerDirItem.TabIndex = 0;
            // 
            // treeViewDir
            // 
            this.treeViewDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDir.ImageIndex = 2;
            this.treeViewDir.ImageList = this.imageList1;
            this.treeViewDir.Location = new System.Drawing.Point(0, 0);
            this.treeViewDir.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewDir.Name = "treeViewDir";
            this.treeViewDir.SelectedImageIndex = 0;
            this.treeViewDir.Size = new System.Drawing.Size(309, 600);
            this.treeViewDir.TabIndex = 0;
            this.treeViewDir.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeViewDir.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeViewDir.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "imageres 004.png");
            this.imageList1.Images.SetKeyName(1, "imageres 006.png");
            this.imageList1.Images.SetKeyName(2, "disk.png");
            // 
            // listViewItem
            // 
            this.listViewItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewItem.HideSelection = false;
            this.listViewItem.Location = new System.Drawing.Point(0, 0);
            this.listViewItem.Margin = new System.Windows.Forms.Padding(4);
            this.listViewItem.MultiSelect = false;
            this.listViewItem.Name = "listViewItem";
            this.listViewItem.Size = new System.Drawing.Size(753, 600);
            this.listViewItem.SmallImageList = this.imageList2;
            this.listViewItem.TabIndex = 0;
            this.listViewItem.UseCompatibleStateImageBehavior = false;
            this.listViewItem.View = System.Windows.Forms.View.List;
            this.listViewItem.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listViewItem.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.SystemColors.InfoText;
            this.imageList2.Images.SetKeyName(0, "imageres 004.png");
            this.imageList2.Images.SetKeyName(1, "imageres 098.png");
            this.imageList2.Images.SetKeyName(2, "imageres 166.png");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 600);
            this.Controls.Add(this.splitContainerDirItem);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "FileMgr";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainerDirItem.Panel1.ResumeLayout(false);
            this.splitContainerDirItem.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDirItem)).EndInit();
            this.splitContainerDirItem.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerDirItem;
        private System.Windows.Forms.TreeView treeViewDir;
        public System.Windows.Forms.ImageList imageList1;
        public System.Windows.Forms.ImageList imageList2;
        public System.Windows.Forms.ListView listViewItem;
    }
}

