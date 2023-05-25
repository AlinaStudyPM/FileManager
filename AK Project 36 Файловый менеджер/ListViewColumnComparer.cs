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
        public string[] Months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
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
                if (ColumnIndex == 4)
                {

                    string[] date1 = X.SubItems[ColumnIndex].Text.Split(' ');
                    string[] date2 = Y.SubItems[ColumnIndex].Text.Split(' ');
                    if (date1[2] == date2[2])
                    {
                        if (date1[0] == date2[0])
                        {
                            int day1 = Convert.ToInt16(date1[1].Substring(0, date1[1].Length - 1));
                            int day2 = Convert.ToInt16(date2[1].Substring(0, date2[1].Length - 1));
                            return day1.CompareTo(day2);
                        }
                        else
                        {
                            return Array.IndexOf(Months, date1[0]).CompareTo(Array.IndexOf(Months, date2[0]));
                        }
                    }
                    else
                    {
                        return String.Compare(date2[2], date1[2]);
                    }
                }



                return String.Compare(X.SubItems[ColumnIndex].Text, Y.SubItems[ColumnIndex].Text);
            }
            catch
            {
               //MessageBox.Show(x.ToString());
               //MessageBox.Show(y.ToString());
                return 0;
            }
        }
    }
}
