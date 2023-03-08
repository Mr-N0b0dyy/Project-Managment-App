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
using Project_Managment_vol_2.Contexts;

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
        Main form = null;
        public string role;
        public Boolean isLogOut = false;

        private void bactolog_Click(object sender, EventArgs e)
        {
            frm = new frmReg();
            frm.Show();
            this.Hide();
        }

        private void log_Click(object sender, EventArgs e)
        {
            foreach (var i in Program.Codefirst.Users)
            {
                if(i.Email == logemil.Text && i.Password == logpas.Text)
                {
                    role = i.Role;
                }
            }

            if(role != null)
            {   
                if(isLogOut == false)
                {
                    form = new Main(frm, this);
                    form.Show();
                    this.Hide();
                } 
                else if(isLogOut == true)
                {
                    form = new Main(frm, this);
                    form.Show();
                    this.Hide();
                }
            } 
            
                        
            else
            {
                MessageBox.Show("Invalid password or email, please try again", "Warning!");
            }
        }
    }
}
