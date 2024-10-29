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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
            string folderpath = e.Node.FullPath.ToString();
            listViewItem.Items.Clear();

            string[] subDirectories;
            try
            {
                subDirectories = Directory.GetDirectories(folderpath);
            } catch (System.UnauthorizedAccessException exc)
            {
                Error(exc.Message, "权限不足");
                return;
            }
            string[] files = Directory.GetFiles(folderpath);
            foreach (string subDirectory in subDirectories)
            {
                DirectoryInfo name = new DirectoryInfo(subDirectory);
                ListViewItem item = new ListViewItem(name.Name, 0);
                //item.SubItems.Add("文件夹");
                item.Tag = subDirectory;
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

                ListViewItem item = new ListViewItem(Path.GetFileName(file), image_index);
                item.Tag = file;
                
                //item.SubItems.Add("文件");
                listViewItem.Items.Add(item);
            }
        }

        private void PopulateTreeView(string folderPath, TreeNode parentNode)
        {
            try
            {
                // 获取当前文件夹的所有子文件夹
                string[] subFolders = Directory.GetDirectories(folderPath);
                if (subFolders == null || subFolders.Length == 0 ) { return; }
                else
                {
                    // 处理当前文件夹              
                    // 递归遍历子文件夹
                    foreach (string subFolder in subFolders)
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(subFolder);
                        TreeNode treeNode = new TreeNode(directoryInfo.Name, 0, 0);
                        if (directoryInfo.Exists)
                        {
                            parentNode.Nodes.Add(treeNode);
                        }
                        PopulateTreeView(subFolder, treeNode);
                    }
                }

            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("访问被拒绝: " + folderPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生错误: " + ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.DriveType == DriveType.Fixed) // 只添加固定磁盘驱动器
                {
                    TreeNode driveNode = treeViewDir.Nodes.Add(drive.Name, drive.Name, 2);
                    driveNode.Tag = drive.RootDirectory.FullName; // 保存路径信息到节点的 Tag 属性
                    driveNode.Nodes.Add("Dummy"); // 添加一个虚拟子节点，表示有子文件夹
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Console.WriteLine(e.ToString());
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "Dummy")
            {
                e.Node.Nodes.Clear(); // 移除虚拟子节点

                string nodePath = e.Node.Tag.ToString();

                try
                {
                    string[] subDirectories = Directory.GetDirectories(nodePath);
                    foreach (string subDirectory in subDirectories)
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(subDirectory);
                        TreeNode subNode = e.Node.Nodes.Add(directoryInfo.Name, directoryInfo.Name, 0);
                        subNode.Tag = directoryInfo.FullName; // 保存路径信息到节点的 Tag 属性
                        subNode.Nodes.Add("Dummy"); // 添加一个虚拟子节点，表示有子文件夹
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // 处理访问被拒绝的情况
                    MessageBox.Show("Access to a folder was denied.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
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
                string[] directories = Directory.GetDirectories(nodeName);
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
