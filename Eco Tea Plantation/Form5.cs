using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eco_Tea_Plantation
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
        public void loadform(object Form)
        {
            
            if (this.panelContainer.Controls.Count > 0) 
                this.panelContainer.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.panelContainer.Controls.Add(f);
            this.panelContainer.Tag = f;
            f.Show();


        }
        private void but1_Click(object sender, EventArgs e)
        {
            loadform(new onepanel());
           

        }

        private void but2_Click(object sender, EventArgs e)
        {
            loadform(new twopanel());
        }

        private void but3_Click(object sender, EventArgs e)
        {
            loadform(new threepanel());
        }

        private void but4_Click(object sender, EventArgs e)
        {
            loadform(new fourpanel());
        }

        private void but5_Click(object sender, EventArgs e)
        {
            loadform(new fivepanel());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToString("hh:mm:ss");
        }

        private void time_Click(object sender, EventArgs e)
        {
            time tm = new time();
            tm.Show();
        }
    }
}
