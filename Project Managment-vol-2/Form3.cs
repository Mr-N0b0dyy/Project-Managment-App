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

namespace Project_Managment_vol_2
{
    public partial class frmLog : Form
    {

        frmReg frm;

        public frmLog(frmReg frmReg)
        {
            InitializeComponent();
            this.frm = frmReg;
        }

        public string role;

        private void bactolog_Click(object sender, EventArgs e)
        {
            new frmReg().Show();
            
        }

        private void log_Click(object sender, EventArgs e)
        {
            foreach (User i in frm.Users)
            {
                if(i.Email == logemil.Text && i.Password == logpas.Text)
                {
                    role = i.Role;
                }
            }

            if(role != null)
            {
                Main form = new Main(frm);
                form.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Invalid password or email, please try again", "Warning!");
            }
        }
    }
}
