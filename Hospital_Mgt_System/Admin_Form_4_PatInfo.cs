using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Hospital_Mgt_System
{
    public partial class Admin_Form_4_PatInfo : Form
    {
        public Admin_Form_4_PatInfo()
        {
            InitializeComponent();
        }
        private void Admin_Form_4_PatInfo_Load(object sender, EventArgs e)
        {
            RefreshGrid();

        }

        private void btnDoctors_Click(object sender, EventArgs e)
        {
            Admin_Form f_doctor = new Admin_Form();
            f_doctor.Show();
            this.Hide();
        }


        private void btnDept_Click_1(object sender, EventArgs e)
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
            string countquery = "Select Count(*) from logtable where Id=@Id";
            SqlCommand countcmd = new SqlCommand(countquery, con);
            countcmd.Parameters.AddWithValue("@Id", txtId.Text);
            int count = (int)countcmd.ExecuteScalar();
            if (count < 0)
            {
                MessageBox.Show("The Patient has not no signed in", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            try
            {
                string sql = "INSERT INTO Patient(Id, Name, Bloodgroup, Condition, Phoneno, Gender,Address)" +
                "VALUES (@Id, @Name, @Bloodgroup, @Condition, @Phoneno, @Gender,@Address)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);
                cmd.Parameters.AddWithValue("@Name", txtFullName.Text);
                cmd.Parameters.AddWithValue("@Bloodgroup", txtBldGrp.Text);
                cmd.Parameters.AddWithValue("@Condition", txtCondition.Text);
                cmd.Parameters.AddWithValue("@Phoneno", txtPhoneno.Text);
                cmd.Parameters.AddWithValue("@Gender", comboBoxGender.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Patient Added Successfully!");
                RefreshGrid();
                ClearFields();

            }
            catch(Exception ex)
            {
                MessageBox.Show("Add failed:" + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateInputs() == false) return;
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                SqlConnection con = new SqlConnection(connString);
                try
                {
                    con.Open();

                    string sql = @"UPDATE Patient 
                   SET Name=@Name,
                       Bloodgroup=@Bloodgroup,
                       Condition=@Condition,
                       Phoneno=@Phoneno,
                       Gender=@Gender,
                       Address=@Address
                   WHERE Id=@Id";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", txtId.Text);
                        cmd.Parameters.AddWithValue("@Name", txtFullName.Text);
                        cmd.Parameters.AddWithValue("@Bloodgroup", txtBldGrp.Text);
                        cmd.Parameters.AddWithValue("@Condition", txtCondition.Text);
                        cmd.Parameters.AddWithValue("@Phoneno", txtPhoneno.Text);
                        cmd.Parameters.AddWithValue("@Gender", comboBoxGender.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);

                        cmd.ExecuteNonQuery();


                        MessageBox.Show("Patient Updated Successfully!");
                        RefreshGrid();
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Update failed: " + ex.Message);
                }
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
                string sql = "DELETE FROM Patient WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Id", txtId.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Patient Deleted.");
                RefreshGrid();
                ClearFields();
            }
            catch (Exception ex) { MessageBox.Show("Delete error: " + ex.Message); } 
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Admin_Form_3_Appt f=new Admin_Form_3_Appt();
            f.Show();
            this.Close();

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            RefreshGrid();

        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtBldGrp.Text) || string.IsNullOrWhiteSpace(txtCondition.Text)
                        || string.IsNullOrWhiteSpace(txtPhoneno.Text)|| string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please fill all the details.", "Input Error", MessageBoxButtons.OK);
                return false;
            }
            if (comboBoxGender.SelectedItem == null)
            {
                MessageBox.Show("Please select a Dpartment.", "Input Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
        private void ClearFields()
        {
            txtId.Clear();
            txtFullName.Clear();
            txtBldGrp.Clear();
            txtCondition.Clear();
            txtPhoneno.Clear();
            comboBoxGender.SelectedIndex = -1;
        }

    }
}
