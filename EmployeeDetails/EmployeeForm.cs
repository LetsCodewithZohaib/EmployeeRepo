using EmployeeBLL;
using EmployeeDetails.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace EmployeeDetails
{
    public partial class EmployeeForm : Form
    {
        private IEmployeeManager employeeManager;
        private int CurrPageNumber;

        public EmployeeForm(IEmployeeManager employeeManager)
        {
            CurrPageNumber = 1;
            InitializeComponent();
            this.employeeManager = employeeManager;
            GridDatabind();
        }

        private void GridDatabind(string criteria = "")
        {
            try
            {
                var root = employeeManager.GetAll(criteria.Trim());
                List<Datum> employees = root.data;
                var paging = root.meta.pagination;
                LoadPagination(paging);
                dgEmployees.DataSource = null;
                dgEmployees.Rows.Clear();
                dgEmployees.Columns.Clear();
                dgEmployees.Refresh();
                dgEmployees.Update();
                cmbStatus.SelectedItem = "Active";
                if (employees.Count > 0)
                {
                    dgEmployees.Columns.Add("Id", "Id");
                    dgEmployees.Columns.Add("Name", "Name");
                    dgEmployees.Columns.Add("Email", "Email");
                    dgEmployees.Columns.Add("Gender", "Gender");
                    dgEmployees.Columns.Add("Status", "Status");
                    DataGridViewLinkColumn lnkEdit = new DataGridViewLinkColumn();
                    lnkEdit.UseColumnTextForLinkValue = true;
                    lnkEdit.LinkBehavior = LinkBehavior.SystemDefault;
                    lnkEdit.HeaderText = "";
                    lnkEdit.Name = "Edit";
                    lnkEdit.Text = "Edit";
                    lnkEdit.UseColumnTextForLinkValue = true;
                    dgEmployees.Columns.Add(lnkEdit);
                    DataGridViewLinkColumn lnkDelete = new DataGridViewLinkColumn();
                    dgEmployees.CellClick += DgEmployees_CellClick;
                    lnkDelete.UseColumnTextForLinkValue = true;
                    lnkDelete.LinkBehavior = LinkBehavior.SystemDefault;
                    lnkDelete.HeaderText = "";
                    lnkDelete.Name = "Delete";
                    lnkDelete.Text = "Delete";
                    lnkDelete.UseColumnTextForLinkValue = true;
                    dgEmployees.Columns.Add(lnkDelete);

                    foreach (var emp in employees)
                    {
                        string[] row = new string[] { emp.id.ToString(), emp.name, emp.email, emp.gender, emp.status };
                        dgEmployees.Rows.Add(row);
                    }
                    dgEmployees.Columns[0].Visible = false;
                    dgEmployees.Refresh();
                    dgEmployees.Update();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex.Message);
            }
        }

        private void LoadPagination(Pagination pagesList)
        {
            if (pagesList.pages > 0)
            {
                cmbPages.Items.Clear();
                for (int i = 1; i <= pagesList.pages; i++)
                {
                    cmbPages.Items.Add(i);
                }
            }
        }

        private void DgEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex > 4)
                {
                    var lnk = dgEmployees.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                    if (lnk.ToLower().Trim() == "edit")
                    {
                        var id = dgEmployees.Rows[e.RowIndex].Cells[0].Value.ToString();
                        lblid.Text = id;
                        txtName.Text = dgEmployees.Rows[e.RowIndex].Cells[1].Value.ToString();
                        txtEmail.Text = dgEmployees.Rows[e.RowIndex].Cells[2].Value.ToString();
                        var Gender = dgEmployees.Rows[e.RowIndex].Cells[3].Value.ToString();

                        if (Gender.ToLower().Trim() == "male")
                        {
                            rdMale.Checked = true;
                            rdFemale.Checked = false;
                        }
                        else if (Gender.ToLower().Trim() == "female")
                        {
                            rdMale.Checked = false;
                            rdFemale.Checked = true;
                        }

                        btnAddEdit.Text = "Update Employee";
                        cmbStatus.SelectedItem = dgEmployees.Rows[e.RowIndex].Cells[4].Value.ToString();

                    }
                    else if (lnk.ToLower().Trim() == "delete")
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Deleted", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            int id = Convert.ToInt32(dgEmployees.Rows[e.RowIndex].Cells[0].Value.ToString());
                            bool isdeleted = employeeManager.DeleteEmployee(id);
                            if (isdeleted)
                            {
                                MessageBox.Show("Employee deleted Successfully");
                            }
                        }
                        GridDatabind();
                        ClearAll();
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("Error :" + ex.Message);
            }
        }

        private void ClearAll()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            rdMale.Checked = true;
            rdFemale.Checked = false;
            cmbStatus.SelectedItem = "Active";
            btnAddEdit.Text = "Add Employee";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAddEdit_Click(object sender, EventArgs e)
        {
            //SAVE DATA
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Employee Name is Required!!");
            }
            else if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Employee Email is Required!!");
            }
            else
            {
                if (btnAddEdit.Text.ToLower().Trim() == "add employee")
                {
                    try
                    {
                        Datum datum = new Datum();
                        datum.name = txtName.Text.Trim();
                        datum.email = txtEmail.Text.Trim();
                        datum.gender = rdMale.Checked ? "Male" : "Female";
                        datum.status = cmbStatus.SelectedItem.ToString();
                        datum.created_at = DateTime.Now;
                        datum.updated_at = DateTime.Now;

                        bool returnval = employeeManager.AddEmployee(datum);
                        if (returnval)
                        {
                            MessageBox.Show("Employee Saved Successfully");
                        }
                        else
                        {
                            MessageBox.Show("Sorry!! Employee is not Saved, Or Employee already Exists");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error :" + ex.Message);
                    }
                }
                else
                {
                    Datum datum = new Datum();
                    datum.id = Convert.ToInt32(lblid.Text.Trim());
                    datum.name = txtName.Text.Trim();
                    datum.email = txtEmail.Text.Trim();
                    datum.gender = rdMale.Checked ? "Male" : "Female";
                    datum.status = cmbStatus.SelectedItem.ToString();
                    datum.updated_at = DateTime.Now;
                    try
                    {
                        bool returnval = employeeManager.UpdateEmployee(datum);
                        if (returnval)
                        {
                            MessageBox.Show("Employee Updated Successfully");
                        }
                        else
                        {
                            MessageBox.Show("Sorry!! Employee is not Updated");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error :" + ex.Message);
                    }
                }
                GridDatabind();
                ClearAll();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string st = Query();
            GridDatabind(st);
        }

        private string Query(int pageNum = 1)
        {
            string res = string.Empty;

            if (!string.IsNullOrEmpty(txtSrchName.Text))
            {
                res += "name=" + txtSrchName.Text.Trim();
            }

            if (!string.IsNullOrEmpty(txtSrchEmail.Text))
            {
                if (!string.IsNullOrEmpty(txtSrchName.Text))
                {
                    res += "&";
                }
                res += "email=" + txtSrchEmail.Text.Trim();
            }

            res = !string.IsNullOrEmpty(res.Trim()) ? "?" + res.Trim() : "";

            if (pageNum == 1)
            {               
                res = string.IsNullOrEmpty(res.Trim()) ? "?page=" + CurrPageNumber : res.Trim() + "&page=" + CurrPageNumber;
            }
            else
            {
                res = string.IsNullOrEmpty(res.Trim()) ? "?page=" + pageNum : res.Trim() + "&page=" + pageNum;
            }

            return res;
        }

        private void btnNex_Click(object sender, EventArgs e)
        {
            CurrPageNumber = CurrPageNumber + 1;
            if (CurrPageNumber == cmbPages.Items.Count)
            {
                btnNex.Enabled = false;
            }
            else
            {
                btnNex.Enabled = true;
            }

            if (CurrPageNumber == 1)
            {
                btnPrev.Enabled = false;
            }
            else
            {
                btnPrev.Enabled = true;
            }
            string st = Query();
            GridDatabind(st);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            CurrPageNumber = CurrPageNumber - 1;

            if (CurrPageNumber == 1)
            {
                btnPrev.Enabled = false;
            }
            else
            {
                btnPrev.Enabled = true;
            }

            if (CurrPageNumber == cmbPages.Items.Count)
            {
                btnNex.Enabled = false;
            }
            else
            {
                btnNex.Enabled = true;
            }

            string st = Query();
            GridDatabind(st);

        }

        private void cmbPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int page = !string.IsNullOrEmpty(cmbPages.SelectedItem.ToString()) ? Convert.ToInt32(cmbPages.SelectedItem.ToString()) : 1;
            string st = Query(page);
            GridDatabind(st);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnCsv_Click(object sender, EventArgs e)
        {
            if (dgEmployees.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Employees.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dgEmployees.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dgEmployees.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dgEmployees.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dgEmployees.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dgEmployees.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }
    }
}
