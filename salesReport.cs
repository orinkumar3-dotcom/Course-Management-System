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
    public partial class salesReport : Form
    {
    string constring = @"Data Source=localhost;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";
        private Form previousForm;
        public salesReport(Form previousForm)
        {
            InitializeComponent();
            this.previousForm = previousForm;
        }

        private void salesReport_Load(object sender, EventArgs e)
        {
            Loadreport();
        }
        private void Loadreport()
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                // ---- Total Revenue ----
                string revenueQuery = @"SELECT ISNULL(SUM(
                            co.courseprice - (co.courseprice * co.discountpercent / 100)
                        ), 0)
                        FROM cart_table c
                        JOIN course_table co ON c.courseid = co.courseid
                        WHERE c.status = 'Purchased'";
                SqlCommand revenueCmd = new SqlCommand(revenueQuery, con);
                decimal totalRevenue = Convert.ToDecimal(revenueCmd.ExecuteScalar());
                label3.Text = totalRevenue.ToString("N2");

                string soldQuery = "SELECT COUNT(*) FROM cart_table WHERE status = 'Purchased'";
                SqlCommand soldCmd = new SqlCommand(soldQuery, con);
                int coursesSold = (int)soldCmd.ExecuteScalar();
                label4.Text = coursesSold.ToString();

                string studentsQuery = "SELECT COUNT(DISTINCT userid) FROM cart_table WHERE status = 'Purchased'";
                SqlCommand studentsCmd = new SqlCommand(studentsQuery, con);
                int studentsCount = (int)studentsCmd.ExecuteScalar();
                label6.Text = studentsCount.ToString();

                string pendingQuery = "SELECT COUNT(*) FROM cart_table WHERE status = 'Pending'";
                SqlCommand pendingCmd = new SqlCommand(pendingQuery, con);
                int pendingCount = (int)pendingCmd.ExecuteScalar();
                label9.Text = pendingCount.ToString();

                string salesQuery = @"SELECT u.userid, (u.firstname + ' ' + u.lastname) AS Name,
             co.courseid, co.coursename, co.courseprice, co.discountpercent,
             CAST(co.courseprice - (co.courseprice * co.discountpercent / 100) AS DECIMAL(10,2)) AS Finalprice,
             c.date_added
                          FROM cart_table c
                          JOIN user_table u ON c.userid = u.userid
                          JOIN course_table co ON c.courseid = co.courseid
                          WHERE c.status = 'Purchased'
                          ORDER BY c.date_added DESC";

                SqlDataAdapter da = new SqlDataAdapter(salesQuery, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        protected override void OnKeyDown(KeyEventArgs T)
        {
            base.OnKeyDown(T);

            if (T.KeyCode == Keys.Escape)
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

    }
}
