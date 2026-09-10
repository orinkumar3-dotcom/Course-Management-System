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
    public partial class History : Form
    {
        private List<string> purchasedCartIds;
        private Form previousForm;
        string constring = @"Data Source=localhost;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        // Constructor 1 — single-transaction receipt
        public History(List<string> cartIds, Form callingForm)
        {
            InitializeComponent();
            this.previousForm = callingForm;
            LoadHistory();
        }

        // Constructor 2 — full purchase history
        public History(string userid, Form callingForm, bool isHistoryView)
        {
            InitializeComponent();
            previousForm = callingForm;

            if (isHistoryView)
            {
                LoadPurchaseHistory(userid);
            }
        }

        private void Receipt_Load(object sender, EventArgs e)
        {
            
        }

        private void LoadHistory()
        {
            if (purchasedCartIds == null || purchasedCartIds.Count == 0)
            {
                MessageBox.Show("No purchase details to show.");
                return;
            }

            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                List<string> paramNames = new List<string>();
                SqlCommand cmd = new SqlCommand();

                for (int i = 0; i < purchasedCartIds.Count; i++)
                {
                    string paramName = "@id" + i;
                    paramNames.Add(paramName);
                    cmd.Parameters.AddWithValue(paramName, purchasedCartIds[i]);
                }

                string query = $@"SELECT u.userid, u.firstname, u.lastname, u.email,
                                  co.courseid, co.coursename, co.courseprice, co.discountpercent,
                                  CAST(co.courseprice - (co.courseprice * co.discountpercent / 100) AS DECIMAL(10,2)) AS Discountprice,
                                  c.status
                           FROM cart_table c
                           JOIN user_table u ON c.userid = u.userid
                           JOIN course_table co ON c.courseid = co.courseid
                           WHERE c.cartid IN ({string.Join(",", paramNames)})";

                cmd.CommandText = query;
                cmd.Connection = con;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Fill the top labels from the first row (same student on every row anyway)
                if (dt.Rows.Count > 0)
                {
                    label4.Text = dt.Rows[0]["userid"].ToString();
                    label6.Text = dt.Rows[0]["firstname"] + " " + dt.Rows[0]["lastname"];
                    label7.Text = dt.Rows[0]["email"].ToString();
                }

                // Remove student columns so the grid only shows course details
                dt.Columns.Remove("userid");
                dt.Columns.Remove("firstname");
                dt.Columns.Remove("lastname");
                dt.Columns.Remove("email");
                dt.Columns.Remove("status");

                dataGridView1.DataSource = dt;

                decimal total = 0;
                foreach (DataRow row in dt.Rows)
                {
                    total += Convert.ToDecimal(row["Discountprice"]);
                }
                label9.Text = total.ToString("N2")+" Tk";
            }
        }

        private void LoadPurchaseHistory(string userid)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();

                string query = @"SELECT u.userid, u.firstname, u.lastname, u.email,
                         co.courseid, co.coursename, co.courseprice, co.discountpercent,
                         CAST(co.courseprice - (co.courseprice * co.discountpercent / 100) AS DECIMAL(10,2)) AS discountprice,
                         c.status, c.date_added
                  FROM cart_table c
                  JOIN user_table u ON c.userid = u.userid
                  JOIN course_table co ON c.courseid = co.courseid
                  WHERE c.userid = @userid AND c.status = 'Purchased'
                  ORDER BY c.date_added DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userid", userid);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    label4.Text = dt.Rows[0]["userid"].ToString();
                    label6.Text = dt.Rows[0]["firstname"] + " " + dt.Rows[0]["lastname"];
                    label7.Text = dt.Rows[0]["email"].ToString();
                }

                dt.Columns.Remove("userid");
                dt.Columns.Remove("firstname");
                dt.Columns.Remove("lastname");
                dt.Columns.Remove("email");
                dt.Columns.Remove("status");
                dt.Columns.Remove("date_added");

                dataGridView1.DataSource = dt;

                decimal total = 0;
                foreach (DataRow row in dt.Rows)
                {
                    total += Convert.ToDecimal(row["discountprice"]); // lowercase — matches this query's alias
                }
                label9.Text = total.ToString("N2")+" Tk";
            }
        }

        protected override void OnKeyDown(KeyEventArgs D)
        {
            base.OnKeyDown(D);

            if (D.KeyCode == Keys.Escape)
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
