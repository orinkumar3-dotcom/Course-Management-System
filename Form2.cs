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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        //korlam jate <- backspace button kaj kore press korle
        protected override void OnKeyDown(KeyEventArgs e) // button = event. event handeller
        {
            base.OnKeyDown(e); // abstract class er method

            if (e.KeyCode == Keys.Escape) //condition, <- backspace press korle home button chole jabe
            { this.Close();}      
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

        private void button4_Click(object sender, EventArgs e)
        {
            panel3.Visible = !panel3.Visible;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel7.Visible = !panel7.Visible;
        }
    }
}
