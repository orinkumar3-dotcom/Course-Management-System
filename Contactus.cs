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
    public partial class Contactus : Form
    {
        public Contactus()
        {
            InitializeComponent();
        }

        private void Contactus_Load(object sender, EventArgs e)
        {

        }

        protected override void OnKeyDown(KeyEventArgs e) 
        {
            base.OnKeyDown(e); 

            if (e.KeyCode == Keys.Escape) 
            { this.Close(); }
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

        private void label1_Click(object sender, EventArgs e) 
        {
            Form1 frm= new Form1();
            
            frm.Show();
            this.Hide();
        }
    }
}
