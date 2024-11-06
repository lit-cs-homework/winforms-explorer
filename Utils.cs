using System;
using System.IO;
using System.Windows.Forms;

namespace file_manage
{
    internal static class Utils
    {

        public delegate void HandleDriveItem(DriveInfo drive);
        /// <summary>
        /// list drives
        /// </summary>
        public static void ListDrives(HandleDriveItem cb)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                switch (drive.DriveType)
                {
                    // 磁盘驱动器
                    case DriveType.Fixed:
                    case DriveType.Removable:
                        cb(drive);
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

        public static void Error(string msg) => MessageBox.Show(msg);
        public static void Error(string msg, string title) => MessageBox.Show(msg, title);

        public static bool TryGetDirectories(out string[] res, string path)
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

        public static ListViewItem newListViewItemFromDir(string directory, ImageIndex imIdx=ImageIndex.Dir)
        {
            var name = new DirectoryInfo(directory).Name;
            var res = new ListViewItem(name, Ord(imIdx));
            res.Name = name;
            SetFullPath(res, directory);
            return res;
        }
        public static TreeNode newTreeNodeFromDir(string directory, ImageIndex imIdx=ImageIndex.Dir)
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
        public static TreeNode newTreeNodeFromDrive(DriveInfo drive)
        {
            var res = newTreeNodeFromDir(drive.Name, ImageIndex.Drive);
            var name = drive.Name.TrimEnd('\\', '/');  // 去除 分隔符后缀
            var text = $"{drive.VolumeLabel} ({name})"; // 模仿 Windows Explorer 行为
            res.Text = text;
            return res;
        }
        public static string GetFullPath(TreeNode     node) => node.Tag.ToString();
        public static string GetFullPath(ListViewItem node) => node.Tag.ToString();
        public static void   SetFullPath(TreeNode     node, string fullPath) => node.Tag = fullPath;
        public static void   SetFullPath(ListViewItem node, string fullPath) => node.Tag = fullPath;
        
        /// <summary>
        /// 虚拟子节点名称，表示parent未初始化（未将entry存为nodes),
        /// 因为Tag存的是abspath，所以不可能有其他项的 Tag 为 "Dummy"
        /// </summary>
        public static readonly string SubDirectoryDummyTag = "Dummy";

    }
}
