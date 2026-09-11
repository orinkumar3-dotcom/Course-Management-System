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
    public partial class RequestCourses : Form
    {
        string constring = @"Data Source=localhost;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";
        private Form previous;
        public RequestCourses(Form calling)
        {
            InitializeComponent();
            previous = calling;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void RequestCourses_Load(object sender, EventArgs e)
        {
            LoadAllCourses();
        }

        private void LoadAllCourses()
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string query = "SELECT * FROM course_table";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt; // "All Courses" grid
            }
        }

        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        protected override void OnKeyDown(KeyEventArgs A)
        {
            base.OnKeyDown(A);

            if (A.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) 
        {
            if (keyData == Keys.Escape)
            {
                //MessageBox.Show("Escape detected in admin.cs");
                if (previous != null)
                {
                    previous.Show();
                }
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void label1_Click(object sender, EventArgs e) 
        {
            
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Contactus cn = new Contactus();
            this.Hide();
            cn.ShowDialog();
            this.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text) ||
        string.IsNullOrWhiteSpace(textBox1.Text) ||
        comboBox1.SelectedItem == null ||
        comboBox2.SelectedItem == null ||
        string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                // Prevent duplicate Course ID
                string checkQuery = "SELECT COUNT(*) FROM course_table WHERE courseid = @courseid";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@courseid", textBox2.Text);

                int exists = (int)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    MessageBox.Show("This Course ID already exists.");
                    return;
                }

                string insertQuery = @"INSERT INTO course_table (courseid, coursename, coursetype, coursetime,courseprice)
                 VALUES (@courseid, @coursename, @coursetype, @coursetime,@courseprice)";

                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@courseid", textBox2.Text);
                cmd.Parameters.AddWithValue("@coursename", textBox1.Text);
                cmd.Parameters.AddWithValue("@coursetype", comboBox1.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@coursetime", comboBox2.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@courseprice", textBox3.Text); 

                cmd.ExecuteNonQuery();

                MessageBox.Show("Course added successfully.");

                LoadAllCourses();
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
                con.Open();

                string query = "SELECT * FROM course_table WHERE courseid = @courseid";

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
                    MessageBox.Show("Course ID not found.");
                    dataGridView1.DataSource = null;
                }
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

               string selectQuery = @"SELECT coursename, coursetype, coursetime, courseprice
                FROM course_table
                WHERE coursed = @courseid";

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
                double oldCoursePrice = Convert.ToDouble(reader["courseprice"]);

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
                                 WHERE courseid = @courseid";

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

                LoadAllCourses();
                ClearFields();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Form1 y = new Form1();
                y.Show();
                this.Hide();
            }
        }
    }
}
