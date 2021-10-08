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
    public partial class FormIncome : Form
    {
        public EventHandler<AccountEventArgs> AccountUpdated;

        public FormIncome()
        {
            InitializeComponent();
            dgvList.DataSourceChanged +=  DgvList_DataSourceChanged;
        }

        private void DgvList_DataSourceChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if(dgv != null)
            {
                dgv.Columns.Remove("File");
                dgv.Columns.Remove("FileExtension");
                lblTotalAmount.Text = dgv.Sum().ToString();
            }
        }

        private void Incomes_Load(object sender, EventArgs e)
        {
            GetIncomes();
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

        private async void GetIncomes()
        {
            IncomeService IncomeService = new IncomeService();
            dgvList.DataSource = await IncomeService.GetAllIncomesAsync();

            AccountService accountService = new AccountService();
            //Account account = await accountService.GetAccountAsync();
            Account account = await accountService.GetAccountV2Async();
            AccountUpdated.Invoke(this, new AccountEventArgs(account.Balance));
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            List<Income> range = new List<Income>();
            IncomeService IncomeService = new IncomeService();
            bool result = false;

            if (dgvList.SelectedRows.Count > 1)
            {
                foreach (DataGridViewRow item in dgvList.SelectedRows)
                {
                    Income income = ConvertDatagridviewrow(item);
                    range.Add(income);
                }
            }
            else if(dgvList.SelectedRows.Count == 1)
            {
                Income income = ConvertDatagridviewrow(dgvList.SelectedRows[0]);
                range.Add(income);
            }

            result = await IncomeService.DeleteRangeIncome(range);

            if (result) {
                MessageBox.Show("Deleted");
                GetIncomes();
            }
        }

        private void dgvList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            btnSave.Enabled = dgvList.SelectedRows.Count == 1;
            if (btnSave.Enabled)
            {
                DataGridViewRow row = dgvList.SelectedRows[0];
                Income income = ConvertDatagridviewrow(row);
                txtId.Text = income.Id.ToString();
                txtAmount.Text = income.Amount.ToString();
                txtDescription.Text = income.Description.ToString();
                dateTimePicker1.Value = income.Date;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            IncomeService IncomeService = new IncomeService();
            bool result = false;
            Income income = new Income()
            {
                Id = Convert.ToInt32(txtId.Text),
                Amount = Convert.ToDouble(txtAmount.Text),
                Description = txtDescription.Text,
                Date = dateTimePicker1.Value
            };

            if(income.Id > 0)
            {
                if (await IncomeService.UpdateIncome(income))
                {
                    MessageBox.Show("Updated");
                    result = true;
                }
            } else
            {
                income = await IncomeService.CreateIncome(income);
                if (income.Id > 0)
                {
                    MessageBox.Show("Created");
                    result = true;
                }
            }

            if(result) GetIncomes();
            btnCancel.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GetIncomes();
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

                    IncomeService IncomeService = new IncomeService();
                    Income Income = new Income()
                    {
                        Id = Convert.ToInt32(txtId.Text),
                        Amount = Convert.ToDouble(txtAmount.Text),
                        Description = txtDescription.Text,
                        Date = dateTimePicker1.Value,
                        File = file,
                        FileExtension = fileInfo.Name
                    };

                    if (Income.Id > 0)
                    {
                        if (await IncomeService.UpdateIncome(Income))
                        {
                            MessageBox.Show("Updated");
                        }
                    }
                }
            }
        }

        private async void btnDownloadFile_Click(object sender, EventArgs e)
        {
            IncomeService incomeService = new IncomeService();
            Income ex = await incomeService.GetById(Convert.ToInt32(txtId.Text));

            //reinstantiate because httpt client is being disposed which needs to fix
            incomeService = new IncomeService();

            byte[] file = await incomeService.DownloadFile(ex.Id);
            if (file == null)
            {
                MessageBox.Show("No File Found!");
            }
            else
            {
                File.WriteAllBytes(Application.StartupPath + "\\" + ex.FileExtension, file);
                System.Diagnostics.Process.Start(Application.StartupPath + "\\" + ex.FileExtension);
            }
        }
        private Income ConvertDatagridviewrow(DataGridViewRow dgvRow) 
        {
            return new Income()
            {
                Id = (int)dgvRow.Cells["Id"].Value,
                Amount = (double)dgvRow.Cells["Amount"].Value,
                Date = (DateTime)dgvRow.Cells["Date"].Value,
                Description = (string)dgvRow.Cells["Description"].Value
            };
        }

    }

}
