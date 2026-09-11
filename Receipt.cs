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
    public partial class Receipt : Form
    {
        private List<string> purchasedCartIds;
        private Form previousForm;
        string constring = @"Data Source=localhost;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";
        public Receipt(List<string> cartIds, Form callingForm)
        {
            InitializeComponent();
            purchasedCartIds = cartIds;
            previousForm = callingForm;
            LoadReceipt();
        }

        private void Receipt_Load(object sender, EventArgs e)
        {

        }

        private void LoadReceipt()
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

                dataGridView1.DataSource = dt;

                decimal total = 0;
                foreach (DataRow row in dt.Rows)
                {
                    total += Convert.ToDecimal(row["Discountprice"]);
                }
                label9.Text = total.ToString("N2");
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
