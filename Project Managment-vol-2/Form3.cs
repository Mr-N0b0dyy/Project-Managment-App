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
        public frmLog()
        {
            InitializeComponent();
        }

        private void bactolog_Click(object sender, EventArgs e)
        {
            new frmReg().Show();
            this.Hide();
        }

        private void log_Click(object sender, EventArgs e)
        {

        }
    }
}
