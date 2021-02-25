using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeBudgetManagementWinForms.Services;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagementWinForms
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void expensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormExpenses form = new FormExpenses();
            form.ShowDialog();
        }

        private void incomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIncome formIncome = new FormIncome();
            formIncome.ShowDialog();
        }

        private async void accountInfoTimer_Tick(object sender, EventArgs e)
        {
            AccountService service = new AccountService();

            Account account = await service.GetAccountAsync();
            toolStripBalance.Text = "Balance: " + account.Balance.ToString();
        }
    }
}
