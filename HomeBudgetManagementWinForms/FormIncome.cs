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
                lblTotalAmount.Text = dgv.Sum().ToString();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

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
                    range.Add(new Income()
                    {
                        Id = (int)item.Cells["Id"].Value,
                        Amount = (double)item.Cells["Amount"].Value,
                        Date = (DateTime)item.Cells["Date"].Value,
                        Description = (string)item.Cells["Description"].Value
                    });
                }

                result = await IncomeService.DeleteRangeIncome(range);
            }
            else if(dgvList.SelectedRows.Count == 1)
            {
                int id = (int)dgvList.SelectedRows[0].Cells["Id"].Value;
                result = await IncomeService.DeleteIncome(id);
            }

            if(result) {
                MessageBox.Show("Deleted");
                GetIncomes();
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
            IncomeService IncomeService = new IncomeService();
            bool result = false;
            Income Income = new Income()
            {
                Id = Convert.ToInt32(txtId.Text),
                Amount = Convert.ToDouble(txtAmount.Text),
                Description = txtDescription.Text,
                Date = dateTimePicker1.Value
            };

            if(Income.Id > 0)
            {
                if (await IncomeService.UpdateIncome(Income))
                {
                    MessageBox.Show("Updated");
                    result = true;
                }
            } else
            {
                Income = await IncomeService.CreateIncome(Income);
                if (Income.Id > 0)
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

        private async  void btnDownloadFile_Click(object sender, EventArgs e)
        {
            IncomeService incomeService = new IncomeService();
            Income ex = await incomeService.GetById(Convert.ToInt32(txtId.Text));
            incomeService = new IncomeService();
            byte[] file = await incomeService.DownloadFile(ex.Id);

            File.WriteAllBytes(Application.StartupPath + "\\" + ex.FileExtension, file);
            System.Diagnostics.Process.Start(Application.StartupPath + "\\" + ex.FileExtension);
        }

        private void dgvList_DataSourceChanged_1(object sender, EventArgs e)
        {
            dgvList.Columns.Remove("File");
            dgvList.Columns.Remove("FileExtension");

        }
    }

}
