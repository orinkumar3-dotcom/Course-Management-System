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

namespace TMs
{
    public partial class superadmin : Form
    {
        private Form previousForm;

        string constring = @"Data Source=localhost;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public superadmin(Form previousFrom)
        {
            InitializeComponent();
            //previousForm= call;
            this.previousForm = previousFrom;
        }

        private void superadmin_Load(object sender, EventArgs e)
        {
            LoadCourses();

        }

        private void LoadCourses()
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string query = @"SELECT courseid, coursename, coursetype, coursetime, courseprice, discountpercent,
                 CAST(courseprice - (courseprice * discountpercent / 100) AS DECIMAL(10,2)) AS Finalprice
                 FROM course_table";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt; 
            }
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //for logout
        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Form1 y= new Form1();
                y.Show();
                this.Hide();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("Please fill all information.");
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                // Check if Course ID already exists
              string query = @"SELECT courseid, coursename, coursetype, coursetime, courseprice, discountpercent,
                 CAST(courseprice - (courseprice * discountpercent / 100) AS DECIMAL(10,2)) AS Finalprice
                 FROM course_table";

                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@courseid", textBox2.Text);

                int count = (int)checkCmd.ExecuteScalar();
                
                if (count > 0)
                {
                    MessageBox.Show("Course ID already exists. Please use a different Course ID.");
                    return;
                }

                 string query = @"SELECT courseid, coursename, coursetype, coursetime, courseprice, discountpercent,
                 CAST(courseprice - (courseprice * discountpercent / 100) AS DECIMAL(10,2)) AS Finalprice
                 FROM course_table";
                   
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@courseid", textBox2.Text);
                    cmd.Parameters.AddWithValue("@coursename", textBox1.Text);
                    cmd.Parameters.AddWithValue("@coursetype", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@coursetime", comboBox2.Text);
                    cmd.Parameters.AddWithValue("@courseprice", textBox3.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Course inserted successfully.");

                    LoadCourses();
                    ClearFields();
                }
            }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter Course ID.");
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

               string query = "SELECT userid, firstname, lastname, email, password, dob FROM user_table WHERE userid = @userid";

                SqlCommand selectCmd = new SqlCommand(selectQuery, con);
                selectCmd.Parameters.AddWithValue("@courseid", textBox2.Text);

                SqlDataReader reader = selectCmd.ExecuteReader();

                if (!reader.Read())
                {
                    reader.Close();
                    MessageBox.Show("Course ID not found.");
                    return;
                }

                string oldCourseName = reader["coursename"].ToString();
                string oldCourseType = reader["coursetype"].ToString();
                string oldCourseTime = reader["coursetime"].ToString();
                Double oldCoursePrice = Convert.ToDouble(reader["courseprice"]);

                reader.Close();

                // If user leaves a field empty, keep the old value
                string newCourseName = string.IsNullOrWhiteSpace(textBox1.Text)
                    ? oldCourseName
                    : textBox1.Text;

                string newCourseType = string.IsNullOrWhiteSpace(comboBox1.Text)
                    ? oldCourseType
                    : comboBox1.Text;

                string newCourseTime = string.IsNullOrWhiteSpace(comboBox2.Text)
                    ? oldCourseTime
                    : comboBox2.Text;
                double newCoursePrice = string.IsNullOrWhiteSpace(textBox3.Text)
                    ? oldCoursePrice
                    : Convert.ToDouble(textBox3.Text);

                string updatequery = @"UPDATE course_table
                 SET coursename = @coursename,
                     coursetype = @coursetype,
                     coursetime = @coursetime,
                     courseprice = @courseprice
                 WHERE coursed = @courseid";

                SqlCommand updatecmd = new SqlCommand(updatequery, con);

                updatecmd.Parameters.AddWithValue("@courseid", textBox2.Text);
                updatecmd.Parameters.AddWithValue("@coursename", newCourseName);
                updatecmd.Parameters.AddWithValue("@coursetype", newCourseType);
                updatecmd.Parameters.AddWithValue("@coursetime", newCourseTime);
                updatecmd.Parameters.AddWithValue("@courseprice", newCoursePrice);

                int result = updatecmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Course updated successfully.");
                }
                else
                {
                    MessageBox.Show("Course ID not found.");
                }

                LoadCourses();
                ClearFields();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter Course ID.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this course?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "DELETE FROM course_table WHERE courseid = @courseid";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@courseid", textBox2.Text);

                con.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Course deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Course ID not found.");
                }

                LoadCourses();
                ClearFields();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter Course ID.");
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = @"SELECT *
                                 FROM course_table
                                 WHERE courseid = @courseid";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@courseid", textBox2.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count > 0)
                { 
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Course not found.");
                }
            }
        }

        private void ClearFields()
        {
            textBox2.Clear();
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text)) // Course Id textbox
            {
                MessageBox.Show("Please enter Course ID.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox4.Text)) // Discount(%) textbox
            {
                MessageBox.Show("Please enter a discount percentage.");
                return;
            }

            if (!decimal.TryParse(textBox4.Text, out decimal discountValue) || discountValue < 0 || discountValue > 100)
            {
                MessageBox.Show("Discount must be a number between 0 and 100.");
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

              string query = @"UPDATE course_table SET discountpercent = @discount WHERE coursed = @courseid";
                  
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@discount", discountValue);
                cmd.Parameters.AddWithValue("@courseid", textBox2.Text);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Discount applied to course " + textBox2.Text + ".");
                }
                else
                {
                    MessageBox.Show("Course ID not found.");
                }

                LoadCourses();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            supstudentmang b= new supstudentmang(this);
            b.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please enter a discount percentage.");
                return;
            }

            if (!decimal.TryParse(textBox4.Text, out decimal discountValue) || discountValue < 0 || discountValue > 100)
            {
                MessageBox.Show("Discount must be a number between 0 and 100.");
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Apply {discountValue}% discount to ALL courses?",
                "Confirm Bulk Discount",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

               string query = "UPDATE course_table SET discountpercent = @discount"; // no WHERE = every row

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@discount", discountValue);

                int rows = cmd.ExecuteNonQuery();

                MessageBox.Show($"Discount applied to {rows} course(s).");

                LoadCourses();
            }
        }
        protected override void OnKeyDown(KeyEventArgs B)
        {
            base.OnKeyDown(B);

            if (B.KeyCode == Keys.Escape)
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
    }
}
