using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TMs
{
    public partial class sProfile : Form
    {
        private string userId;
        string constring = @"Data Source=localhost;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public sProfile(string id)
        {
            InitializeComponent();
            userId = id;
        }

        private void sProfile_Load(object sender, EventArgs e)
        {
            LoadUserData();

        }

        private void LoadUserData()
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "SELECT userid, firstname, lastname, email, password, dob FROM user_table WHERE userid = @userid";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userid", userId);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label2.Text = reader["userid"].ToString();
                            label12.Text = reader["firstname"].ToString();
                            label7.Text = reader["lastname"].ToString();
                            label9.Text = reader["email"].ToString();
                            label8.Text = reader["password"].ToString();
                            label6.Text = Convert.ToDateTime(reader["dob"]).ToString("dd/MM/yyyy");

                            label10.Text = "Hello " + reader["firstname"].ToString() + " " + reader["lastname"].ToString();

                        }
                    }
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                //for back jabe student page e
                student w = new student(userId);
                w.Show();
                this.Close();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                OnKeyDown(new KeyEventArgs(keyData));
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                 string.IsNullOrWhiteSpace(textBox3.Text) ||
                    string.IsNullOrWhiteSpace(dateTimePicker1.Text))
            {
                MessageBox.Show("Please fill in all fields before updating.");
                return; // stop here — don't touch the database
            }
            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = @"UPDATE user_table 
                     SET firstname = @firstname, 
                     lastname = @lastname, 
                     email = @email, 
                     dob = @dob 
                 WHERE userid = @userid";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@firstname", textBox1.Text);
                        cmd.Parameters.AddWithValue("@lastname", textBox2.Text);
                        cmd.Parameters.AddWithValue("@email", textBox3.Text);
                        cmd.Parameters.AddWithValue("@dob", dateTimePicker1.Value.Date);
                        cmd.Parameters.AddWithValue("@userid", userId);

                    try
                    {
                        { 
                            con.Open();
                            cmd.ExecuteNonQuery();
                        
                            MessageBox.Show("Profile updated successfully!");
                        }
                    }


                    catch (SqlException ex)
                    {
                        MessageBox.Show("Update failed: " + ex.Message);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please fill in all password fields.");
                return;
            }
            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = @"UPDATE user_table 
                          SET password = @password 
                          WHERE userid = @userid";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (textBox5.Text != textBox6.Text)
                    {
                        MessageBox.Show("Passwords does not match");
                        return;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@password", textBox5.Text);
                        cmd.Parameters.AddWithValue("@userid", userId);
                    }

                    try
                    {
                        { 
                            con.Open();
                            cmd.ExecuteNonQuery();
                        
                        
                            MessageBox.Show("Password updated successfully!");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Update failed: " + ex.Message);
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox5.UseSystemPasswordChar = false;  //design theke usesystem false korte hobe
                textBox6.UseSystemPasswordChar = false;  

            }
            else
            {
                textBox5.UseSystemPasswordChar = true; 
                textBox6.UseSystemPasswordChar = true; 
            }
        }
    }
}
