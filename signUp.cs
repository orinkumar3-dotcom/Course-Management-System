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
    public partial class signUp : Form
    {
        public signUp()
        {
            InitializeComponent();
        }

        //select image
        private void button5_Click(object sender, EventArgs e)
        {
            //OpenFileDialog ofd = new OpenFileDialog();  //object create korlam select image er jonno
            //ofd.Filter = "Image Files |*.jpg;*.jpeg;*.png;*.svg;*.bmp;|All File|*.*"; // image upload er jonno, (*) diye suru (*) diye ses

            //if (ofd.ShowDialog() == DialogResult.OK)
            //{     // je image dibo oita show kore kina er jonno condition

            //    string selectedFilePath = ofd.FileName;    //je file select korbo oita chole ashbe
            //    PictureBox pic = new PictureBox();           // file duka show korar jonno picturebox obj korlam 
            //    pic.Image = Image.FromFile(selectedFilePath);      //them pic pabo kotha theke (selectedpathfile). ta diye dilam 

            //}
        }
        // <- backspace for signUp button
        protected override void OnKeyDown(KeyEventArgs e) // button = event. event handeller
        {
            base.OnKeyDown(e); // abstract class er method

            if (e.KeyCode == Keys.Escape) //condition, <- backspace press korle singUp button chole jabe
            { this.Close(); }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // <- backspace system e kaj koranor jonno but tao condition must lagbe
        {
            if (keyData == Keys.Escape) //condition
            {
                OnKeyDown(new KeyEventArgs(keyData)); //onkeydown jokhn call korbo (event handler) tokhn value pass kore diye dibo ref message er moddhe
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) //for signUp page show pass
        {
            if (checkBox2.Checked)
            {
                textBox3.UseSystemPasswordChar = false;
                textBox4.UseSystemPasswordChar = false;
            }
            else
            {
                textBox3.UseSystemPasswordChar = true;
                textBox4.UseSystemPasswordChar = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            this.Hide();
            frm.Show();
            this.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Contactus cn = new Contactus();
            this.Hide();
            cn.Show();
            this.Show();
        }
        //jeno shob info sothik input ney, khali jeno na thake
        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox5.Text) == true)
            {
                textBox5.Focus(); //focus rakhtesi khali kina.
                errorProvider1.SetError(this.textBox5, "Please Fill First Name"); //errorprovide set kore this. diye locate korlam then message show korlam
            }

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text) == true)
            {
                textBox1.Focus();
                errorProvider2.SetError(this.textBox1, "Please Fill Last Name");
            }

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox2.Text) == true)
            {
                textBox2.Focus();
                errorProvider3.SetError(this.textBox2, "Please Fill Email");
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox3.Text) == true)
            {
                textBox3.Focus();
                errorProvider4.SetError(this.textBox3, "Please Fill Password");
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox4.Text) == true)
            {
                textBox4.Focus();
                errorProvider5.SetError(this.textBox4, "Please Fill Confirm Password");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //clear error providers before validation
            errorProvider1.Clear(); 
            errorProvider2.Clear(); 
            errorProvider3.Clear(); 
            errorProvider4.Clear(); 
            errorProvider5.Clear();

            if (string.IsNullOrEmpty(textBox6.Text) == true)
            {
                textBox6.Focus();
                errorProvider6.SetError(this.textBox6, "Please Fill User ID");
                return;
            }

            else if (String.IsNullOrEmpty(textBox5.Text) == true)
            {
                textBox5.Focus(); //focus rakhtesi khali kina.
                errorProvider1.SetError(this.textBox5, "Please Fill First Name"); //errorprovide set kore this. diye locate korlam then message show korlam
                return; //stop further execution if validation fails
            }

            else if (String.IsNullOrEmpty(textBox1.Text) == true)
            {
                textBox1.Focus();
                errorProvider2.SetError(this.textBox1, "Please Fill Last Name");
                return;
            }

            else if (String.IsNullOrEmpty(textBox2.Text) == true)
            {
                textBox2.Focus();
                errorProvider3.SetError(this.textBox2, "Please Fill Email");
                return;
            }
            else if (String.IsNullOrEmpty(textBox3.Text) == true)
            {
                textBox3.Focus();
                errorProvider4.SetError(this.textBox3, "Please Fill Password");
                return;
            }

            else if (String.IsNullOrEmpty(textBox4.Text) == true)
            {
                textBox4.Focus();
                errorProvider5.SetError(this.textBox4, "Please Fill Confirm Password");
                return;
            }

            else if (textBox3.Text != textBox4.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            string constring = "Data Source=localhost;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(constring))
            {
                string query = @"INSERT INTO user_table
                         (userid, firstname, lastname, email, password, role,dob)
                         VALUES
                         (@userid, @firstname, @lastname, @email,@password, @role, @dob)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userid", textBox6.Text);
                    cmd.Parameters.AddWithValue("@firstname", textBox5.Text);
                    cmd.Parameters.AddWithValue("@lastname", textBox1.Text);
                    cmd.Parameters.AddWithValue("@email", textBox2.Text);
                    cmd.Parameters.AddWithValue("@password", textBox3.Text);
                    cmd.Parameters.AddWithValue("@dob", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@role", "Student");

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Registration Successful!");

                        // Clear fields
                        textBox6.Clear();
                        textBox5.Clear();
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();

                        // direct goto login page after successful registration
                        Form1 frm = new Form1();
                        frm.Show();
                        this.Close();   
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627 || ex.Number == 2601)
                        {
                            MessageBox.Show("This User ID already exists.");
                        }
                        else
                        {
                            MessageBox.Show("Database Error: " + ex.Message);
                        }
                    }
                }
            }

        }

        private void textBox6_MouseLeave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox6.Text)== true)
            {
                textBox6.Focus();
                errorProvider6.SetError(this.textBox6, "Please Fill User ID");
            }
        }

        private void signUp_Load(object sender, EventArgs e)
        {

        }
    }
}
