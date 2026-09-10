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

        protected override void OnKeyDown(KeyEventArgs e) // button = event. event handeller
        {
            base.OnKeyDown(e); // abstract class er method

            if (e.KeyCode == Keys.Escape) //condition, <- backspace press korle home button chole jabe
            { this.Close(); }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // <- backspace system e kaj koranor jonno but tao condition must lagbe
        {
            if (keyData == Keys.Escape) //condition
            {
                OnKeyDown(new KeyEventArgs(keyData)); //onkeydown jokhn call korbo (even handler) tokhn value pass kore diye dibo ref message er moddhe
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void label1_Click(object sender, EventArgs e) // contact page e home button click korle home e jabe. obj create korlam Form1 er
        {
            Form1 frm= new Form1();
            
            frm.Show();
            this.Hide();
        }
    }
}
