using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Eco_Tea_Plantation
{
    public partial class onepanel : Form


    {
        
        public onepanel()
        {
            InitializeComponent();
        }
        public void loadform(object Form)
        {
            if (this.panel2.Controls.Count > 0)
                this.panel2.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(f);
            this.panel2.Tag = f;
            f.Show();


        }

        private void regibut1_Click(object sender, EventArgs e)
        {
            loadform(new employeeregi());
        }

        private void regibut1_Click_1(object sender, EventArgs e)
        {
            loadform(new employeeregi());
            //loadform(new employeregiphotos());
        }

        private void regibut2_Click(object sender, EventArgs e)
        {
            loadform(new supply());
        }

        private void regibut3_Click(object sender, EventArgs e)
        {
            loadform(new prouct());
        }

        private void regibut4_Click(object sender, EventArgs e)
        {
            loadform(new vehicle());
        }

        private void regibut5_Click(object sender, EventArgs e)
        {
            loadform(new printid());
        }
    }
}
