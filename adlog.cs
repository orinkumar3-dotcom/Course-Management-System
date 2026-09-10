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
    public partial class adlog : Form
    {
        public adlog()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RequestCourses p = new RequestCourses( this); //go to admin work page. this bollam, dekhalam je o close. point korlam
            p.Show();
            this.Hide();

        }
    }
}
