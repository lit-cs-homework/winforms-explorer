using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace file_manage
{

    public static class ViewUtils
    {
        public static void ExpandToRoot(this TreeNode node)
        {
            for (var n = node; n != null; n = n.Parent)
            {
                n.Expand();
            }
        }
    }
}
