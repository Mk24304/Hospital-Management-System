using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital_Mgt_System
{
    public partial class Admin_Form : Form
    {
        public Admin_Form()
        {
            InitializeComponent();
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            Admin_Form_2_Dept f_dept = new Admin_Form_2_Dept();
            f_dept.Show();
            this.Hide();
        }

        private void btnAppt_Click(object sender, EventArgs e)
        {
            Admin_Form_3_Appt f_appt = new Admin_Form_3_Appt();
            f_appt.Show();
            this.Hide();
        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            Admin_Form_4_PatInfo f_patient = new Admin_Form_4_PatInfo();
            f_patient.Show();
            this.Hide();
        }
        private void RefreshGrid()
        {
            string conString = "Data Source =.\\SQLEXPRESS; Initial Catalog = Loginform; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            SqlConnection con = new SqlConnection(conString);
            try
            {
                con.Open();
                string query = "Select * from doctor";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvDoctors.DataSource = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(ValidateInputs()== false)
            {
                return;
            }
            if(comboBoxDept.SelectedItem == null)
            {
                MessageBox.Show("Please Select a Department", "Input Error", MessageBoxButtons.OK);
                return;
            }
            string deptIdText = comboBoxDept.Text;
            string[] parts = deptIdText.Split('-');
            if(parts.Length==0)
            {
                MessageBox.Show("Invalid department format.", "Input Error", MessageBoxButtons.OK);
                return;
            }
            string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string countquery = "Select Count(*) from logtable where Id=@Id";
            SqlCommand countcmd = new SqlCommand(countquery, con);
            countcmd.Parameters.AddWithValue("@Id", txtId.Text);
            int count = (int)countcmd.ExecuteScalar();
            if (count < 0)
            {
                MessageBox.Show("The Doctor has not no signed in", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            try
            {
                  string sql = "INSERT INTO doctor (Id, Experience, Specialization, Salary, FullName, Dept_Id)" +
                  "VALUES (@Id, @Experience, @Specialization, @Salary, @FullName, @Dept_Id)";
                  SqlCommand cmd = new SqlCommand(sql, con);
                 string deptIdOnly= parts[0].Trim(); // Extract only the ID part
                  cmd.Parameters.AddWithValue("@Id", txtId.Text);
                  cmd.Parameters.AddWithValue("@Experience", txtExp.Text);
                  cmd.Parameters.AddWithValue("@Specialization", txtSpl.Text);
                  cmd.Parameters.AddWithValue("@Salary", txtSalary.Text);
                  cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                  cmd.Parameters.AddWithValue("@Dept_Id", deptIdOnly);

                  cmd.ExecuteNonQuery();

                  MessageBox.Show("Doctor Added Successfully!");
                  RefreshGrid();
                  ClearFields();
            
            }
            catch (Exception ex)
            {
                 MessageBox.Show("Add failed:" + ex.Message);
            }
  

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateInputs() == false)
            {
                return;
            }
            string deptIdText = comboBoxDept.Text;
            string[] parts = deptIdText.Split('-');
            string connString = "Data Source =.\\SQLEXPRESS; Initial Catalog = Loginform; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                string sql = "UPDATE doctor SET FullName=@FullName, Specialization=@specialization, Salary=@Salary, " +
                         "Experience=@Experience, Dept_Id=@Dept_Id WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, con);
                string deptIdOnly = parts[0].Trim(); // Extract only the ID part
                cmd.Parameters.AddWithValue("@Id", txtId.Text);
                    cmd.Parameters.AddWithValue("@Experience", txtExp.Text);
                    cmd.Parameters.AddWithValue("@specialization", txtSpl.Text);
                    cmd.Parameters.AddWithValue("@Salary", txtSalary.Text);
                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                    cmd.Parameters.AddWithValue("@Dept_Id", deptIdOnly);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record Updated!");
                    RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update error." + ex.Message);
            }
        }
    
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text)) return;
            string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                string sql = "DELETE FROM doctor WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Doctor Deleted.");
                RefreshGrid();
                ClearFields();            
            }
            catch (Exception ex) { MessageBox.Show("Delete error: " + ex.Message); }
            ///Delete

        }

        private void Admin_Form_Load(object sender, EventArgs e)
        {
            RefreshGrid();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void ClearFields()
        {
            txtId.Clear();
            txtFullName.Clear();
            txtSpl.Clear();
            txtSalary.Clear();
            txtExp.Clear();
            comboBoxDept.SelectedIndex = -1;

            txtId.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text)|| string.IsNullOrWhiteSpace(txtSpl.Text)|| string.IsNullOrWhiteSpace(txtSalary.Text)|| string.IsNullOrWhiteSpace(txtFullName.Text)
                        || string.IsNullOrWhiteSpace(txtExp.Text))
            {
                MessageBox.Show("Please fill all the details.", "Input Error", MessageBoxButtons.OK);
                return false;
            }
            if (comboBoxDept.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Dpartment.", "Input Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void dgvDoctors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string deptIdText = comboBoxDept.Text;
            string[] parts = deptIdText.Split('-');
            string deptIdOnly = parts[0].Trim();
            if (e.RowIndex >=0)
            {
                DataGridViewRow row = dgvDoctors.Rows[e.RowIndex];
                txtId.Text = row.Cells["Id"].Value.ToString();
                txtFullName.Text = row.Cells["FullName"].Value.ToString();
                txtSpl.Text = row.Cells["Specialization"].Value.ToString();
                txtSalary.Text = row.Cells["Salary"].Value.ToString();
                txtExp.Text = row.Cells["Experience"].Value.ToString();
                comboBoxDept.Text = row.Cells["Dept_Id"].Value.ToString();
            }
        }
    }
}

