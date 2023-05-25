using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace AK_Project_36_Файловый_менеджер
{
    internal class ListViewColumnComparer : IComparer
    {
        public int ColumnIndex { get; set; }
        public ListViewColumnComparer(int columnIndex)
        {
            ColumnIndex = columnIndex;
        }
        public int Compare(object x, object y)
        {
            try
            {
                ListViewItem X = (ListViewItem)x;
                ListViewItem Y = (ListViewItem)y;
                if (ColumnIndex == 2)
                {
                    return (-1) * String.Compare(X.SubItems[ColumnIndex].Text, Y.SubItems[ColumnIndex].Text);
                }
                return String.Compare(X.SubItems[ColumnIndex].Text, Y.SubItems[ColumnIndex].Text);
            }
            catch
            {
                return 0;
            }
        }
    }
}
