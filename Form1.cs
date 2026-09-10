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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            //Form2 frm2 = new Form2();  // call object for home
            //this.Hide();              // home click korle,new form open hobe ager form hide hobe
            //frm2.ShowDialog();
            //this.Show();              // home form cancel korle ager form e fire jabe
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        //sign UP 
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            signUp sp = new signUp();    // signUp clik korle page open hobe, ejonno signup obj nilam
            sp.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //panel3.Visible=!panel3.Visible;  //courses click korle visible hobe na korle hobe na
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {

                textBox2.UseSystemPasswordChar = false;  //check korle false hobe
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;  //uncheck korle ture hobe
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Contactus cn = new Contactus();
            cn.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //RequestCourses re = new RequestCourses();
            //this.Hide();
            //re.Show();
            //this.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string constring = "Data Source=localhost;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = "SELECT userid, password, role FROM user_table " +
                               "WHERE userid = @userid AND password = @password";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userid", textBox1.Text);
                    cmd.Parameters.AddWithValue("@password", textBox2.Text);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string role = reader["role"].ToString();

                            MessageBox.Show("Login Successful!\nRole: " + role);

                            if (role == "Super Admin")
                            {
                                // Open Super Admin dashboard
                                suplog h = new suplog();

                                h.FormClosed += (s, args) =>
                                {
                                    Application.Exit();
                                };

                                h.Show();
                                this.Hide();
                            }
                            else if (role == "Admin")
                            {
                                // Open Admin dashboard
                                adlog a = new adlog();

                                a.FormClosed += (s, args) =>
                                {
                                    Application.Exit();
                                };

                                a.Show();
                                this.Hide();
                            }
                            else if (role == "Student")
                            {
                                // Open Student dashboard
                                student n = new student(textBox1.Text); //id store korlam.

                                n.FormClosed += (s, args) =>
                                {
                                    Application.Exit();
                                };

                                n.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Unknown user role.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid User ID or Password.");
                        }
                    }
                }
            }
        }
    } // end class Form1
} // end namespace TMs

