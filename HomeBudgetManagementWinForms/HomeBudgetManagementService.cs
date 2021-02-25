using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeBudgetManagementWinForms
{
    //Extension methods
    static class HomeBudgetManagementService
    {
       public static double Sum(this DataGridView dataGridView)
       {
            double total = 0;
            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                total += (double)item.Cells["Amount"].Value;
            }

            return total;
        }
    }
}
