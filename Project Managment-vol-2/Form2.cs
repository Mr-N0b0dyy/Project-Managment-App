
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using Project_Managment_vol_2;

namespace Project_Managment_vol_2
{ 
    public partial class frmReg : Form
    {

        public frmReg()
        {
            InitializeComponent();
        }

        List<User> users = new List<User>();

        internal List<User> Users { get => users; set => users = value; }

        private void reg_Click(object sender, EventArgs e)
        {
            if (regkey.Text == "aziz")
            {
                User user = new User();

                user.Email = regemil.Text;
                user.Password = regpas.Text;
                user.Role = "Admin";

                Users.Add(user);

                frmLog form = new frmLog(this);
                form.Show();
                this.Hide();
                
            }

            else
            {
                MessageBox.Show("Please Enter Key", "Warning!");
            }
            

        }

        private void bactolog_Click(object sender, EventArgs e)
        {
            frmLog form = new frmLog(this);
            form.Show();
            this.Hide();

        }
    }


}
