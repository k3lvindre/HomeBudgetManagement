using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeBudgetManagement.Models;
using HomeBudgetManagementWinForms.Services;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HomeBudgetManagementWinForms
{
    public partial class FormExpenses : Form
    {
        //public delegate  void ReCalculateBalance(object sender, AccountEventArgs e);
        //replace this delegate handler with event so user don't have direct access to removing,adding and invoking the method/delegate handler.
        //public ReCalculateBalance handler;
        //public event ReCalculateBalance newHandler;
        //We can also use public event EventHandler<AccountEventArgs> newHandler; so we no longer need to create the delegate -public delegate void ReCalculateBalance(object sender, AccountEventArgs e);
        public event EventHandler<AccountEventArgs> idealWayOfEventHandler;

        public FormExpenses()
        {
            InitializeComponent();
            dgvList.DataSourceChanged +=  DgvList_DataSourceChanged;
        }

        private void DgvList_DataSourceChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if(dgv != null)
            {
                lblTotalAmount.Text = dgv.Sum().ToString();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Expenses_Load(object sender, EventArgs e)
        {
            GetExpenses();
        }

        private  void BtnAdd_ClickAsync(object sender, EventArgs e)
        {
            txtId.Text = "0";
            txtAmount.Text = "0.00";
            txtDescription.Text = "";
            dateTimePicker1.ResetText();

            btnCancel.Visible = true;
            btnSave.Enabled = true;
        }

        private async void GetExpenses()
        {
            ExpenseService expenseService = new ExpenseService();
            dgvList.DataSource = await expenseService.GetAllExpensesAsync();

            AccountService service = new AccountService();
            Account account = await service.GetAccountAsync();
            //replace this delegate invokecation with event
            //handler();
            //newHandler?.Invoke(this, new AccountEventArgs(account.Balance));
            //Here we now use eventhandler
            idealWayOfEventHandler?.Invoke(this, new AccountEventArgs(account.Balance));
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            List<Expense> range = new List<Expense>();
            ExpenseService expenseService = new ExpenseService();
            bool result = false;

            if (dgvList.SelectedRows.Count > 1)
            {
                foreach (DataGridViewRow item in dgvList.SelectedRows)
                {
                    range.Add(new Expense()
                    {
                        Id = (int)item.Cells["Id"].Value,
                        Amount = (double)item.Cells["Amount"].Value,
                        Date = (DateTime)item.Cells["Date"].Value,
                        Description = (string)item.Cells["Description"].Value
                    });
                }

                result = await expenseService.DeleteRangeExpense(range);
            }
            else if(dgvList.SelectedRows.Count == 1)
            {
                int id = (int)dgvList.SelectedRows[0].Cells["Id"].Value;
                result = await expenseService.DeleteExpense(id);
            }

            if(result) {
                MessageBox.Show("Deleted");
                GetExpenses();
            }
        }

        private void dgvList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            btnSave.Enabled = dgvList.SelectedRows.Count == 1;
            if(btnSave.Enabled)
            {
                DataGridViewRow row = dgvList.SelectedRows[0];
                txtId.Text = row.Cells["Id"].Value.ToString();
                txtAmount.Text = row.Cells["Amount"].Value.ToString();
                txtDescription.Text = row.Cells["Description"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["Date"].Value);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            ExpenseService expenseService = new ExpenseService();
            bool result = false;
            Expense expense = new Expense()
            {
                Id = Convert.ToInt32(txtId.Text),
                Amount = Convert.ToDouble(txtAmount.Text),
                Description = txtDescription.Text,
                Date = dateTimePicker1.Value
            };

            if(expense.Id > 0)
            {
                if (await expenseService.UpdateExpense(expense))
                {
                    MessageBox.Show("Updated");
                    result = true;
                }
            } else
            {
                expense = await expenseService.CreateExpense(expense);
                if (expense.Id > 0)
                {
                    MessageBox.Show("Created");
                    result = true;
                }
            }

            if(result) GetExpenses();
            btnCancel.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GetExpenses();
            btnCancel.Visible = false;
            btnSave.Enabled = false;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private async void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                lblFIleName.Text = openFileDialog.FileName;
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                
                using(FileStream fs = fileInfo.OpenRead())
                {
                    BinaryReader binaryReader = new BinaryReader(fs);
                    byte[] file = binaryReader.ReadBytes((int)fs.Length);

                    ExpenseService expenseService = new ExpenseService();
                    Expense expense = new Expense()
                    {
                        Id = Convert.ToInt32(txtId.Text),
                        Amount = Convert.ToDouble(txtAmount.Text),
                        Description = txtDescription.Text,
                        Date = dateTimePicker1.Value,
                        File = file,
                        FileExtension = fileInfo.Name
                    };

                    if (expense.Id > 0)
                    {
                        if (await expenseService.UpdateExpense(expense))
                        {
                            MessageBox.Show("Updated");
                        }
                    }
                }
            }
        }

        private async void btnDownloadFile_Click(object sender, EventArgs e)
        {
            ExpenseService expenseService = new ExpenseService();
            Expense ex = await expenseService.GetById(Convert.ToInt32(txtId.Text));
            expenseService = new ExpenseService();
            byte[] file = await expenseService.DownloadFile(ex.Id);

            File.WriteAllBytes(Application.StartupPath+"\\"+ex.FileExtension, file);
            System.Diagnostics.Process.Start(Application.StartupPath + "\\" + ex.FileExtension);
        }

        private void dgvList_DataSourceChanged_1(object sender, EventArgs e)
        {
            dgvList.Columns.Remove("File");
            dgvList.Columns.Remove("FileExtension");

        }
    }

    public class AccountEventArgs : EventArgs
    {
        public readonly double Balance;
        public AccountEventArgs(double balance)
        {
            Balance = balance;
        }
    }

}
