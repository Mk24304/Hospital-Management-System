using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace Hospital_Mgt_System
{
    public partial class Payment : Form
    {
        public Payment()
        {
            InitializeComponent();
        }
        string patId;
        public Payment(string id)
        {
            InitializeComponent();
            patId = id;
        }
        private void Payment_Load(object sender, EventArgs e)
        {
            txtId.Text = patId;
            RefreshGrid();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInputs() == false)
            {
                return;

            }
            if (comboBoxAmt.SelectedItem == null)
            {
                MessageBox.Show("Please select an option.", "Input Error", MessageBoxButtons.OK);
                return;
            }
            string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string value = comboBoxAmt.Text;
            string[] parts = value.Split('-');
            if (parts.Length > 0 && int.TryParse(parts[0], out int sum))
            {
                try
                {
                    string countquery = "Select Count(*) from patient where Id=@Id";
                    SqlCommand countcmd = new SqlCommand(countquery, con);
                    countcmd.Parameters.AddWithValue("@Id", txtId.Text);
                    int count = (int)countcmd.ExecuteScalar();
                    if (count <= 0)
                    {
                        MessageBox.Show("No Paitent with this Id", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    int apptPay = 100;
                    string query_info = "SELECT condition from patient where Id=@Id";
                    SqlCommand cmd_info = new SqlCommand(query_info, con);
                    cmd_info.Parameters.AddWithValue("@Id", txtId.Text);
                    string condition = cmd_info.ExecuteScalar()?.ToString();
                    string insertQuery = "INSERT INTO Bill(Bill_Id,pt_Id,total_amount,info) VALUES (NEXT VALUE FOR bill_seq,@pt_Id,@total_amount,@info)";
                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@pt_Id", txtId.Text);
                    cmd.Parameters.AddWithValue("@total_amount", sum + apptPay);
                    cmd.Parameters.AddWithValue("@info", condition);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Paymement Successful. Please return back to your dashboard", "Message", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message);
                }

            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtBillId.Text)||string.IsNullOrWhiteSpace(txtId.Text))
            {
                    MessageBox.Show("Please enter your Bill Id and patient", "Input Error", MessageBoxButtons.OK);
                    return;
            }
            string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string countquery = "SELECT COUNT(*) From patient Where Id=@Id";
            SqlCommand sql = new SqlCommand(countquery, con);
            sql.Parameters.AddWithValue("@Id", txtId.Text);
            int count = (int)sql.ExecuteScalar();
            if (count <= 0)
            {
                MessageBox.Show("No Paitent with this Id", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string value = comboBoxAmt.Text;
            string[] parts = value.Split('-');
            if (parts.Length > 0 && int.TryParse(parts[0], out int sum))
            {
                try
                {
                    int apptPay = 100;
                    string query_info = "SELECT condition from patient where Id=@Id";
                    SqlCommand cmd_info = new SqlCommand(query_info, con);
                    cmd_info.Parameters.AddWithValue("@Id", txtId.Text);
                    string condition = cmd_info.ExecuteScalar()?.ToString();
                    string updateQuery = "UPDATE Bill SET total_amount=@total_amount,info=@info WHERE pt_Id=@pt_Id and Bill_Id=@Bill_Id";
                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@Bill_Id", txtBillId.Text);
                    cmd.Parameters.AddWithValue("@pt_Id", txtId.Text);
                    cmd.Parameters.AddWithValue("@total_amount", sum + apptPay);
                    cmd.Parameters.AddWithValue("@info", condition);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Payment Updated Successfully", "Message", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message);
                }
            }


        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBillId.Text))
            {
                MessageBox.Show("Please enter your Bill Id", "Input Error", MessageBoxButtons.OK);
            }
            string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            try
            {
                string deleteQuery = "DELETE FROM Bill WHERE Bill_Id=@Bill_Id";
                SqlCommand cmd = new SqlCommand(deleteQuery, con);
                cmd.Parameters.AddWithValue("@Bill_Id", txtBillId.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Payment Deleted Successfully", "Message", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }



        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Please enter your patient Id", "Input Error", MessageBoxButtons.OK);
                return false;
            }
            if (comboBoxAmt.SelectedItem == null)
            {
                MessageBox.Show("Please slect an option.", "Input Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Please enter your patient Id", "Input Error", MessageBoxButtons.OK);
                return;
            }
            string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            try
            {
                string query = "SELECT * FROM Bill Where pt_Id=@pt_Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pt_Id", txtId.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvAmt.DataSource = dt;
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No payment with this Id.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }

        }
        public void RefreshGrid()
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Please enter your patient Id", "Input Error", MessageBoxButtons.OK);
                return;
            }
            string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            try
            {
                string query = "SELECT * FROM Bill Where pt_Id=@pt_Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pt_Id", txtId.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvAmt.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Patient_Form f = new Patient_Form();
            f.Show();
            this.Close();
        }
    }
}
