﻿using System;
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
    public enum ImageIndex
    {
        Drive,
        Dir,
        Archive,
        File,
    }
    public partial class MainForm : Form
    {
        public int Ord(ImageIndex index) => (int)index;
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

        protected ListViewItem newListViewItemFromDir(string directory)
        {
            var name = new DirectoryInfo(directory).Name;
            var list = new ListViewItem(name);
            return new ListViewItem(name)
            {
                Tag = directory,
                ImageIndex = Ord(ImageIndex.Dir)
            };
        }
        protected TreeNode newTreeNodeFromDir(string directory, TreeViewEventArgs e)
        {
            var directoryInfo = newListViewItemFromDir(directory);
            var subNode = e.Node.Nodes.Add(directoryInfo.Name, directoryInfo.Name, directoryInfo.ImageIndex);
            //TreeNode subNode = e.Node.Nodes.Add(directoryInfo.Name, directoryInfo.Name, Ord(ImageIndex.Dir);
            //subNode.Tag = directoryInfo.FullName; // 保存路径信息到节点的 Tag 属性
            subNode.Tag = directoryInfo.Tag;
            subNode.Text = directoryInfo.Text;
            return subNode;
        }

        protected HashSet<string> archivesExt = new HashSet<string>
            { ".zip", ".rar", ".7z", ".tar", ".xz"}
        ;
        protected int GetImageIndex(string filepath) => 
            archivesExt.Contains(Path.GetExtension(filepath).ToLower()) ?
                Ord(ImageIndex.Archive) :
                Ord(ImageIndex.File)
            ;

        private void treeViewDir_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
            string folderpath = e.Node.Tag.ToString();
            listViewItem.Items.Clear();

            var succ = TryGetDirectories(out string[] subDirectories, folderpath);
            if (!succ) return;
            foreach (var subDirectory in subDirectories)
            {
                var item = newListViewItemFromDir(subDirectory);
                listViewItem.Items.Add(item);
            }
            string[] files = Directory.GetFiles(folderpath);
            foreach (string file in files)
            {
                var image_index = GetImageIndex(file);

                var item = new ListViewItem(Path.GetFileName(file), image_index)
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
                    var treeNode = new TreeNode(directoryInfo.Name, Ord(ImageIndex.Dir), 0);
                    
                    if (directoryInfo.Exists)
                    {
                        parentNode.Nodes.Add(treeNode);
                    }
                    PopulateTreeView(subFolder, treeNode);
                }
            }


        }
        protected static readonly string SubDirectoryDummyTag = "Dummy";

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
                            var key = drive.Name;
                            var name = key.TrimEnd('/', '\\');  // 去除 分隔符后缀
                            var text = $"{drive.VolumeLabel} ({name})"; // 模仿 Windows Explorer 行为
                                                                        //var text = key;
                            TreeNode driveNode = treeViewDir.Nodes.Add(key, text, Ord(ImageIndex.Drive));
                            driveNode.Tag = drive.RootDirectory.FullName; // 保存路径信息到节点的 Tag 属性
                            driveNode.Nodes.Add(SubDirectoryDummyTag); // 添加一个虚拟子节点，表示有子文件夹
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


        private void treeViewDir_AfterExpand(object sender, TreeViewEventArgs e)
        {
            
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == SubDirectoryDummyTag)
            {
                e.Node.Nodes.Clear(); // 移除虚拟子节点

                var nodePath = e.Node.Tag.ToString();

                var succ = TryGetDirectories(out string[] subDirectories, nodePath);
                if (!succ) return;
                foreach (var subDirectory in subDirectories)
                {
                    var subNode = newTreeNodeFromDir(subDirectory, e);
                    subNode.Nodes.Add(SubDirectoryDummyTag); // 添加一个虚拟子节点，表示有子文件夹
                }
                //ListDirsToNode(nodePath, ref e.Node.Nodes);
                
            }
        }

        private void listViewItem_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem selectedListItem = listViewItem.SelectedItems[0];
            listViewItemRender(selectedListItem);
        }
        protected void listViewItemRender(ListViewItem selectedListItem)
        {
            var nodeName = selectedListItem.Tag.ToString(); // 获取选中节点Fullpath
            if (File.Exists(nodeName))  // is file
            {
                Process.Start(nodeName);
            }
            else if (Directory.Exists(nodeName))
            {
                listViewItem.Clear();
                var succ = TryGetDirectories(out string[] directories, nodeName);
                if (!succ) return;
                string[] files = Directory.GetFiles(nodeName);
                foreach (var directory in directories)
                {
                    var item = newListViewItemFromDir(directory);
                    listViewItem.Items.Add(item);
                }
                foreach (string file in files)
                {
                    int image_index = GetImageIndex(file);
                    var item = new ListViewItem(Path.GetFileName(file), image_index)
                    {
                        Tag = file
                    };

                    //item.SubItems.Add("文件");
                    listViewItem.Items.Add(item);
                }
            }
        }

        private void textBoxPath_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    MessageBox.Show("asd");
            //    e.Handled = true;
            //}
        }
    }
}
