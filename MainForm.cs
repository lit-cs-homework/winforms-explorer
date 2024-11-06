using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace file_manage
{
    using static Utils;
    public partial class MainForm : Form
    {
        #region i18n
        protected enum ZhEn { Zh, En }
        protected I18N2<ZhEn> i18n;
        protected ZhEn Lang
        {
            get => i18n.Lang;
            set
            {
                i18n.Lang = value;
                UpdateToolTips();
            }
        }

        /// <summary>
        /// like `_` of gettext,
        /// but only for 2 languages.
        /// </summary>
        /// <param name="zh">Chinese text</param>
        /// <param name="en">English text</param>
        /// <returns></returns>
        protected string _2(string zh, string en) => i18n.T(zh, en);

        private void InitLangSelector(int selectedIdx)
        {
            comboBoxLang.Items.AddRange(LangText);
            comboBoxLang.SelectedIndex = selectedIdx;
        }
        protected void InitLocaleFromSystemConfig()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            var e = ZhEn.En;
            if (culture.Name == "zh" || culture.Name == "zh-Hans") e = ZhEn.Zh;
            i18n = new I18N2<ZhEn>(e);
        }
        private static readonly string[] LangText = { "zh", "en" };
        private void comboBoxLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbox = (ComboBox)sender;
            cbox.SelectedItem = LangText[cbox.SelectedIndex];
            Lang = (ZhEn)cbox.SelectedIndex;
        }
        #endregion i18n

        #region utils
        private void ListDrives() =>
            Utils.ListDrives((drive) =>
               {
                   var driveNode = newTreeNodeFromDrive(drive);
                   treeViewDir.Nodes.Add(driveNode);
               }
            );

        #endregion utils
        
        #region init
        public MainForm()
        {
            InitializeComponent();

            InitLocaleFromSystemConfig();  // must be before InitToolTip and after InitalizeComponent
            InitToolTips();
            InitLangSelector((int)i18n.Lang); // must be after InitToolTips

            InitAutoUpdateWidth();

        }

        protected void InitAutoUpdateWidth()
        {
            flowLayoutPanelInput.KeepWidthOfParent(0.85);
            textBoxPath.KeepWidthOfParent(0.85);
        }

        private void MainForm_Load(object sender, EventArgs e) => ListDrives();
        #endregion init

        #region treeView
        private void treeViewDirBindedRender(TreeNode node)
        {
            treeViewDirRender(node);
            node.ExpandToRoot();
            // bind
            var fullPath = GetFullPath(node);
            listViewItemRender(fullPath);
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
            => treeViewDirRender(e.Node);

        private void treeViewDir_AfterCollapse(object _sender, TreeViewEventArgs e)
        {
            var node = e.Node.Parent;
            if (node == null) return;
            var path = GetFullPath(node);
            listViewItemRender(path);

        }
        private void treeViewDirRender(TreeNode node)
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

            listViewItemRender(fullPath);

            // 同步treeView
            var curTreeNode = treeViewDir.SelectedNode;
            var lastPart = selectedListItem.Text;
                
            treeViewDirRender(curTreeNode);
            var ls = curTreeNode.Nodes.Find(lastPart, true);
            if (ls.Count() == 0) return;
            curTreeNode = ls[0];

            curTreeNode.ExpandToRoot();
            UpdatePathInput(fullPath);
        }
        private void listViewItem_DoubleClick(object _sender, EventArgs e)
            => listViewItemBindedRender();

        protected void listViewItemRender(string fullPath)
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
        {
            var txt = textBoxPath.Text;
            if (txt == string.Empty)
            {
                MessageBox.Show(_2("路径为空", "nothing to copy"));
                return;
                // or error is thrown:
                /*System.ArgumentNullException: 'Value cannot be null.
                   Parameter name: text'*/
            }
            Clipboard.SetText(txt);
        }
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

        protected ToolTip toolTipLong, toolTipShort;
        protected void InitToolTips()
        {
            toolTipLong = newToopTip(700);
            toolTipShort = newToopTip(100);
            UpdateToolTips();
        }

        protected void UpdateToolTips()
        {
            // must remove the old to eval `_2` again.
            toolTipLong.RemoveAll();
            toolTipShort.RemoveAll();

            toolTipLong.SetToolTip(btnRefresh, _2("刷新", "Refresh"));
            toolTipLong.SetToolTip(btnNavParentDir, _2("上一级目录", "Go to parent dir"));
            toolTipShort.SetToolTip(textBoxPath, _2("点击复制", "Click to copy to clipboard"));
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
