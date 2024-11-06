using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace file_manage
{
    public enum ImageIndex
    {
        Drive,
        Dir,
        Archive,
        File,
    }
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
            InitRefreshBtnToolTip();
        }
        #region utils
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
            //TreeNode subNode = e.Node.Nodes.Add(directoryInfo.Name, directoryInfo.Name, Ord(ImageIndex.Dir);
            //subNode.Tag = directoryInfo.FullName; // 保存路径信息到节点的 Tag 属性

            SetFullPath(subNode, GetFullPath(item));
            subNode.Nodes.Add(SubDirectoryDummyTag); // 添加一个虚拟子节点，表示未初始化（未将entry存为nodes) (与已检查的空文件夹区分)
            return subNode;
        }
        protected static TreeNode newTreeNodeFromDrive(DriveInfo drive)
        {
            var res = newTreeNodeFromDir(drive.Name, ImageIndex.Drive);
            var name = drive.Name.TrimEnd('\\', '/');  // 去除 分隔符后缀
            var text = $"{drive.VolumeLabel} ({name})"; // 模仿 Windows Explorer 行为
                                                        //var text = key;
            res.Text = text;
            return res;
        }
        protected static HashSet<string> archivesExt = new HashSet<string>
            { ".zip", ".rar", ".7z", ".tar", ".xz"}
        ;
        protected static int GetImageIndex(string filepath) => 
            archivesExt.Contains(Path.GetExtension(filepath).ToLower()) ?
                Ord(ImageIndex.Archive) :
                Ord(ImageIndex.File)
            ;
        protected static string GetFullPath(TreeNode     node) => node.Tag.ToString();
        protected static string GetFullPath(ListViewItem node) => node.Tag.ToString();
        protected static void   SetFullPath(TreeNode     node, string fullPath) => node.Tag = fullPath;
        protected static void   SetFullPath(ListViewItem node, string fullPath) => node.Tag = fullPath;
        
        /// <summary>
        /// 虚拟子节点名称，表示parent未初始化（未将entry存为nodes),
        /// 因为Tag存的是abspath，所以不可能有其他项的 Tag 为 "Dummy"
        /// </summary>
        protected static readonly string SubDirectoryDummyTag = "Dummy";
        #endregion // utils

        /// <summary>
        /// list drives
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
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
                            //SetFullPath(driveNode, drive.RootDirectory.FullName); // 保存路径信息到节点的 Tag 属性
                            //driveNode.Nodes.Add(SubDirectoryDummyTag); // 添加一个虚拟子节点，表示有子文件夹
                            treeViewDir.Nodes.Add(driveNode);
                        }
                        break;
                    case DriveType.Network:
                        {
                            // TODO
                        }
                        break;
                }
            }
        }

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
                // 递归遍历子文件夹
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
        #endregion  // treeView

        #region listView
        private void listViewItemBindedRender()
        {
            ListViewItem selectedListItem = listViewItem.SelectedItems[0];
            var fullPath = GetFullPath(selectedListItem);

            listViewItemRender_Uncached(fullPath);

            // 同步treeView
            var curTreeNode = treeViewDir.SelectedNode;
            //treeViewDirRender(newTreeNodeFromDir(selectedListItem.Tag.ToString(), curTreeNode));
            //curTreeNode.Collapse();
            var lastPart = selectedListItem.Text;
                
            //var dir = Path.Combine(GetFullPath(curTreeNode), lastPart);
            init_treeViewDir_if_needed(curTreeNode);
            var ls = curTreeNode.Nodes.Find(lastPart, true);
            if (ls.Count() == 0) return;
            curTreeNode = ls[0];

            curTreeNode.ExpandToRoot();
            UpdatePathInput(fullPath);
            //PopulateTreeView(dir, curTreeNode);
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
                string[] files = Directory.GetFiles(fullPath);
                foreach (var directory in directories)
                {
                    var item = newListViewItemFromDir(directory);
                    listViewItem.Items.Add(item);
                }
                foreach (string file in files)
                {
                    int image_index = GetImageIndex(file);
                    var item = new ListViewItem(Path.GetFileName(file), image_index);
                    SetFullPath(item, file);

                    //item.SubItems.Add("文件");
                    listViewItem.Items.Add(item);
                }
            }
        }
        #endregion // listView

        protected void UpdatePathInput(string fullPath) =>
            textBoxPath.Text = fullPath;

        // unimpl yet
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

        #region refreshBtn
        protected void InitRefreshBtnToolTip()
        {
            var refreshTip = new ToolTip()
            {
                AutoPopDelay = 5000,
                InitialDelay = 700,
                ReshowDelay = 500,
                ShowAlways = true,
            };
            refreshTip.SetToolTip(btnRefresh, "刷新");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var curTreeNode = treeViewDir.SelectedNode;
            curTreeNode.Nodes.Clear();
            curTreeNode.Nodes.Add(SubDirectoryDummyTag);
            treeViewDirBindedRender(curTreeNode);

        }
        #endregion // refreshBtn
    }
}
