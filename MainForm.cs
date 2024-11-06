using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace file_manage
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
            InitToolTip();
        }

        #region utils

        /// <summary>
        /// list drives
        /// </summary>
        protected void ListDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                switch (drive.DriveType)
                {
                    // 磁盘驱动器
                    case DriveType.Fixed:
                    case DriveType.Removable:
                        {
                            var driveNode = newTreeNodeFromDrive(drive);
                            treeViewDir.Nodes.Add(driveNode);
                        }
                        break;
                    case DriveType.Network:
                        {
                            /* TODO: will GetDrives yield Network drive? To either: ...
                             * - inspect if GetDrives won't return Network (as I've seen, not sure
                             * - rm this case-clause
                             */
                        }
                        break;
                }
            }
        }

        // a few hard-coded default icon for non-file directory entry
        public enum ImageIndex
        {
            Drive, Dir,
        }
        public static int Ord(ImageIndex index) => (int)index;

        protected static void Error(string msg) => MessageBox.Show(msg);
        protected static void Error(string msg, string title) => MessageBox.Show(msg, title);

        protected static bool TryGetDirectories(out string[] res, string path)
        {
            string title = "错误";
            string msg;
            try
            {
                res = Directory.GetDirectories(path);
                return true;
            }
            catch (UnauthorizedAccessException exc)
            {
                msg = exc.Message;
                title = "权限不足";
            }
            catch (DirectoryNotFoundException exc)
            {
                msg = exc.Message;
                title = "目录不存在";
            }
            catch (PathTooLongException exc)
            {
                msg = exc.Message;
                title = "路径过长，无法支持";
            }
            catch (NotSupportedException exc)
            { 
                msg = exc.Message;
                msg += ' ';
                msg += path;
                title = "路径参数格式错误";
            }
            catch (Exception e)
            {
                msg = e.Message;
                msg += '\n';
                msg += e.GetType().ToString();
            }
            Error(msg, title);
            res = new string[0];
            return false;
        }

        protected static ListViewItem newListViewItemFromDir(string directory, ImageIndex imIdx=ImageIndex.Dir)
        {
            var name = new DirectoryInfo(directory).Name;
            var res = new ListViewItem(name, Ord(imIdx));
            res.Name = name;
            SetFullPath(res, directory);
            return res;
        }
        protected static TreeNode newTreeNodeFromDir(string directory, ImageIndex imIdx=ImageIndex.Dir)
        {
            var item = newListViewItemFromDir(directory, imIdx);
            var subNode = new TreeNode(item.Text, item.ImageIndex, item.ImageIndex)
            {
                Name = item.Name
            };

            SetFullPath(subNode, GetFullPath(item));
            subNode.Nodes.Add(SubDirectoryDummyTag); // 添加一个虚拟子节点，表示未初始化（未将entry存为nodes) (与已检查的空文件夹区分)
            return subNode;
        }
        protected static TreeNode newTreeNodeFromDrive(DriveInfo drive)
        {
            var res = newTreeNodeFromDir(drive.Name, ImageIndex.Drive);
            var name = drive.Name.TrimEnd('\\', '/');  // 去除 分隔符后缀
            var text = $"{drive.VolumeLabel} ({name})"; // 模仿 Windows Explorer 行为
            res.Text = text;
            return res;
        }
        protected static string GetFullPath(TreeNode     node) => node.Tag.ToString();
        protected static string GetFullPath(ListViewItem node) => node.Tag.ToString();
        protected static void   SetFullPath(TreeNode     node, string fullPath) => node.Tag = fullPath;
        protected static void   SetFullPath(ListViewItem node, string fullPath) => node.Tag = fullPath;
        
        /// <summary>
        /// 虚拟子节点名称，表示parent未初始化（未将entry存为nodes),
        /// 因为Tag存的是abspath，所以不可能有其他项的 Tag 为 "Dummy"
        /// </summary>
        protected static readonly string SubDirectoryDummyTag = "Dummy";
        #endregion utils

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
