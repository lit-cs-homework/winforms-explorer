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
using System.Collections.Specialized;
using System.Diagnostics;



namespace file_manage
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected void Error(string msg) => MessageBox.Show(msg);
        protected void Error(string msg, string title) => MessageBox.Show(msg, title);

        protected bool TryGetDirectories(out string[] res, string path)
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
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
            string folderpath = e.Node.FullPath;
            listViewItem.Items.Clear();

            var succ = TryGetDirectories(out string[] subDirectories, folderpath);
            if (!succ) return;
            string[] files = Directory.GetFiles(folderpath);
            foreach (string subDirectory in subDirectories)
            {
                var name = new DirectoryInfo(subDirectory);
                var item = new ListViewItem(name.Name, 0)
                {
                    //SubItems.Add("文件夹");
                    Tag = subDirectory
                };
                listViewItem.Items.Add(item);
            }
            foreach (string file in files)
            {
                int image_index = 1;
                string fileExtension = Path.GetExtension(file).ToLower();
                if (fileExtension == ".txt")
                {
                    image_index = 1;
                }
                else if(fileExtension ==".zip" || fileExtension == ".rar")
                {
                    image_index = 2;
                }

                ListViewItem item = new ListViewItem(Path.GetFileName(file), image_index)
                {
                    Tag = file
                };

                //item.SubItems.Add("文件");
                listViewItem.Items.Add(item);
            }
        }

        private void PopulateTreeView(string folderPath, TreeNode parentNode)
        {
            // 获取当前文件夹的所有子文件夹
            var succ = TryGetDirectories(out string[] subFolders, folderPath);
            if (!succ) return;
            if (subFolders == null || subFolders.Length == 0 ) return;
            else
            {
                // 处理当前文件夹              
                // 递归遍历子文件夹
                foreach (var subFolder in subFolders)
                {
                    var directoryInfo = new DirectoryInfo(subFolder);
                    var treeNode = new TreeNode(directoryInfo.Name, 0, 0);
                    if (directoryInfo.Exists)
                    {
                        parentNode.Nodes.Add(treeNode);
                    }
                    PopulateTreeView(subFolder, treeNode);
                }
            }


        }
        protected static readonly string SubDirectoryDummyTag = "Dummy";

        private void Form1_Load(object sender, EventArgs e)
        {

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                if (drive.DriveType == DriveType.Fixed) // 只添加固定磁盘驱动器
                {
                    var key = drive.Name;
                    //var name = key.TrimEnd('/', '\\');  // 去除 分隔符后缀
                    //var text = $"{drive.VolumeLabel} ({name})"; // 模仿 Windows Explorer 行为
                    var text = key;
                    TreeNode driveNode = treeViewDir.Nodes.Add(key, text, 2);
                    driveNode.Tag = drive.RootDirectory.FullName; // 保存路径信息到节点的 Tag 属性
                    driveNode.Nodes.Add(SubDirectoryDummyTag); // 添加一个虚拟子节点，表示有子文件夹
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Console.WriteLine(e.ToString());
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == SubDirectoryDummyTag)
            {
                e.Node.Nodes.Clear(); // 移除虚拟子节点

                var nodePath = e.Node.Tag.ToString();

                var succ = TryGetDirectories(out string[] subDirectories, nodePath);
                if (!succ) return;
                foreach (var subDirectory in subDirectories)
                {
                    var directoryInfo = new DirectoryInfo(subDirectory);
                    TreeNode subNode = e.Node.Nodes.Add(directoryInfo.Name, directoryInfo.Name, 0);
                    subNode.Tag = directoryInfo.FullName; // 保存路径信息到节点的 Tag 属性
                    subNode.Nodes.Add(SubDirectoryDummyTag); // 添加一个虚拟子节点，表示有子文件夹
                }
                //ListDirsToNode(nodePath, ref e.Node.Nodes);
                
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem selectedListItem = listViewItem.SelectedItems[0];
            string nodeName = selectedListItem.Tag.ToString(); // 获取选中节点的文本
            string itemName = selectedListItem.Text;
            if (File.Exists(nodeName))
            {
                Process.Start(nodeName);
            }
            else if (Directory.Exists(nodeName))
            {
                listViewItem.Clear();
                var succ = TryGetDirectories(out string[] directories, nodeName);
                if (!succ) return;
                string[] files = Directory.GetFiles(nodeName);
                foreach (string directory in directories)
                {
                    DirectoryInfo foo = new DirectoryInfo(directory);
                    ListViewItem item = new ListViewItem(foo.Name, 0);
                    item.Tag = foo.FullName;
                    listViewItem.Items.Add(item);
                }
                foreach (string file in files)
                {
                    int image_index = 1;
                    string fileExtension = Path.GetExtension(file).ToLower();
                    if (fileExtension == ".txt")
                    {
                        image_index = 1;
                    }
                    else if (fileExtension == ".zip" || fileExtension == ".rar")
                    {
                        image_index = 2;
                    }

                    ListViewItem item = new ListViewItem(Path.GetFileName(file), image_index);
                    item.Tag = file;

                    //item.SubItems.Add("文件");
                    listViewItem.Items.Add(item);
                }
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {


        }
    }
}
