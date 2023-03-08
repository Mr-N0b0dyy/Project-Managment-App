using DevExpress.XtraEditors.ViewInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using Project_Managment_vol_2.Contexts;

namespace Project_Managment_vol_2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        public static CodeFirstContext Codefirst { get; set; }
        [STAThread]

        static void Main()
        {
            Codefirst = new CodeFirstContext();
            Codefirst.projects.ToList();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmReg());
            
        }
    }
}
