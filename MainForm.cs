using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace file_manage
{
    using static file_manage.Utils;
    public partial class MainForm : Form
    {

        #region utils
        private void ListDrives() =>
            Utils.ListDrives((drive) =>
               {
                   var driveNode = newTreeNodeFromDrive(drive);
                   treeViewDir.Nodes.Add(driveNode);
               }
            );

        #endregion utils
        public MainForm()
        {
            InitializeComponent();
            InitToolTip();
        }
        private void MainForm_Load(object sender, EventArgs e) => ListDrives();
 

        #region treeView
        private void treeViewDirBindedRender(TreeNode node)
        {
            init_treeViewDir_if_needed(node);
            node.ExpandToRoot();
            // bind
            var fullPath = GetFullPath(node);
            listViewItemRender_Uncached(fullPath);
            UpdatePathInput(fullPath);
        }
        private void treeViewDirBindedRender() => treeViewDirBindedRender(treeViewDir.SelectedNode);
        private void treeViewDir_AfterSelect(object sender, TreeViewEventArgs e)
            => treeViewDirBindedRender(e.Node);


        private void PopulateTreeView(string folderPath, TreeNode parentNode)
        {
            // 获取当前文件夹的所有子文件夹
            var succ = TryGetDirectories(out string[] subFolders, folderPath);
            if (!succ) return;
            if (subFolders == null || subFolders.Length == 0) return;
            else
            {
                // 处理当前文件夹              
                foreach (var subFolder in subFolders)
                {
                    var treeNode = newTreeNodeFromDir(subFolder);

                    if (Directory.Exists(subFolder))
                    {
                        parentNode.Nodes.Add(treeNode);
                    }
                    PopulateTreeView(subFolder, treeNode);
                }
            }


        }
        private void treeViewDir_AfterExpand(object _sender, TreeViewEventArgs e)
            => init_treeViewDir_if_needed(e.Node);

        private void treeViewDir_AfterCollapse(object _sender, TreeViewEventArgs e)
        {
            var node = e.Node.Parent;
            if (node == null) return;
            var path = GetFullPath(node);
            listViewItemRender_Uncached(path);

        }
        private void init_treeViewDir_if_needed(TreeNode node)
        {

            if (node.Nodes.Count == 1 && node.Nodes[0].Text == SubDirectoryDummyTag)
            {
                node.Nodes.Clear(); // 移除虚拟子节点

                // 初始化
                var nodePath = GetFullPath(node);

                var succ = TryGetDirectories(out string[] subDirectories, nodePath);
                if (!succ) return;
                foreach (var subDirectory in subDirectories)
                {
                    var subNode = newTreeNodeFromDir(subDirectory);
                    node.Nodes.Add(subNode);
                    
                }
                //ListDirsToNode(nodePath, ref e.Node.Nodes);
                
            }
        }
        #endregion treeView

        #region listView
        private void listViewItemBindedRender()
        {
            ListViewItem selectedListItem = listViewItem.SelectedItems[0];
            var fullPath = GetFullPath(selectedListItem);

            listViewItemRender_Uncached(fullPath);

            // 同步treeView
            var curTreeNode = treeViewDir.SelectedNode;
            var lastPart = selectedListItem.Text;
                
            init_treeViewDir_if_needed(curTreeNode);
            var ls = curTreeNode.Nodes.Find(lastPart, true);
            if (ls.Count() == 0) return;
            curTreeNode = ls[0];

            curTreeNode.ExpandToRoot();
            UpdatePathInput(fullPath);
        }
        private void listViewItem_DoubleClick(object _sender, EventArgs e)
            => listViewItemBindedRender();

        protected void listViewItemRender_Uncached(string fullPath)
        {

            if (File.Exists(fullPath))  // is file
            {
                Process.Start(fullPath);
            }
            else if (Directory.Exists(fullPath))
            {
                listViewItem.Clear();
                var succ = TryGetDirectories(out string[] directories, fullPath);
                if (!succ) return;
                foreach (var directory in directories)
                {
                    var item = newListViewItemFromDir(directory);
                    listViewItem.Items.Add(item);
                }
                string[] files = Directory.GetFiles(fullPath);
                foreach (string file in files)
                {

                    var item = new ListViewItem(Path.GetFileName(file));

                    // Check to see if the image collection contains an image
                    // for this extension, using the extension as a key.
                    var suffix = Path.GetExtension(file);
                    // ref
                    // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-extract-the-icon-associated-with-a-file-in-windows-forms?view=netframeworkdesktop-4.8
                    if (!imageListDirView.Images.ContainsKey(suffix))
                    {
                        // If not, add the image to the image list.
                        // TODO: support dark theme globally, ...
                        //  so that the dark bg icon won't look unsuitable.
                        Icon iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(file);
                        imageListDirView.Images.Add(suffix, iconForFile);
                    }
                    item.ImageKey = suffix;

                    SetFullPath(item, file);

                    listViewItem.Items.Add(item);
                }
            }
        }
        #endregion listView

        #region textBoxPath
        protected void UpdatePathInput(string fullPath) =>
            textBoxPath.Text = fullPath;

        // unimpl yet
        // TODO: rm ReadOnly attr of textBoxPath, and allow edit to navigate.
        private void textBoxPath_KeyDown(object sender, KeyEventArgs e)
        {
        /*
            if (e.KeyCode == Keys.Enter)
            {
                RecurviseUpdateView();
                e.Handled = true;
            }
        */
        }



        private void textBoxPath_MouseClick(object sender, MouseEventArgs e)
            => Clipboard.SetText(textBoxPath.Text);
        #endregion textBoxPath

        #region NavBtn

        /// <summary>
        /// navigate to parent directory, if any.
        /// </summary>
        private void btnNavParentDir_Click(object sender, EventArgs e)
        {
            var curTreeNode = treeViewDir.SelectedNode;
            if (curTreeNode == null) return;
            curTreeNode.Collapse();
            curTreeNode = curTreeNode.Parent;
            if (curTreeNode == null) return;
            treeViewDir.SelectedNode = curTreeNode;
            treeViewDirBindedRender(curTreeNode);
        }
        #endregion NavBtn

        #region ToopTip

        private ToolTip newToopTip(int delay) =>
            new ToolTip()
            {
                AutoPopDelay = 5000,
                InitialDelay = delay,
                ReshowDelay = 500,
                ShowAlways = true,
            };

        protected void InitToolTip()
        {
            var toolTip = newToopTip(700);
            toolTip.SetToolTip(btnRefresh, "刷新");
            toolTip.SetToolTip(btnNavParentDir, "上一级目录");
            newToopTip(100).SetToolTip(textBoxPath, "点击复制");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var curTreeNode = treeViewDir.SelectedNode;
            if (curTreeNode == null) // if we've just started this app, haven't entered any directory
                return;
            curTreeNode.Nodes.Clear();
            curTreeNode.Nodes.Add(SubDirectoryDummyTag);
            treeViewDirBindedRender(curTreeNode);

        }
        #endregion ToopTip

    }
}
