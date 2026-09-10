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
    public partial class student : Form
    {
        private string userId;
        public student(String id)
        {
            InitializeComponent();
            userId = id;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StgetCourse z= new StgetCourse(userId,this); // use (this) for point this form is closed.
            z.Show();
            this.Hide();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sProfile ef= new sProfile(userId);
            ef.Show();
            this.Hide();
        }
    }
}
