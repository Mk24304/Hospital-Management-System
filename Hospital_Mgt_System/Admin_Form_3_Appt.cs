using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Hospital_Mgt_System
{
    public partial class Admin_Form_3_Appt : Form
    {
        public Admin_Form_3_Appt()
        {
            InitializeComponent();
        }
        private void btnPatients_Click(object sender, EventArgs e)
        {
            Admin_Form_4_PatInfo f_patient = new Admin_Form_4_PatInfo();
            f_patient.Show();
            this.Hide();
        }

        private void btnDept_Click_1(object sender, EventArgs e)
        {
            Admin_Form_2_Dept f_dept = new Admin_Form_2_Dept();
            f_dept.Show();
            this.Hide();
        }

        private void btnDoctors_Click(object sender, EventArgs e)
        {
            Admin_Form f_doctor = new Admin_Form();
            f_doctor.Show();
            this.Hide();
        }
        private void Admin_Form_3_Appt_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        private void RefreshGrid()
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInputs() == false) return;
            string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Loginform;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            string countquery = "Select Count(*) from Patient where Id=@Id";
            SqlCommand countcmd = new SqlCommand(countquery, con);
            countcmd.Parameters.AddWithValue("@Id", txtPatId.Text);
            int count = (int)countcmd.ExecuteScalar();
            if (count <= 0)
            {
                MessageBox.Show("No Patient with this Id has been yet Added in the Patient list. Please add the patient in the list first", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                
                string sql = "INSERT INTO Appointment (Appt_Id, Doc_Id, pt_Id, appt_status,Date_Time) VALUES (@Appt_Id, @Doc_Id, @pt_Id, @appt_status,@Date_Time)";
                SqlCommand cmd = new SqlCommand(sql, con);
                
                 cmd.Parameters.AddWithValue("@Appt_Id", txtApptId.Text);
                 cmd.Parameters.AddWithValue("@Doc_Id", txtDoctorId.Text);
                 cmd.Parameters.AddWithValue("@pt_Id", txtPatId.Text);
                 cmd.Parameters.AddWithValue("@appt_status", comboBoxAppt.Text);
                 cmd.Parameters.AddWithValue("@Date_Time", dtpDateTime.Value);

                cmd.ExecuteNonQuery();
                 MessageBox.Show("Appointment Added!");
                 RefreshGrid();
                 ClearFields();

                
            }
            catch (Exception ex) { MessageBox.Show("Add failed:" + ex.Message); }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateInputs() == false) return;
            string connString = "Data Source=.\\SQLEXPRESS; Initial Catalog=Loginform; Integrated Security=Trus; Encrypt=True; TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connString);


            try
            {
                con.Open();
                string sql = "UPDATE Appointment SET Appt_Id=@Appt_Id, appt_statuss=@appt_status , Doc_Id=@Doc_Id,pt_Id=@ptId,Date_Time=@Date_Time WHERE pt_Id=@pt_Id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Appt_Id", txtApptId.Text);
                    cmd.Parameters.AddWithValue("@Doc_Id", txtDoctorId.Text);
                    cmd.Parameters.AddWithValue("@pt_Id", txtPatId.Text);
                    cmd.Parameters.AddWithValue("@appt_status", comboBoxAppt.Text);
                    cmd.Parameters.AddWithValue("@Date_Time", dtpDateTime.Value);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Appointment Updated!");
                    RefreshGrid();
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
                    RefreshGrid();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete error: " + ex.Message);
                }
            }
        }
        private void ClearFields()
        {
            txtApptId.Clear();
            txtDoctorId.Clear();
            txtPatId.Clear();
            comboBoxAppt.SelectedIndex = -1;
            txtApptId.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Admin_Form_2_Dept f_dept= new Admin_Form_2_Dept();
            f_dept.Show();
            this.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtApptId.Text) || string.IsNullOrWhiteSpace(txtDoctorId.Text) || string.IsNullOrWhiteSpace(txtPatId.Text))
            {
                MessageBox.Show("Please fill all the details.", "Input Error", MessageBoxButtons.OK);
                return false;
            }
            if (comboBoxAppt.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Appointment Status.", "Input Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void dgvAppt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ( e.RowIndex < dgvAppt.Rows.Count)
            {
                DataGridViewRow row = dgvAppt.Rows[e.RowIndex];
                txtApptId.Text = row.Cells["Appt_Id"].Value.ToString();
                txtDoctorId.Text = row.Cells["Doc_Id"].Value.ToString();
                txtPatId.Text = row.Cells["pt_Id"].Value.ToString();
                comboBoxAppt.Text = row.Cells["appt_status"].Value.ToString();
                if (row.Cells["Date_Time"].Value== null)
                {
                    dtpDateTime.Value = Convert.ToDateTime(row.Cells["Date_Time"].Value);
                }

            }
        }

    }
}
