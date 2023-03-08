
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
using Project_Managment_vol_2.Contexts;

namespace Project_Managment_vol_2
{ 
    public partial class frmReg : Form
    {
        
        public frmReg()
        {
            
            InitializeComponent();

        }

        frmLog form = null;

        public Boolean firstTime = false;

        

        

        private void reg_Click(object sender, EventArgs e)
        {
            if (regkey.Text == "IamAdmin")
            {
                User FindUser = Program.Codefirst.Users.FirstOrDefault(x => x.UserId == "0");
                if (FindUser != null)
                {
                    User temp = new User(null,null,null);
                
                    temp.Counter = FindUser.Counter-1;
                }
                User user = new User("Admin", regemil.Text, regpas.Text);
                

                
                Program.Codefirst.Users.Add(user);
                Program.Codefirst.SaveChanges();
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
            
            form = new frmLog(this);
            form.Show();
            this.Hide();
                

            
            

        }
    }


}
