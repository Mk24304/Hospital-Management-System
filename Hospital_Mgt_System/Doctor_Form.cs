using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace Hospital_Mgt_System
{
    public partial class Doctor_Form : Form
    {
        public Doctor_Form()
        {
            InitializeComponent();
        }
        private void Doctor_Form_Load(object sender, EventArgs e)
        {
            RefreshGrid_Patient();
            RefreshGrid_Appt();
        }
        private void RefreshGrid_Patient()
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
        private void RefreshGrid_Appt()
        {
            string conString = "Data Source =.\\SQLEXPRESS; Initial Catalog = Loginform; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";
            SqlConnection con = new SqlConnection(conString);
            try
            {
                con.Open();
                string query = "Select * from Appointment";
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
        private void ClearFields()
        {
            txtApptId.Clear();
            comboBoxAppt.SelectedIndex = -1;
            comboBoxAppt.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string countquery = "Select Count(*) from Appointment where Appt_Id=@Appt_Id";
            SqlCommand countcmd = new SqlCommand(countquery, con);
            countcmd.Parameters.AddWithValue("@Appt_Id",txtApptId.Text);
            int count = (int)countcmd.ExecuteScalar();
            if (count <= 0)
            {
                MessageBox.Show("No Appointment with this Id has been yet Added in the Appointment list.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string sql = "UPDATE Appointment SET Doc_Id=@Doc_Id, appt_status=@appt_status ,Date_Time=@Date_Time WHERE Appt_Id=@Appt_Id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Doc_Id", txtDocId.Text);
                    cmd.Parameters.AddWithValue("@Appt_Id", txtApptId.Text);
                    cmd.Parameters.AddWithValue("@appt_status", comboBoxAppt.Text);
                    cmd.Parameters.AddWithValue("@Date_Time", dtpDateTime.Value);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Appointment Updated!");
                    RefreshGrid_Patient();
                    RefreshGrid_Appt();
                }
            }
            catch (Exception ex) { MessageBox.Show("Update failed: " + ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string connString = "Data Source=.\\SQLEXPRESS; Initial Catalog=Loginform; Integrated Security=True; Encrypt=True; TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            {
                try
                {
                    con.Open();
                    string sql = "DELETE FROM Appointment WHERE Appt_Id=@ApptId";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@ApptId", txtApptId.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Appointment Deleted.");
                    RefreshGrid_Patient();
                    RefreshGrid_Appt();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete error: " + ex.Message);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            RefreshGrid_Patient();
            RefreshGrid_Appt(); 
        }

        private void dgvAppt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < dgvAppt.Rows.Count)
            {
                DataGridViewRow row = dgvAppt.Rows[e.RowIndex];
                txtApptId.Text = row.Cells["Appt_Id"].Value.ToString();
                comboBoxAppt.Text = row.Cells["appt_status"].Value.ToString();
                if (row.Cells["Date_Time"].Value == null)
                {
                    dtpDateTime.Value = Convert.ToDateTime(row.Cells["Date_Time"].Value);
                }

            }
        }
    }
}
