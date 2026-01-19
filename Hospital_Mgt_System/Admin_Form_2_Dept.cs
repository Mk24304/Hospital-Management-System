using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Hospital_Mgt_System
{
    public partial class Admin_Form_2_Dept : Form
    {
        public Admin_Form_2_Dept()
        {
            InitializeComponent();
        }
        private void Admin_Form_2_Dept_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void btnDoctors_Click(object sender, EventArgs e)
        {
            Admin_Form f_doctor = new Admin_Form();
            f_doctor.Show();
            this.Hide();
        }

        private void btnAppt_Click(object sender, EventArgs e)
        {
            
            Admin_Form_3_Appt f_appt = new Admin_Form_3_Appt();
            f_appt.Show();
            this.Hide();
        }
        private void RefreshGrid()
        {
            string conString = "Data Source =.\\SQLEXPRESS; Initial Catalog = Loginform; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            SqlConnection con = new SqlConnection(conString);
            try
            {
                con.Open();
                string query = "Select * from department";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvDept.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            Admin_Form_4_PatInfo f_patient= new Admin_Form_4_PatInfo();
            f_patient.Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInputs() == false) return;
            string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            try
            {
                con.Open();
                string sql = "INSERT INTO Department (Dept_ID, Dept_Name, No_of_doctors) VALUES (@Dept_Id, @Dept_Name, @No_of_doctors)";
                SqlCommand cmd = new SqlCommand(sql, con);
                
                  cmd.Parameters.AddWithValue("@Dept_Id", txtDeptId.Text);
                  cmd.Parameters.AddWithValue("@Dept_Name", txtDeptName.Text);
                  cmd.Parameters.AddWithValue("@No_of_doctors", txtNoOfDoc.Text);

                  cmd.ExecuteNonQuery();
                  MessageBox.Show("Department Added!");
                  RefreshGrid();
                  ClearFields();

                
            }
            catch (Exception ex) { MessageBox.Show("Add failed:" + ex.Message); }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateInputs() == false) return;
            string connString = "Data Source=.\\SQLEXPRESS; Initial Catalog=Loginform; Integrated Security=True; Encrypt=True; TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            {
                try
                {
                    con.Open();
                    string sql = "UPDATE Department SET Dept_Name=@Dept_Name, No_of_doctors=@No_of_doctors WHERE Dept_Id=@Dept_Id";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    {
                        cmd.Parameters.AddWithValue("@Dept_Id", txtDeptId.Text);
                        cmd.Parameters.AddWithValue("@Dept_Name", txtDeptName.Text);
                        cmd.Parameters.AddWithValue("@No_of_doctors", txtNoOfDoc.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Department Upadated!");
                        RefreshGrid();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update failed:" + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string connString = "Data Source.\\SQLEXPRESS; Initial Catalog=LoginForm; Integrated Security=True; Encrypt=True; TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            {
                try
                {
                    con.Open();
                    string sql = "DELETE FROM Department WHERE Dept_Id=@Dept_Id";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@Dept_Id", txtDeptId.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Department Deleted.");
                    RefreshGrid();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete error" + ex.Message);
                }
            }
        }
        private void ClearFields()
        {
            txtDeptId.Clear();
            txtDeptName.Clear();
            txtNoOfDoc.Clear();
            txtDeptId.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Admin_Form f_admin = new Admin_Form();
            f_admin.Show();
            this.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtDeptId.Text) || string.IsNullOrWhiteSpace(txtDeptName.Text) 
                        || string.IsNullOrWhiteSpace(txtNoOfDoc.Text))
            {
                MessageBox.Show("Please fill all the details.", "Input Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void dgvDept_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex <dgvDept.Rows.Count)
            {
                DataGridViewRow row = dgvDept.Rows[e.RowIndex];
                txtDeptId.Text = row.Cells["Dept_Id"].Value.ToString();
                txtDeptName.Text = row.Cells["Dept_Name"].Value.ToString();
                txtNoOfDoc.Text = row.Cells["No_of_doctors"].Value.ToString();
            }
        }

    }
}
