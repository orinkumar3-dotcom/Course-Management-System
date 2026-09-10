using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TMs
{
    public partial class StgetCourse : Form
    {
        private string loggedInUserId;
        private Form previousForm;     //NEW — holds a reference to whichever form opened this one
        string constring = @"Data Source=localhost;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";
        public StgetCourse(string userid, Form callingForm)
        {
            InitializeComponent();
            loggedInUserId = userid;
            previousForm = callingForm;
            //MessageBox.Show("Logged in as: [" + userid + "]");

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

                string query = @"SELECT courseid, coursename, coursetype, coursetime, courseprice, discountpercent,
                                 CAST(courseprice - (courseprice * discountpercent / 100) AS DECIMAL(10,2)) AS Finalprice
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

        private void StgetCourse_Load(object sender, EventArgs e)
        {
            LoadAvailableCourses();
            LoadCartedItems();
        }

        private void LoadAvailableCourses()
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

        private void LoadCartedItems()
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string query = @"SELECT ROW_NUMBER() OVER (PARTITION BY c.userid ORDER BY c.cartid) AS DisplayNo,
                                    c.cartid, c.courseid, co.coursename, co.courseprice,co.discountpercent,
                                    CAST(co.courseprice - (co.courseprice * co.discountpercent / 100) AS DECIMAL(10, 2)) AS Finalprice,
                                    c.status, c.date_added
                                FROM cart_table c
                                JOIN course_table co ON c.courseid = co.courseid
                                WHERE c.userid = @userid";


                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userid", loggedInUserId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadAvailableCourses();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a course from Available Courses first.");
                return;
            }

            string courseId = dataGridView1.SelectedRows[0].Cells["courseid"].Value.ToString();

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string checkQuery = @"SELECT COUNT(*) FROM cart_table
                               WHERE userid = @userid AND courseid = @courseid 
                               AND status IN ('Pending', 'Approved', 'Purchased')";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@userid", loggedInUserId);
                checkCmd.Parameters.AddWithValue("@courseid", courseId);

                int exists = (int)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    MessageBox.Show("You have already purchased, requested, or been approved for this course.");
                    return;
                }

                //int newCartId = GetNextCartId(con); // if you're still using the reuse-ID version — otherwise remove this line

                string insertQuery = @"INSERT INTO cart_table (userid, courseid, status, date_added)
                                VALUES (@userid, @courseid, 'Pending', GETDATE())";

                SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                insertCmd.Parameters.AddWithValue("@userid", loggedInUserId);
                insertCmd.Parameters.AddWithValue("@courseid", courseId);

                insertCmd.ExecuteNonQuery();

                MessageBox.Show("Course added to cart.");
                LoadCartedItems();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a course from your Carted Item list first.");
                return;
            }

            string cartId = dataGridView2.SelectedRows[0].Cells["cartid"].Value.ToString();

            DialogResult result = MessageBox.Show(
                "Are you sure you want to cancel this order?",
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

                string query = "DELETE FROM cart_table WHERE cartid = @cartid AND userid = @userid";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cartid", cartId);
                cmd.Parameters.AddWithValue("@userid", loggedInUserId);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Order cancelled.");
                }
                else
                {
                    MessageBox.Show("Could not cancel this order.");
                }

                LoadCartedItems();
            }
        }

        private void button8_Click(object sender, EventArgs e)
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
        protected override void OnKeyDown(KeyEventArgs t)
        {
            base.OnKeyDown(t);

            if (t.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.Escape)
            {
                if (previousForm != null)
                {
                    previousForm.Show();
                }
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a course to buy.");
                return;
            }

            string status = dataGridView2.SelectedRows[0].Cells["status"].Value.ToString();

            if (status == "Rejected")
            {
                MessageBox.Show("This request was rejected and cannot be purchased.");
                return;
            }

            if (status == "Pending")
            {
                MessageBox.Show("This course is still awaiting approval and cannot be purchased yet.");
                return;
            }

            if (status == "Purchased")
            {
                MessageBox.Show("You already purchased this course.");
                return;
            }

            if (status != "Approved")
            {
                MessageBox.Show("This course cannot be purchased right now.");
                return;
            }

            string cartId = dataGridView2.SelectedRows[0].Cells["cartid"].Value.ToString();

            DialogResult result = MessageBox.Show(
                "Confirm purchase of this course?",
                "Confirm Purchase",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string query = "UPDATE cart_table SET status = 'Purchased' " +
                    "WHERE cartid = @cartid AND userid = @userid AND status = 'Approved'";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cartid", cartId);
                cmd.Parameters.AddWithValue("@userid", loggedInUserId);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Purchase successful!");

                    LoadCartedItems();

                    Receipt E = new Receipt(new List<string> { cartId }, this);
                    E.Show();
                    this.Hide();
                    //return; // skip the LoadCartedItems() below since we already did it
                }
                else
                {
                    MessageBox.Show("Purchase failed. This course may no longer be approved.");
                }

                LoadCartedItems();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                // First, get the actual cartids that are Approved (so we know what to show on the receipt)
                string idsQuery = "SELECT cartid FROM cart_table WHERE userid = @userid AND status = 'Approved'";
                SqlCommand idsCmd = new SqlCommand(idsQuery, con);
                idsCmd.Parameters.AddWithValue("@userid", loggedInUserId);

                List<string> approvedIds = new List<string>();
                using (SqlDataReader reader = idsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        approvedIds.Add(reader["cartid"].ToString());
                    }
                }

                if (approvedIds.Count == 0)
                {
                    MessageBox.Show("You have no approved courses to buy.");
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Buy all {approvedIds.Count} approved course(s)?",
                    "Confirm Purchase",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }

                string query = "UPDATE cart_table SET status = 'Purchased' WHERE userid = @userid AND status = 'Approved'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userid", loggedInUserId);

                int rows = cmd.ExecuteNonQuery();

                MessageBox.Show(rows + " course(s) purchased successfully.");

                LoadCartedItems();
                Receipt E = new Receipt(approvedIds, this);
                E.Show();
                this.Hide();

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            History H= new History(loggedInUserId, this, true);
            H.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one course to view its receipt.");
                return;
            }

            List<string> cartIds = new List<string>();

            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                string status = row.Cells["status"].Value.ToString();

                if (status != "Purchased")
                {
                    MessageBox.Show("Only purchased courses have a receipt. Please select only purchased courses.");
                    return;
                }

                cartIds.Add(row.Cells["cartid"].Value.ToString());
            }

            Receipt r = new Receipt(cartIds, this);
            r.Show();
            this.Hide();
        }

    }

}
