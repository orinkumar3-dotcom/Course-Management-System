using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TMs
{
    public partial class supstudentmang : Form
    {
        string constring = @"Data Source=localhost;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        private Form previousForm; 
        public supstudentmang(Form previousFrom)
        {
            InitializeComponent();
            this.previousForm = previousFrom;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadAllStudents(); // when click show, show all stdnts
        }

        private void supstudentmang_Load(object sender, EventArgs e)
        {
            LoadAllStudents();  // will fillup grid when page is load.
            LoadAllRequests(); // Load all pending requests when the form loads
        }

        private void LoadAllStudents()
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string query = @"SELECT userid, firstname, lastname, email,dob,password
                         FROM user_table
                         WHERE role = 'Student'";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string query = @"SELECT userid, firstname, lastname, email, password,dob
                         FROM user_table
                         WHERE userid = @userid
                         AND role = 'Student'";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@userid", textBox2.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Student not found.");

                    dataGridView1.DataSource = null;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter Student ID.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please enter a new password.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please confirm the password.");
                return;
            }

            if (textBox5.Text != textBox6.Text)
            {
                MessageBox.Show("Password and Confirm Password do not match.");
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string checkQuery = @"SELECT COUNT(*)
                              FROM user_table
                              WHERE userid = @userid
                              AND role = 'Student'";

                SqlCommand checkCmd = new SqlCommand(checkQuery, con);

                checkCmd.Parameters.AddWithValue("@userid", textBox2.Text);

                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    MessageBox.Show("Student not found.");
                    return;
                }

                string updateQuery = @"UPDATE user_table
                               SET password = @password
                               WHERE userid = @userid
                               AND role = 'Student'";

                SqlCommand updateCmd = new SqlCommand(updateQuery, con);

                updateCmd.Parameters.AddWithValue("@userid", textBox2.Text);
                updateCmd.Parameters.AddWithValue("@password", textBox5.Text);

                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Student password updated successfully.");

                // Clear password fields
                textBox5.Clear();
                textBox6.Clear();

                // Refresh grid
                LoadAllStudents();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                // First get the student's current information
                string selectQuery = @"SELECT firstname, lastname, email
                               FROM user_table
                               WHERE userid = @userid
                               AND role = 'Student'";

                SqlCommand selectCmd = new SqlCommand(selectQuery, con);

                selectCmd.Parameters.AddWithValue("@userid", textBox2.Text);

                SqlDataReader reader = selectCmd.ExecuteReader();

                if (!reader.Read())
                {
                    MessageBox.Show("Student not found.");
                    reader.Close();
                    return;
                }

                // Existing database values
                string oldFirstName = reader["firstname"].ToString();
                string oldLastName = reader["lastname"].ToString();
                string oldEmail = reader["email"].ToString();

                // If textbox is empty, keep old value
                string newFirstName = string.IsNullOrWhiteSpace(textBox1.Text)
                    ? oldFirstName
                    : textBox1.Text;

                string newLastName = string.IsNullOrWhiteSpace(textBox3.Text)
                    ? oldLastName
                    : textBox3.Text;

                string newEmail = string.IsNullOrWhiteSpace(textBox4.Text)
                    ? oldEmail
                    : textBox4.Text;

                reader.Close();

                // Update database
                string updateQuery = @"UPDATE user_table
                               SET firstname = @firstname,
                                   lastname = @lastname,
                                   email = @email
                               WHERE userid = @userid
                               AND role = 'Student'";

                SqlCommand updateCmd = new SqlCommand(updateQuery, con);

                updateCmd.Parameters.AddWithValue("@userid", textBox2.Text);
                updateCmd.Parameters.AddWithValue("@firstname", newFirstName);
                updateCmd.Parameters.AddWithValue("@lastname", newLastName);
                updateCmd.Parameters.AddWithValue("@email", newEmail);

                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Student information updated successfully.");

                textBox1.Clear();
                textBox3.Clear();
                textBox4.Clear();

                // Refresh grid
                LoadAllStudents();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text)) 
            {
                MessageBox.Show("Please enter Student ID.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this student?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "DELETE FROM user_table WHERE userid = @userid";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@userid", textBox2.Text);

                con.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Student deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Student ID not found.");
                }

                LoadAllStudents();
                //ClearFields();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                //MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Form1 y= new Form1();
                y.Show();
                this.Hide();
            }
        }
        protected override void OnKeyDown(KeyEventArgs C)
        {
            base.OnKeyDown(C);

            if (C.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                //MessageBox.Show("Escape detected in admin.cs");
                if (previousForm != null)
                {
                    previousForm.Show();
                }
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadAllRequests()
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string query = @"SELECT c.cartid, c.userid, u.firstname, u.lastname,
                                 c.courseid, co.coursename, co.courseprice,
                                 c.status, c.date_added
                          FROM cart_table c
                          JOIN user_table u ON c.userid = u.userid
                          JOIN course_table co ON c.courseid = co.courseid
                          WHERE c.status = 'Pending'
                          ORDER BY c.date_added";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt; 
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            LoadAllRequests(); 
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a request to accept.");
                return;
            }

            string cartId = dataGridView2.SelectedRows[0].Cells["cartid"].Value.ToString();

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string query = "UPDATE cart_table SET status = 'Approved' WHERE cartid = @cartid";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cartid", cartId);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Request accepted.");
                }
                else
                {
                    MessageBox.Show("Could not accept request.");
                }

                LoadAllRequests(); // refresh 
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Accept ALL pending requests?",
            "Confirm",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string query = "UPDATE cart_table SET status = 'Approved' WHERE status = 'Pending'";

                SqlCommand cmd = new SqlCommand(query, con);

                int rows = cmd.ExecuteNonQuery();

                MessageBox.Show(rows + " request(s) accepted.");

                LoadAllRequests();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a request to cancel.");
                return;
            }

            string cartId = dataGridView2.SelectedRows[0].Cells["cartid"].Value.ToString();

            DialogResult result = MessageBox.Show(
                "Cancel this student's request?",
                "Confirm Cancel",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string query = "UPDATE cart_table SET status = 'Rejected' WHERE cartid = @cartid";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cartid", cartId);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Request cancelled.");
                }
                else
                {
                    MessageBox.Show("Could not cancel request.");
                }

                LoadAllRequests();
            }
        }
    }
}
