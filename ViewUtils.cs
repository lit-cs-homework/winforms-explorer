
using System.Windows.Forms;

namespace file_manage
{

    public static class ViewUtils
    {
        /// <summary>
        /// Expand on each parent until root
        /// </summary>
        /// <param name="node">child node to expand from (included)</param>
        public static void ExpandToRoot(this TreeNode node)
        {
            for (var n = node; n != null; n = n.Parent)
            {
                n.Expand();
            }
        }
    }
}
