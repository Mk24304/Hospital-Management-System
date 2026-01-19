using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital_Mgt_System
{
    public partial class Patient_Form : Form
    {
        public Patient_Form()
        {
            InitializeComponent();
        }

        private void Patient_Form_Load(object sender, EventArgs e)
        {
            RefreshGrid_Appt();
            RefreshGrid_Patient();
        }

        private void RefreshGrid()
        {
            string conString = "Data Source =.\\SQLEXPRESS; Initial Catalog = Loginform; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            SqlConnection con = new SqlConnection(conString);
            try
            {
                con.Open();
                string query = "Select * from Patient";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvPatient.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInputs() == false)
            {
                return;
            }
            if (comboBoxGender.SelectedItem == null)
            {
                MessageBox.Show("Please Select a Gender", "Input Error", MessageBoxButtons.OK);
                return;
            }
            string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            try
            {
                string sql = "INSERT INTO Patient(Id, Name, Bloodgroup, Condition, Phoneno, Gender, Address)" +
                "VALUES (@Id, @Name, @Bloodgroup, @Condition, @Phoneno, @Gender, @Address)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);
                cmd.Parameters.AddWithValue("@Name", txtFullName.Text);
                cmd.Parameters.AddWithValue("@Bloodgroup", txtBldGrp.Text);
                cmd.Parameters.AddWithValue("@Condition", txtCondition.Text);
                cmd.Parameters.AddWithValue("@Phoneno", txtPhoneno.Text);
                cmd.Parameters.AddWithValue("@Gender", comboBoxGender.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                string countquery = "Select Count(*) from logtable where Id=@Id";
                SqlCommand countcmd = new SqlCommand(countquery, con);
                countcmd.Parameters.AddWithValue("@Id", txtId.Text);
                int count = (int)countcmd.ExecuteScalar();
                if (count <= 0)
                {
                    MessageBox.Show("The Patient has not no signed in", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                cmd.ExecuteNonQuery();
                MessageBox.Show("Patient Added Successfully!");
                string sql_appt= "Insert into Appointment(Appt_Id,pt_Id Date_Time) " +
                                 "Values (NEXT VALUE FOR Appt_Seq,@pt_Id,@Date_Time)";
                SqlCommand cmd_appt = new SqlCommand(sql_appt, con);
                cmd_appt.Parameters.AddWithValue("@pt_Id", txtId.Text);
                cmd_appt.Parameters.AddWithValue("@Date_Time", dtpDateTime.Value);
                cmd_appt.ExecuteNonQuery();
                MessageBox.Show("Appointment time Added Successfully!");

                RefreshGrid_Patient();
                RefreshGrid_Appt();
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
            string connString = "Data Source =.\\SQLEXPRESS; Initial Catalog = Loginform; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            SqlConnection con = new SqlConnection(connString);

            try
            {
                con.Open();
                string sql = "UPDATE Patient SET Name=@Name, Bloodgroup=@Bloodgroup, Condition=@Condition, " +
                             "Phoneno=@Phoneno, Gender=@Gender, Address=@Address WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@Id", txtId.Text);
                cmd.Parameters.AddWithValue("@Name", txtFullName.Text);
                cmd.Parameters.AddWithValue("@Bloodgroup", txtBldGrp.Text);
                cmd.Parameters.AddWithValue("@Condition", txtCondition.Text);
                cmd.Parameters.AddWithValue("@Phoneno", txtPhoneno.Text);
                cmd.Parameters.AddWithValue("@Gender", comboBoxGender.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Updated!");
                string sql_appt = "UPDATE Appointment SET Date_Time=@Date_Time WHERE pt_Id=@pt_Id";
                SqlCommand cmd_appt = new SqlCommand(sql_appt, con);
                cmd.Parameters.AddWithValue("@Date_Time", dtpDateTime.Value);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Appointment time Updated!");
                RefreshGrid_Patient();
                RefreshGrid_Appt();
                ClearFields();
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
                string sql = "DELETE FROM Appointment WHERE pt_Id=@pt_Id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Appointment Deleted.");
                RefreshGrid();
                ClearFields();
            }
            catch (Exception ex) { MessageBox.Show("Delete error: " + ex.Message); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnShow_Click(object sender, EventArgs e)
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
            txtBldGrp.Clear();
            txtCondition.Clear();
            txtPhoneno.Clear();
            txtAddress.Clear();
            comboBoxGender.SelectedIndex = -1;

            txtId.Focus();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtFullName.Text) || 
                string.IsNullOrWhiteSpace(txtBldGrp.Text) || string.IsNullOrWhiteSpace(txtCondition.Text) ||
                string.IsNullOrWhiteSpace(txtPhoneno.Text) || string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please fill all the details.", "Input Error", MessageBoxButtons.OK);
                return false;
            }
            if (comboBoxGender.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Gender.", "Input Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
        private void RefreshGrid_Patient()
        {
            string conString = "Data Source =.\\SQLEXPRESS; Initial Catalog = Loginform; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            SqlConnection con = new SqlConnection(conString);
            try
            {
                con.Open();
                string query = "Select * from Patient where Id=@Id";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvPatient.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void RefreshGrid_Appt()
        {
            string conString = "Data Source =.\\SQLEXPRESS; Initial Catalog = Loginform; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            SqlConnection con = new SqlConnection(conString);
            try
            {
                con.Open();
                string query = "Select * from Appointment where parId=@pat_Id";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvAppt.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvPatient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPatient.Rows[e.RowIndex];
                txtId.Text = row.Cells["Id"].Value.ToString();
                txtFullName.Text = row.Cells["Name"].Value.ToString();
                txtBldGrp.Text = row.Cells["Bloodgroup"].Value.ToString();
                txtCondition.Text = row.Cells["Condition"].Value.ToString();
                txtPhoneno.Text = row.Cells["Phoneno"].Value.ToString();
                comboBoxGender.Text = row.Cells["Gender"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
            }
        }

        private void dtpDateTime_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Patient_Form_Load_1(object sender, EventArgs e)
        {
            RefreshGrid_Appt();
            RefreshGrid_Patient();
        }
    }
}