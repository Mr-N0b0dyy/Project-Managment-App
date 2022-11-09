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
    public partial class frmReg : Form
    {
        
        List<Admin> AdminList = new List<Admin>();
        List<Employee> EmployeeList = new List<Employee>();

        
        public frmReg()
        {
            InitializeComponent();

        }

        private void reg_Click(object sender, EventArgs e)
        {

            if (regkey.Text == "aziz")
            {
                Admin admin = new Admin();

                admin.Email = regemil.Text;
                admin.Password = regpas.Text;

                if (!AdminList.Contains(admin))
                {
                    AdminList.Add(admin);
                    new frmLog().Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("User Allready Exits", "Warning!");
                }

            }
            else
            {
                MessageBox.Show("Please Enter Key", "Warning!");
            }
            

        }

        private void bactolog_Click(object sender, EventArgs e)
        {
            new frmLog().Show();
            this.Hide();
        }

        private void frmReg_Load(object sender, EventArgs e)
        {
           
        }
    }
}
