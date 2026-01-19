using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System.IdentityModel.Selectors;

namespace Hospital_Mgt_System
{
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Select Role");
                return;
            }
            SqlConnection con = new SqlConnection("Data Source =.\\SQLEXPRESS; Initial Catalog = Loginform; Integrated Security = True; Encrypt = True; TrustServerCertificate = True");
            try
            {
                con.Open();
                string countqry= "SELECT COUNT(*) FROM logtable WHERE username=@Username OR @password=password";
                SqlCommand checkcmd = new SqlCommand(countqry, con);
                checkcmd.Parameters.AddWithValue("@Username", txtUser.Text);
                checkcmd.Parameters.AddWithValue("@Password", txtPass.Text);
                int userExists = Convert.ToInt32(checkcmd.ExecuteScalar());
                if(userExists >0)
                {
                    MessageBox.Show("UserName or Password already exists. Please enter another.");
                }
                else
                {
                    string insertQuery = "Insert into logtable (Id,Username,Password,Role) values(NEXT VALUE FOR LogId_sequence,@username,@password,@Role)";
                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@Username", txtUser.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPass.Text);
                    cmd.Parameters.AddWithValue("@Role", comboBox1.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sign In Successsful !", "Info", MessageBoxButtons.OK);
                    return;
                }

            }
            catch(Exception exx)
            {
                MessageBox.Show("Error Occured" + exx.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUser.Clear();
            txtPass.Clear();
            comboBox1.SelectedIndex = -1;
            txtUser.Focus(); // To set Foccus back to username field
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Close();
        }
    }
}
