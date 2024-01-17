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
            //Replace how delegate handler is called with event so user doesn't have direct access invoking the delegate handler like form.handler.invoke() and prevents from replacing all the registered methods 
            //form.handler +=  new FormExpenses.ReCalculateBalance(UpdateBalanceEvent);
            //form.newHandler += UpdateBalanceEvent;
            //Here we used the eventhandler instead of event
            form.idealWayOfEventHandler += UpdateBalanceEvent;
            form.ShowDialog();
        }

        private void incomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIncome formIncome = new FormIncome();
            //Used lambda expression for assigning event
            formIncome.AccountUpdated += (frmIncomeAsTheSender , eventArgs)=>{
                System.Diagnostics.Debug.WriteLine(frmIncomeAsTheSender.ToString());
                toolStripBalance.Text = "Balance: " + eventArgs.Balance.ToString();
            };
            formIncome.ShowDialog();
        }

        private  void UpdateBalanceEvent(object sender, AccountEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(sender.ToString());
            toolStripBalance.Text = "Balance: " + e.Balance.ToString();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
    }
}
