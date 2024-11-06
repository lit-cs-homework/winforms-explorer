namespace winforms_explorer
{
    partial class MainForm
    {
        /// <summary>
        /// a must for Designer
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// clean resource used
        /// </summary>
        /// <param name="disposing">whether to free managed resource</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Window Designer generated code

        /// <summary>
        /// Designer needed method - DO NOT modify directly
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            splitContainerDirItem = new System.Windows.Forms.SplitContainer();
            treeViewDir = new System.Windows.Forms.TreeView();
            imageListDirView = new System.Windows.Forms.ImageList(components);
            listViewItem = new System.Windows.Forms.ListView();
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            flowLayoutPanelHeader = new System.Windows.Forms.FlowLayoutPanel();
            flowLayoutPanelBtns = new System.Windows.Forms.FlowLayoutPanel();
            btnRefresh = new System.Windows.Forms.Button();
            btnNavParentDir = new System.Windows.Forms.Button();
            flowLayoutPanelInput = new System.Windows.Forms.FlowLayoutPanel();
            textBoxPath = new System.Windows.Forms.TextBox();
            comboBoxLang = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)splitContainerDirItem).BeginInit();
            splitContainerDirItem.Panel1.SuspendLayout();
            splitContainerDirItem.Panel2.SuspendLayout();
            splitContainerDirItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            flowLayoutPanelHeader.SuspendLayout();
            flowLayoutPanelBtns.SuspendLayout();
            flowLayoutPanelInput.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerDirItem
            // 
            splitContainerDirItem.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerDirItem.Location = new System.Drawing.Point(0, 0);
            splitContainerDirItem.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            splitContainerDirItem.Name = "splitContainerDirItem";
            // 
            // splitContainerDirItem.Panel1
            // 
            splitContainerDirItem.Panel1.Controls.Add(treeViewDir);
            // 
            // splitContainerDirItem.Panel2
            // 
            splitContainerDirItem.Panel2.Controls.Add(listViewItem);
            splitContainerDirItem.Size = new System.Drawing.Size(1365, 931);
            splitContainerDirItem.SplitterDistance = 392;
            splitContainerDirItem.SplitterWidth = 5;
            splitContainerDirItem.TabIndex = 0;
            // 
            // treeViewDir
            // 
            treeViewDir.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewDir.ImageIndex = 1;
            treeViewDir.ImageList = imageListDirView;
            treeViewDir.Location = new System.Drawing.Point(0, 0);
            treeViewDir.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            treeViewDir.Name = "treeViewDir";
            treeViewDir.SelectedImageIndex = 0;
            treeViewDir.Size = new System.Drawing.Size(392, 931);
            treeViewDir.TabIndex = 0;
            treeViewDir.AfterCollapse += treeViewDir_AfterCollapse;
            treeViewDir.AfterExpand += treeViewDir_AfterExpand;
            treeViewDir.AfterSelect += treeViewDir_AfterSelect;
            // 
            // imageListDirView
            // 
            imageListDirView.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListDirView.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListDirView.ImageStream");
            imageListDirView.TransparentColor = System.Drawing.Color.Transparent;
            imageListDirView.Images.SetKeyName(0, "disk.png");
            imageListDirView.Images.SetKeyName(1, "imageres 004.png");
            // 
            // listViewItem
            // 
            listViewItem.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewItem.Location = new System.Drawing.Point(0, 0);
            listViewItem.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            listViewItem.MultiSelect = false;
            listViewItem.Name = "listViewItem";
            listViewItem.Size = new System.Drawing.Size(968, 931);
            listViewItem.SmallImageList = imageListDirView;
            listViewItem.TabIndex = 0;
            listViewItem.UseCompatibleStateImageBehavior = false;
            listViewItem.View = System.Windows.Forms.View.List;
            listViewItem.DoubleClick += listViewItem_DoubleClick;
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(0, 0);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            splitContainerMain.Name = "splitContainerMain";
            splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(flowLayoutPanelHeader);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(splitContainerDirItem);
            splitContainerMain.Size = new System.Drawing.Size(1365, 1039);
            splitContainerMain.SplitterDistance = 102;
            splitContainerMain.SplitterWidth = 6;
            splitContainerMain.TabIndex = 1;
            // 
            // flowLayoutPanelHeader
            // 
            flowLayoutPanelHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanelHeader.Controls.Add(flowLayoutPanelBtns);
            flowLayoutPanelHeader.Controls.Add(flowLayoutPanelInput);
            flowLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            flowLayoutPanelHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            flowLayoutPanelHeader.Name = "flowLayoutPanelHeader";
            flowLayoutPanelHeader.Size = new System.Drawing.Size(1365, 102);
            flowLayoutPanelHeader.TabIndex = 5;
            // 
            // flowLayoutPanelBtns
            // 
            flowLayoutPanelBtns.Controls.Add(btnRefresh);
            flowLayoutPanelBtns.Controls.Add(btnNavParentDir);
            flowLayoutPanelBtns.Location = new System.Drawing.Point(4, 5);
            flowLayoutPanelBtns.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            flowLayoutPanelBtns.Name = "flowLayoutPanelBtns";
            flowLayoutPanelBtns.Size = new System.Drawing.Size(130, 84);
            flowLayoutPanelBtns.TabIndex = 4;
            // 
            // btnRefresh
            // 
            btnRefresh.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            btnRefresh.Location = new System.Drawing.Point(8, 10);
            btnRefresh.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(50, 76);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "🗘";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnNavParentDir
            // 
            btnNavParentDir.Location = new System.Drawing.Point(70, 5);
            btnNavParentDir.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnNavParentDir.Name = "btnNavParentDir";
            btnNavParentDir.Size = new System.Drawing.Size(46, 81);
            btnNavParentDir.TabIndex = 2;
            btnNavParentDir.Text = "<";
            btnNavParentDir.UseVisualStyleBackColor = true;
            btnNavParentDir.Click += btnNavParentDir_Click;
            // 
            // flowLayoutPanelInput
            // 
            flowLayoutPanelInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanelInput.Controls.Add(textBoxPath);
            flowLayoutPanelInput.Controls.Add(comboBoxLang);
            flowLayoutPanelInput.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanelInput.Location = new System.Drawing.Point(142, 5);
            flowLayoutPanelInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            flowLayoutPanelInput.Name = "flowLayoutPanelInput";
            flowLayoutPanelInput.Size = new System.Drawing.Size(1166, 84);
            flowLayoutPanelInput.TabIndex = 5;
            // 
            // textBoxPath
            // 
            textBoxPath.Cursor = System.Windows.Forms.Cursors.Help;
            textBoxPath.Location = new System.Drawing.Point(5, 6);
            textBoxPath.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            textBoxPath.Name = "textBoxPath";
            textBoxPath.ReadOnly = true;
            textBoxPath.Size = new System.Drawing.Size(1034, 45);
            textBoxPath.TabIndex = 0;
            textBoxPath.MouseClick += textBoxPath_MouseClick;
            textBoxPath.KeyDown += textBoxPath_KeyDown;
            // 
            // comboBoxLang
            // 
            comboBoxLang.Font = new System.Drawing.Font("SimSun", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            comboBoxLang.FormattingEnabled = true;
            comboBoxLang.Location = new System.Drawing.Point(1048, 5);
            comboBoxLang.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            comboBoxLang.Name = "comboBoxLang";
            comboBoxLang.Size = new System.Drawing.Size(64, 35);
            comboBoxLang.TabIndex = 3;
            comboBoxLang.SelectedIndexChanged += comboBoxLang_SelectedIndexChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(15F, 38F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1365, 1039);
            Controls.Add(splitContainerMain);
            Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            Name = "MainForm";
            Text = "FileMgr";
            Load += MainForm_Load;
            splitContainerDirItem.Panel1.ResumeLayout(false);
            splitContainerDirItem.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerDirItem).EndInit();
            splitContainerDirItem.ResumeLayout(false);
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            flowLayoutPanelHeader.ResumeLayout(false);
            flowLayoutPanelBtns.ResumeLayout(false);
            flowLayoutPanelInput.ResumeLayout(false);
            flowLayoutPanelInput.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerDirItem;
        private System.Windows.Forms.TreeView treeViewDir;
        public System.Windows.Forms.ImageList imageListDirView;
        public System.Windows.Forms.ListView listViewItem;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelHeader;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelBtns;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnNavParentDir;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.ComboBox comboBoxLang;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelInput;
    }
}

