using Microsoft.Data.SqlClient;
using System;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Hospital_Mgt_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Select Role");
                return;
            }
            SqlConnection con = new SqlConnection("Data Source =.\\SQLEXPRESS; Initial Catalog = Loginform; Integrated Security = True; Encrypt = True; TrustServerCertificate = True");
            con.Open();
            string countquery = "Select Count(*) from logtable where username=@Username and password=@Password";
            SqlCommand countcmd = new SqlCommand(countquery, con);
            countcmd.Parameters.AddWithValue("@Username", txtUser.Text);
            countcmd.Parameters.AddWithValue("@Password", txtPass.Text);
            int count = (int)countcmd.ExecuteScalar();
            if (count > 0)
            {
                MessageBox.Show("Login Successful", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Login Failed. First Sign in");
            }
            string query="Select * from logtable where username=@Username and password=@Password";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Username", txtUser.Text);
            cmd.Parameters.AddWithValue("@Password", txtPass.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();
            string cmbItemValue = comboBox1.SelectedItem.ToString();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Role"].ToString() == cmbItemValue)
                    {
                        if (comboBox1.SelectedIndex == 0)
                        {
                            MessageBox.Show($"You are logged in as {dt.Rows[i][3]}");
                            Doctor_Form f = new Doctor_Form();
                            f.Show();
                            this.Hide();
                        }
                        else if (comboBox1.SelectedIndex == 1)
                        {
                            MessageBox.Show($"You are logged in as {dt.Rows[i][3]}");
                            Patient_Form f = new Patient_Form();
                            f.Show();
                            this.Hide();
                        }
                        else if (comboBox1.SelectedIndex == 2)
                        {
                            Admin_Form f = new Admin_Form();
                            f.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Role");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select Role");
                    }

                }
            }
            else
            {
                MessageBox.Show("Invalid Username or Password or Role Not selected");
            }
        }
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            SignIn f = new SignIn();
            f.Show();
            this.Hide();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
