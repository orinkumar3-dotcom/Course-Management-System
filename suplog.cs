using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TMs
{
    public partial class suplog : Form
    {
        public suplog()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            superadmin s= new superadmin(this);
            s.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            supstudentmang x = new supstudentmang(this);
            x.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            salesReport M= new salesReport(this);
            M.Show();
            this.Hide();
        }
    }
}
