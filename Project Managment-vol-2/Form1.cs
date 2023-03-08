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
using DevExpress.XtraTab;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;
using DevExpress.XtraEditors;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net;
using System.Net.Mail;
using DevExpress.Utils.DPI;
using TreeView = System.Windows.Forms.TreeView;
using System.Security;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DevExpress.XtraRichEdit.Forms;
using DevExpress.Utils;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ConsoleTables;
using System.Collections;
using System.CodeDom.Compiler;

namespace Project_Managment_vol_2
{

    public partial class Main : Form
    {

        frmReg frm;
        frmLog frm2;
        SmtpClient smtp;

        public Main(frmReg frmReg, frmLog frmLog)
        {
            InitializeComponent();
            this.frm = frmReg;
            this.frm2 = frmLog;
            this.peditdesc.SelectedTabPage = this.protasktab;
            setCounters();

            UpdateProject();
            Updateemp();
            getItems();
            refreshEmpgrid();
            cleanTree();
            initializeTree();
            var source = new BindingSource(emp, null);
            Empgrid.DataSource = source;
            Empgrid.AutoResizeColumns();
            Empgrid.Columns[9].Visible = false;
            Empgrid.Columns[11].Visible = false;
            Empgrid.Columns[12].Visible = false;

            smtp = new SmtpClient();
            smtp.Host = "smtp.office365.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("gtafurkan@outlook.com", "Please write a pasword before running");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            //Useless code Just for demonstration
            List<int> list = new List<int> { 1, 2, 3, 4 };
            var result = list.Select((x, i) => i < list.Count - 1 ? x * list[i + 1] : 0);
            foreach(var r in result)
            {
                Console.WriteLine(r);
            }
        }



        BindingList<Employee> emp = new BindingList<Employee>();
        internal BindingList<Employee> Emp { get => emp; set => emp = value; }
        public static DateTime MaxDT = new DateTime(2005, 01, 01, 00, 00, 00);
        Employee selectedEmp;
        List<Project> projects = new List<Project>();
        Project selecPro;
        MileStone selecMil;
        Task selecTask;


        private void setCounters()
        {

            Employee Findemp = Program.Codefirst.Employees.FirstOrDefault(x => x.Id == "0");
            if (Findemp != null)
            {
                Employee temp = new Employee(null, null, null, DateTime.Now, null, null, null, null, null);

                temp.Counter = Findemp.Counter - 1;
            }
            Project FindPro = Program.Codefirst.projects.FirstOrDefault(x => x.PrjNo == "PRJ0");
            if (FindPro != null)
            {
                Project temp1 = new Project(null, null, null, DateTime.Now, DateTime.Now, 0, null, null);
                temp1.Count = FindPro.Count - 1;
            }
            MileStone FindMil = Program.Codefirst.MileStones.FirstOrDefault(x => x.MileId == "0");
            if (FindMil != null)
            {
                MileStone temp2 = new MileStone(null, DateTime.Now, DateTime.Now, null);
                temp2.CountM = FindMil.CountM - 1;
            }

            Task FindTas = Program.Codefirst.tasks.FirstOrDefault(x => x.TaskId == "0");
            if (FindTas != null)
            {
                Task temp3 = new Task(null, null, null);
                temp3.CounT = FindTas.CounT - 1;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            peditdesc.Visible = false;
        }

        private void TabClick(object sender, MouseEventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;

            switch (btn.Name)
            {
                case "Pro":


                    this.peditdesc.SelectedTabPage = this.protab;
                    ProSub.Visible = true;
                    ProUpt.Visible = false;
                    ProDel.Visible = false;
                    milesub.Visible = false;
                    miledel.Visible = false;
                    mileupt.Visible = false;
                    tasub.Visible = false;
                    tasup.Visible = false;
                    tasdel.Visible = false;
                    CompCheck.Visible = false;
                    break;



                case "ProTask":
                    this.peditdesc.SelectedTabPage = this.protasktab;

                    break;

                case "Empl":
                    this.peditdesc.SelectedTabPage = this.emptab;
                    break;



            }
        }
        private void initializeTree()
        {

            int counter = 0;
            foreach (Project P in projects)
            {
                int counter2 = 2;
                protree.Nodes.Add("P " + P.PrjNo + " " + P.PjName);
                if (P.PjStatus == "Completed")
                {
                    protree.Nodes[counter].ForeColor = Color.Green;
                }
                if (P.ProbDatEnd < DateTime.Now && P.PjStatus != "Completed")
                {
                    P.PjStatus = "Delayed";
                    protree.Nodes[counter].ForeColor = Color.Red;
                }
                string team = " ";


                foreach (Employee a in P.Team1)
                {
                    team = team + " | " + a.FirstName + " " + a.MiddleName + " " + a.LastName;
                }
                protree.Nodes[counter].Nodes.Add("Status: " + P.PjStatus + " | Manager: " + P.PjManager.FirstName + " " + P.PjManager.MiddleName + " " + P.PjManager.LastName + " | Registaration Date: " + P.DatReg1 + " | Start Date: " + P.ProbDatStart + " | End Date: " + P.ProbDatEnd + " | Moneytery Return: " + P.Monret + " | Money Type: " + P.Monrettype + " | Project Team: " + team);
                protree.Nodes[counter].Nodes.Add("Description: " + P.Description);
                foreach (MileStone M in P.Milestones1)
                {
                    int counter3 = 1;
                    protree.Nodes[counter].Nodes.Add("M " + M.MileId + " " + M.Name);

                    if (M.Status == "Completed")
                    {
                        protree.Nodes[counter].Nodes[counter2].ForeColor = Color.Green;
                    }
                    if (M.EndDate < DateTime.Now && M.Status != "Completed")
                    {
                        M.Status = "Delayed";
                        protree.Nodes[counter].Nodes[counter2].ForeColor = Color.Red;
                    }
                    protree.Nodes[counter].Nodes[counter2].Nodes.Add("Status: " + M.Status + " | Start Date: " + M.StartDate + " | Exp. EndDate:" + M.EndDate);
                    foreach (Task T in M.Tasks)
                    {
                        Employee Assigned = null;
                        foreach (Employee i in emp)
                        {
                            if (T.AssignedTo == i.Id)
                            {
                                Assigned = i;
                            }
                        }

                        protree.Nodes[counter].Nodes[counter2].Nodes.Add("T " + T.TaskId + " " + T.Name);
                        if (T.Status == "Completed")
                        {
                            protree.Nodes[counter].Nodes[counter2].Nodes[counter3].ForeColor = Color.Green;
                        }
                        protree.Nodes[counter].Nodes[counter2].Nodes[counter3].Nodes.Add("Status: " + T.Status + " | AssignedTo: " + Assigned.FirstName + " " + Assigned.MiddleName + " " + Assigned.LastName);
                        counter3++;
                    }
                    counter2++;
                }
                counter++;

                protree.Refresh();
            }
        }
        private void cleanTree()
        {
            protree.Nodes.Clear();
            protree.Refresh();

        }

        private void delItems()
        {
            foreach (Employee i in emp)
            {
                Teambox.Items.Clear();
                Manager.Items.Clear();
                Assigned.Items.Clear();
            }
        }
        private void getItems()
        {
            foreach (Employee i in emp)
            {
                Teambox.Items.Add(i.Id + " " + i.FirstName + " " + i.MiddleName + " " + i.LastName);
                Manager.Items.Add(i.Id + " " + i.FirstName + " " + i.MiddleName + " " + i.LastName);
                Assigned.Items.Add(i.Id + " " + i.FirstName + " " + i.MiddleName + " " + i.LastName);
            }
        }

        private void refreshEmpgrid()
        {
            Empgrid.DataSource = emp;
            Empgrid.AutoResizeColumns();
            Empgrid.Refresh();
        }
        private void Updateemp()
        {

            foreach (var e in Program.Codefirst.Employees.ToList())
            {
                Employee New = new Employee(e.FirstName, e.MiddleName, e.LastName, e.Birthday, e.Email, e.Phone, e.Address1, e.Address2, e.Password);
                New.Id = e.Id;
                New.Counter = New.Counter - 1;
                New.IsManager = e.IsManager;
                foreach (Project p in projects)
                {
                    if (e.PId == p.PrjNo)
                    {
                        New.PId = p.PrjNo;
                    }
                }
                emp.Add(New);

            }
        }

        private void UpdateProject()
        {

            foreach (var p in Program.Codefirst.projects.ToList())
            {
                IQueryable<Employee> statement = Program.Codefirst.Employees.Where(e => e.PId == p.PrjNo).OrderBy(e => e.Id);
                List<Employee> employees = statement.ToList();

                Project project = new Project(p.PjName, p.PjManager, p.Description, p.ProbDatStart, p.ProbDatEnd, p.Monret, p.Monrettype, employees);

                project.PjStatus = p.PjStatus;
                project.PrjNo = p.PrjNo;
                project.Count = project.Count - 1;
                foreach (Employee e in project.Team1)
                {
                    e.PId = project.PrjNo;
                }
                project.Milestones1 = UpdateMilestone(project);
                projects.Add(project);

            }
        }
        private List<MileStone> UpdateMilestone(Project pId)
        {

            List<MileStone> MStones = new List<MileStone>();
            List<MileStone> NewStones = new List<MileStone>();
            IQueryable<MileStone> statement = Program.Codefirst.MileStones.Where(m => m.PId == pId.PrjNo).OrderBy(m => m.Name).ThenBy(m => m.MileId);
            MStones = statement.ToList();
            foreach (var mil in MStones)
            {
                MileStone m = new MileStone(mil.Name, mil.StartDate, mil.EndDate, pId.PrjNo);
                m.Status = mil.Status;
                m.MileId = mil.MileId;
                m.CountM = m.CountM - 1;

                m.Tasks = UpdateTask(pId, mil);
                NewStones.Add(m);

            }
            return NewStones;
        }
        private List<Task> UpdateTask(Project Pıd, MileStone mId)
        {
            List<Task> tasks = new List<Task>();
            List<Task> newTasks = new List<Task>();
            IQueryable<Task> statement = Program.Codefirst.tasks.Where(t => t.MId == mId.MileId).OrderBy(t => t.Name).ThenBy(t => t.TaskId);
            tasks = statement.ToList();

            foreach (var task in tasks)
            {
                Employee assignedEmp = null;
                foreach (Employee emp in Pıd.Team1)
                {
                    if (task.AssignedTo == emp.Id)
                    {
                        assignedEmp = emp;
                    }
                }
                Task tas = new Task(task.Name, assignedEmp.Id, mId.MileId);
                tas.Status = task.Status;
                tas.TaskId = task.TaskId;
                tas.CounT = task.CounT - 1;
                newTasks.Add(tas);

            }
            return newTasks;
        }


        private void EmpSubmit_Click(object sender, EventArgs e)
        {

            if (Email.Text == " " || Nametxt.Text == " " || Mname.Text == " " || Lname.Text == " " || Birth.Value >= MaxDT || Add1.Text == " " || Add2.Text == " " || Pass.Text == " ")
            {
                MessageBox.Show("Please fill all the fields correctly before submition!", "Warning");
            }
            else
            {

                Employee employee = new Employee(Nametxt.Text, Mname.Text, Lname.Text, Birth.Value, Email.Text, Pnumber.Text, Add1.Text, Add2.Text, Pass.Text);


                emp.Add(employee);




                delItems();
                getItems();
                refreshEmpgrid();
                Nametxt.Text = " ";
                Mname.Text = " ";
                Lname.Text = " ";
                Lname.Text = " ";

                Add1.Text = " ";
                Add2.Text = " ";
                Pass.Text = " ";
                Birth.Value = MaxDT;
                Email.Text = " ";
                Pnumber.Text = " ";

            }
        }



        private void FullName(object sender, EventArgs e)
        {
            Fname.Text = Nametxt.Text + " " + Mname.Text + " " + Lname.Text;
        }

        private void Empgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {

                DataGridViewRow row = Empgrid.Rows[e.RowIndex];
                foreach (Employee i in emp)
                {

                    if (i.Id.ToString().Equals(row.Cells[0].Value.ToString()))
                    {

                        selectedEmp = i;
                        break;
                    }
                }
                Nametxt.Text = row.Cells[1].Value.ToString();
                Mname.Text = row.Cells[2].Value.ToString();
                Lname.Text = row.Cells[3].Value.ToString();
                Birth.Value = Convert.ToDateTime(row.Cells[4].Value.ToString());
                Email.Text = row.Cells[5].Value.ToString();
                Pnumber.Text = row.Cells[6].Value.ToString();
                Add1.Text = row.Cells[7].Value.ToString();
                Add2.Text = row.Cells[8].Value.ToString();
                Pass.Text = row.Cells[10].Value.ToString();

            }
            else { };
        }

        private void EmpUpt_Click(object sender, EventArgs e)
        {
            if (selectedEmp != null)
            {
                if ((Email.Text == " ") || (Nametxt.Text == " ") || (Mname.Text == " ") || (Lname.Text == " ") || Birth.Value >= MaxDT || (Add1.Text == " ") || (Add2.Text == " ") || (Pass.Text == " "))
                {
                    MessageBox.Show("Please fill all the fields correctly before submition!", "Warning");
                }
                else
                {


                    selectedEmp.FirstName = Nametxt.Text;
                    selectedEmp.MiddleName = Mname.Text;
                    selectedEmp.LastName = Lname.Text;
                    selectedEmp.Email = Email.Text;
                    selectedEmp.Birthday = Birth.Value;
                    selectedEmp.Address1 = Add1.Text;
                    selectedEmp.Address2 = Add2.Text;
                    selectedEmp.Password = Pass.Text;

                    foreach (Project p in projects)
                    {
                        if (selectedEmp.PId == p.PrjNo)
                        {

                            foreach (Employee a in p.Team1)
                            {
                                if (selectedEmp.Id == a.Id)
                                {
                                    p.Team1.Remove(a);
                                    p.Team1.Add(selectedEmp);
                                    p.Team1 = p.Team1.OrderBy(x => x.Id).ToList();
                                }
                            }
                        }
                    }


                    delItems();
                    getItems();
                    refreshEmpgrid();

                }
            }
            else { MessageBox.Show("Please select an employee from grid first!", "Warning"); }
        }

        private void EmpDel_Click(object sender, EventArgs e)
        {
            if (selectedEmp != null)
            {

                foreach (Project p in projects)
                {
                    if (selectedEmp.PId == p.PrjNo)
                    {
                        if (p.PjManager == selectedEmp || p.Team1.Count() == 1)
                        {
                            MessageBox.Show("This employee is crutial and cannot be deleted!", "Warning");
                        }
                        else
                        {
                            emp.Remove(selectedEmp);
                            foreach (Employee a in p.Team1)
                            {
                                if (selectedEmp.Id == a.Id)
                                {
                                    p.Team1.Remove(a);

                                }
                            }


                        }
                    }
                }
                if (selectedEmp.PId == null)
                {
                    emp.Remove(selectedEmp);

                }



                delItems();
                getItems();
                refreshEmpgrid();
            }
            else { MessageBox.Show("Please select an employee from grid first!", "Warning"); }
        }
        private Boolean isStillManger(Employee man)
        {
            Boolean manager = false;
            foreach (Project P in projects)
            {
                if (P.PjManager.Id == man.Id)
                {
                    manager = true; break;
                }

            }
            return manager;

        }

        private void clearFields()
        {
            Proname.Text = " "; Prodes.Text = " "; MoneyRet.Value = 0; taskexp.Text = " "; Manager.SelectedIndex = -1; Manager.SelectedItem = null; Assigned.SelectedIndex = -1; Assigned.SelectedItem = null; Teambox.ClearSelected(); mileexp.Text = " "; taskexp.Text = " ";
        }
        private void ProSub_Click(object sender, EventArgs e)
        {
            bool cancel = false;
            if (milestart.Value < prostart.Value || mileend.Value > profinish.Value || milestart.Value >= mileend.Value || milestart.Value < DateTime.Now || prostart.Value >= profinish.Value || prostart.Value < DateTime.Now)
            {
                MessageBox.Show("Please enter valid dates!", "Warning");
            }
            else
            {
                List<Employee> temp = new List<Employee>();
                Employee man = null;
                Employee assigned = null;
                if (Manager.SelectedItem != null && Assigned.SelectedItem != null)
                {

                    string[] suba = Manager.SelectedItem.ToString().Split(' ');
                    string[] subb = Assigned.SelectedItem.ToString().Split(' ');
                    string type = null;
                    foreach (Employee b in emp)
                    {

                        if (suba[0] == b.Id.ToString())
                        {
                            if (b.PId != null)
                            {
                                MessageBox.Show("Employee: " + b.Id + " already has a project please Select another!", "Warning");
                                cancel = true;
                            }
                            else
                            {

                                b.IsManager = true;
                                man = b;
                            }



                        }
                        foreach (string item in Teambox.CheckedItems)
                        {
                            string[] subs = item.Split(' ');

                            if (subs[0] == b.Id.ToString())
                            {
                                if (b.PId != null)
                                {
                                    MessageBox.Show("Employee: " + b.Id + " already has a project please select another!", "Warning");
                                    cancel = true;
                                }
                                else
                                {
                                    temp.Add(b);
                                }
                            }

                        }
                        if (subb[0] == b.Id.ToString())
                        {
                            assigned = b;
                        }


                        foreach (RadioButton rb in groupBox1.Controls)
                        {
                            if (rb.Checked)
                            {
                                type = rb.Text;
                            }
                        }
                    }
                    if (Teambox.CheckedItems == null || type == null || Proname.Text == " " || Prodes.Text == " " || MoneyRet.Value == 0 || mileexp.Text == " " || taskexp.Text == " ")
                    {

                        MessageBox.Show("Please fill all the fields before submition!", "warning");
                    }
                    else
                    {
                        if (!cancel)
                        {
                            Project project = new Project(Proname.Text, man, Prodes.Text, prostart.Value, profinish.Value, Convert.ToDecimal(MoneyRet.Value), type, temp);
                            foreach (Employee i in project.Team1)
                            {
                                foreach (Employee j in emp)
                                {
                                    if (i.Id == j.Id)
                                    {
                                        j.PId = project.PrjNo;
                                        i.PId = project.PrjNo;
                                    }
                                }
                            }
                            if (project.ProbDatStart > DateTime.Now)
                            {
                                project.PjStatus = "In Progress";
                            }
                            else
                            {

                                project.PjStatus = "To Be Started";
                            }
                            foreach (Employee em in project.Team1)
                            {
                                em.PId = project.PrjNo;
                                foreach (Employee c in emp)
                                {
                                    if (em.Id == c.Id)
                                    {
                                        c.PId = project.PrjNo;
                                    }
                                }
                            }
                            MileStone firstMile = new MileStone(mileexp.Text, milestart.Value, mileend.Value, project.PrjNo);
                            if (firstMile.StartDate > DateTime.Now)
                            {
                                firstMile.Status = "In Progress";
                            }
                            else
                            {
                                firstMile.Status = "To Be Started";
                            }
                            firstMile.PId = project.PrjNo;
                            Task firstTask = new Task(taskexp.Text, assigned.Id, firstMile.MileId);
                            firstTask.Status = "To Be Completed";
                            firstTask.MId = firstMile.MileId;
                            firstMile.Tasks.Add(firstTask);
                            project.Milestones1.Add(firstMile);
                            projects.Add(project);




                            cleanTree();
                            initializeTree();
                            clearFields();


                            this.peditdesc.SelectedTabPage = this.protasktab;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please fill all the fields before submition!", "warning");
                }
            }
        }

        private void protree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            TreeView tv = sender as TreeView;

            TreeNode tn = tv.SelectedNode;
            string[] subs = tn.Text.Split(' ');

            switch (subs[0])
            {
                case "P":
                    foreach (Project p in projects)
                    {
                        if (p.PrjNo == subs[1])
                        {
                            this.peditdesc.SelectedTabPage = this.protab;
                            ProSub.Visible = false;

                            ProUpt.Visible = true;
                            ProDel.Visible = true;


                            milesub.Visible = true;
                            miledel.Visible = false;
                            mileupt.Visible = false;
                            tasub.Visible = false;
                            tasup.Visible = false;
                            tasdel.Visible = false;
                            CompCheck.Visible = false;

                            Proname.Text = p.PjName; prostart.Value = p.ProbDatStart; profinish.Value = p.ProbDatEnd; p.Description = Prodes.Text; MoneyRet.Value = p.Monret;
                            Manager.SelectedIndex = Manager.FindString(p.PjManager.Id + " " + p.PjManager.FirstName + " " + p.PjManager.MiddleName + " " + p.PjManager.LastName);
                            for (int i = 0; i < Teambox.Items.Count; i++)
                            {
                                string[] subc = Teambox.Items[i].ToString().Split(' ');
                                foreach (Employee a in p.Team1)
                                {
                                    if (subc[0] == a.Id.ToString())
                                    {
                                        Teambox.SetItemChecked(i, true);
                                    }
                                }
                            }
                            foreach (RadioButton rb in groupBox1.Controls)
                            {
                                if (rb.Text == p.Monrettype)
                                {
                                    rb.Checked = true;
                                }
                            }
                            selecPro = p;
                        }
                    }
                    break;
                case "M":
                    foreach (Project p in projects)
                    {
                        foreach (MileStone M in p.Milestones1)
                        {
                            if (M.MileId.ToString() == subs[1])
                            {
                                this.peditdesc.SelectedTabPage = this.protab;
                                ProSub.Visible = false;
                                ProUpt.Visible = false;
                                ProDel.Visible = false;
                                milesub.Visible = false;
                                miledel.Visible = true;
                                mileupt.Visible = true;
                                tasub.Visible = true;
                                tasup.Visible = false;
                                tasdel.Visible = false;
                                CompCheck.Visible = false;
                                mileexp.Text = M.Name; milestart.Value = M.StartDate; mileend.Value = M.EndDate;
                                selecMil = M;
                            }

                        }
                    }
                    break;
                case "T":
                    foreach (Project p in projects)
                    {
                        foreach (MileStone M in p.Milestones1)
                        {
                            foreach (Task T in M.Tasks)
                            {
                                if (T.TaskId.ToString() == subs[1])
                                {
                                    this.peditdesc.SelectedTabPage = this.protab;
                                    ProSub.Visible = false;
                                    ProUpt.Visible = false;
                                    ProDel.Visible = false;
                                    milesub.Visible = false;
                                    miledel.Visible = false;
                                    mileupt.Visible = false;
                                    tasub.Visible = false;
                                    tasup.Visible = true;
                                    tasdel.Visible = true;
                                    CompCheck.Visible = true;
                                    taskexp.Text = T.Name; Assigned.SelectedIndex = Assigned.FindString(p.PjManager.Id + " " + p.PjManager.FirstName + " " + p.PjManager.MiddleName + " " + p.PjManager.LastName);
                                    selecTask = T;
                                }
                            }
                        }
                    }
                    break;
                default:
                    MessageBox.Show("Plaese select a valid node", "Warning");
                    break;

            }

        }

        private void ProDel_Click(object sender, EventArgs e)
        {
            foreach (Project p in projects)
            {
                if (p.PrjNo == selecPro.PrjNo)
                {

                    Employee oldman = p.PjManager;

                    if (!isStillManger(oldman))
                    {

                        oldman.IsManager = false;



                    }

                    foreach (Employee a in p.Team1)
                    {
                        foreach (Employee b in emp)
                        {
                            if (a.Id == b.Id)
                            {
                                b.PId = null;
                            }
                        }

                    }
                    projects.Remove(p);
                    selecPro = null;
                    this.peditdesc.SelectedTabPage = this.protasktab;
                    break;
                }
            }

            cleanTree();
            initializeTree();

            this.peditdesc.SelectedTabPage = this.protasktab;

        }

        private void ProUpt_Click(object sender, EventArgs e)
        {
            bool cancel = false;
            if (prostart.Value >= profinish.Value || prostart.Value < DateTime.Now)
            {
                MessageBox.Show("Please enter valid dates!", "Warning");
            }
            else
            {

                foreach (Project p in projects)
                {
                    if (p.PrjNo == selecPro.PrjNo)
                    {

                        List<Employee> temp = new List<Employee>();

                        Employee man = null;

                        if (Manager.SelectedItem != null)
                        {
                            string[] suba = Manager.SelectedItem.ToString().Split(' ');
                            string type = null;
                            foreach (Employee b in emp)
                            {

                                if (suba[0] == b.Id.ToString())
                                {
                                    if (b.PId != null)
                                    {
                                        MessageBox.Show("Employee: " + b.Id + " already has a project please select another!", "Warning");
                                    }
                                    else
                                    {

                                        b.IsManager = true;
                                        man = b;
                                    }



                                }
                                foreach (string item in Teambox.CheckedItems)
                                {
                                    string[] subs = item.Split(' ');

                                    if (subs[0] == b.Id.ToString())
                                    {
                                        if (b.PId != null)
                                        {
                                            MessageBox.Show("Employee: " + b.Id + " already has a project please select another!", "Warning");
                                        }
                                        else
                                        {
                                            temp.Add(b);
                                        }
                                    }

                                }

                                foreach (RadioButton rb in groupBox1.Controls)
                                {
                                    if (rb.Checked)
                                    {
                                        type = rb.Text;
                                    }
                                }
                                if (Teambox.CheckedItems == null || type == null || Proname.Text == " " || Prodes.Text == " " || MoneyRet.Value == 0)
                                {
                                    MessageBox.Show("Please fill all the fields before submition!", "warning");
                                }
                                else
                                {
                                    if (!cancel)
                                    {
                                        Employee oldman = p.PjManager;
                                        foreach (Employee g in p.Team1)
                                        {
                                            foreach (Employee h in emp)
                                            {
                                                if (g.Id == h.Id)
                                                {
                                                    g.PId = null;
                                                }
                                            }
                                        }
                                        p.Team1 = temp; p.PjManager = man; p.Monrettype = type; p.PjName = Proname.Text; p.Description = Prodes.Text; p.ProbDatEnd = profinish.Value; p.ProbDatStart = prostart.Value; p.Monret = Convert.ToDecimal(MoneyRet.Value);
                                        foreach (Employee i in p.Team1)
                                        {
                                            foreach (Employee j in emp)
                                            {
                                                if (i.Id == j.Id)
                                                {
                                                    j.PId = p.PrjNo;
                                                    i.PId = p.PrjNo;
                                                }
                                            }
                                        }
                                        if (!isStillManger(oldman))
                                        {
                                            oldman.IsManager = false;


                                        }

                                        cleanTree();
                                        initializeTree();
                                        clearFields();
                                        this.peditdesc.SelectedTabPage = this.protasktab;
                                    }
                                }

                            }
                        }
                        else { MessageBox.Show("Please enter valid dates! and/or Fill all the fields before submition!", "Warning"); }
                    }
                }

            }
        }

        private void milesub_Click(object sender, EventArgs e)
        {
            foreach (Project p in projects)
            {
                if (mileend.Value > p.ProbDatEnd || milestart.Value >= mileend.Value || milestart.Value < DateTime.Now || milestart.Value < p.ProbDatStart)
                {
                    MessageBox.Show("Please enter valid dates!", "Warning");
                    break;
                }
                else
                {
                    if (p.PrjNo == selecPro.PrjNo)
                    {

                        MileStone mile = new MileStone(mileexp.Text, milestart.Value, mileend.Value, p.PrjNo);
                        Employee assigned = null;
                        if (Assigned.SelectedItem != null)
                        {
                            string[] subb = Assigned.SelectedItem.ToString().Split(' ');
                            foreach (Employee b in emp)
                            {
                                if (subb[0] == b.Id.ToString())
                                {
                                    assigned = b;
                                }
                            }
                            if (mileexp.Text == " " || taskexp.Text == " ")
                            {
                                MessageBox.Show("Please fill all the fields before submition!", "warning");
                                break;
                            }
                            else
                            {
                                Task task = new Task(taskexp.Text, assigned.Id, mile.MileId);
                                task.Status = "To Be Completed";
                                mile.Tasks.Add(task);
                                if (mile.StartDate > DateTime.Now)
                                {
                                    mile.Status = "In Progress";
                                }
                                else
                                {
                                    mile.Status = "To Be Started";
                                }
                                p.Milestones1.Add(mile);
                                mile.PId = p.PrjNo;
                                cleanTree();
                                initializeTree();
                                clearFields();
                                this.peditdesc.SelectedTabPage = this.protasktab;
                            }
                        }
                        else { MessageBox.Show("Please enter valid dates! and/or Fill all the fields before submition!", "Warning"); }
                    }
                }
            }

        }

        private void mileupt_Click(object sender, EventArgs e)
        {
            foreach (Project p in projects)
            {

                if (selecMil.PId == p.PrjNo)
                {
                    if (mileend.Value > p.ProbDatEnd || milestart.Value >= mileend.Value || milestart.Value < DateTime.Now || milestart.Value < p.ProbDatStart || mileexp.Text == " ")
                    {
                        MessageBox.Show("Please enter valid dates! and/or Fill all the fields before submition!", "Warning");
                        break;
                    }
                    else
                    {
                        p.Milestones1.Remove(selecMil);
                        selecMil.Name = mileexp.Text; selecMil.EndDate = mileend.Value; selecMil.StartDate = milestart.Value;
                        if (selecMil.StartDate > DateTime.Now)
                        {
                            selecMil.Status = "In Progress";
                        }
                        else
                        {
                            selecMil.Status = "To Be Started";
                        }
                        p.Milestones1.Add(selecMil);
                        cleanTree();
                        initializeTree();
                        clearFields();
                        this.peditdesc.SelectedTabPage = this.protasktab;
                    }
                }
            }


        }

        private void miledel_Click(object sender, EventArgs e)
        {
            foreach (Project p in projects)
            {
                if (selecMil.PId == p.PrjNo)
                {
                    p.Milestones1.Remove(selecMil);
                    cleanTree();
                    initializeTree();
                    this.peditdesc.SelectedTabPage = this.protasktab;
                }
            }
        }

        private void tasub_Click(object sender, EventArgs e)
        {
            Employee assigned = null;
            if (Assigned.SelectedItem != null)
            {
                string[] subb = Assigned.SelectedItem.ToString().Split(' ');
                foreach (Employee b in emp)
                {
                    if (subb[0] == b.Id.ToString())
                    {
                        assigned = b;
                    }
                }
                if (taskexp.Text == " ")
                {
                    MessageBox.Show("Please fill all the fields before submition!", "warning");
                }
                else
                {
                    Task task = new Task(taskexp.Text, assigned.Id, selecMil.MileId);
                    task.Status = "To Be Completed";
                    selecMil.Tasks.Add(task);
                    task.MId = selecMil.MileId;
                    cleanTree();
                    initializeTree();
                    clearFields();
                    this.peditdesc.SelectedTabPage = this.protasktab;
                }
            }
            else
            {
                MessageBox.Show("Please enter valid dates! and/or Fill all the fields before submition!", "Warning");
            }
        }

        private void tasup_Click(object sender, EventArgs e)
        {
            Employee assigned = null;
            if (Assigned.SelectedItem != null)
            {


                string[] subb = Assigned.SelectedItem.ToString().Split(' ');
                foreach (Employee b in emp)
                {
                    if (subb[0] == b.Id.ToString())
                    {
                        assigned = b;
                    }
                }
                if (taskexp.Text == " ")
                {
                    MessageBox.Show("Please fill all the fields before submition!", "warning");
                }
                else
                {
                    if (CompCheck.Checked)
                    {
                        selecTask.Status = "Completed";

                        foreach (Project p in projects)
                        {

                            Boolean allMileComp = true;
                            foreach (MileStone m in p.Milestones1)
                            {
                                Boolean allTaskComp = true;

                                foreach (Task t in m.Tasks)
                                {
                                    if (t.Status != "Completed")
                                    {
                                        allTaskComp = false;
                                    }
                                }

                                if (allTaskComp == true)
                                {
                                    m.Status = "Completed";
                                }
                                else
                                {
                                    allMileComp = false;
                                }

                            }

                            if (allMileComp == true)
                            {
                                p.PjStatus = "Completed";
                                p.DatEnd1 = DateTime.Now;
                                if (!p.MailSent1)
                                {
                                    foreach (Employee a in p.Team1)
                                    {
                                        try
                                        {
                                        MailAddress to = new MailAddress(a.Email, "test");
                                        MailAddress from = new MailAddress("gtafurkan@outlook.com", "test1");

                                        MailMessage email = new MailMessage(from, to);
                                        email.Subject = "Project Completed";
                                        email.Body = "Project" + p.PrjNo + "You were working on was Completed";

                                        smtp.Send(email);
                                        p.MailSent1 = true;
                                        }
                                        catch (SmtpException ex)
                                        {
                                            Console.WriteLine(ex.ToString());
                                        }
                                        catch (FormatException i)
                                        {
                                            MessageBox.Show("Email adress of: " + a.Id + " is not valid so tehy will not going to recieve an e-mail", "Warning");
                                            Console.WriteLine(i.ToString());
                                        }
                                    }
                                    
                                }
                            }

                        }
                        cleanTree();
                        initializeTree();
                    }
                    foreach (Project p in projects)
                    {
                        foreach (MileStone a in p.Milestones1)
                        {
                            if (selecTask.MId == a.MileId)
                            {
                                selecTask.Name = taskexp.Text;
                                selecTask.AssignedTo = assigned.Id;
                            }
                        }
                    }

                    cleanTree();
                    initializeTree();
                    clearFields();
                    this.peditdesc.SelectedTabPage = this.protasktab;
                }


            }
        }

        private void tasdel_Click(object sender, EventArgs e)
        {
            string mileStone = selecTask.MId;
            foreach (Project p in projects)
            {
                foreach (MileStone a in p.Milestones1)
                {
                    if (mileStone == a.MileId)
                    {
                        a.Tasks.Remove(selecTask);

                    }
                }

                cleanTree();
                initializeTree();
                clearFields();
                this.peditdesc.SelectedTabPage = this.protasktab;
            }
        }

        private void Outclick_Click(object sender, EventArgs e)
        {
            UpdateDataBase();
            frm2 = new frmLog(frm);
            frm2.isLogOut = true;
            frm2.Show();
            this.Hide();
        }

        private void UpdateDataBase()
        {
            foreach (var item in Program.Codefirst.tasks)
            {
                Program.Codefirst.tasks.Remove(item);
            }
            Program.Codefirst.SaveChanges();
            foreach (var item in Program.Codefirst.MileStones)
            {
                Program.Codefirst.MileStones.Remove(item);
            }
            Program.Codefirst.SaveChanges();
            foreach (var item in Program.Codefirst.projects)
            {
                item.PjManager = null;
            }
            Program.Codefirst.SaveChanges();
            foreach (var item in Program.Codefirst.Employees)
            {
                Program.Codefirst.Employees.Remove(item);
            }
            Program.Codefirst.SaveChanges();
            foreach (var item in Program.Codefirst.projects)
            {
                Program.Codefirst.projects.Remove(item);
            }
            Program.Codefirst.SaveChanges();


            List<ManagerHolder> Employees = new List<ManagerHolder>();
            foreach (Project p in projects)
            {
                ManagerHolder temp = new ManagerHolder(p.PjManager, p.PrjNo);
                Employees.Add(temp);
                p.PjManager = null;
                Program.Codefirst.projects.Add(p);
            }
            Program.Codefirst.SaveChanges();


            foreach (Employee i in emp)
            {
                if (i.PId == null)
                {
                    Program.Codefirst.Employees.Add(i);
                }
            }

            Program.Codefirst.SaveChanges();

            foreach (var o in Program.Codefirst.projects)
            {
                foreach (ManagerHolder n in Employees)
                {
                    if (n.project.Equals(o.PrjNo))
                    {
                        Console.WriteLine(o.PrjNo.ToString());
                        o.PjManager = n.manager;

                    }
                }
            }
            Program.Codefirst.SaveChanges();
            emp.Clear();
            projects.Clear();



        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateDataBase();
        }

        private void ListEmployees_Click(object sender, EventArgs e)
        {
            UpdateDataBase();
            UpdateProject();
            Updateemp();
            delItems();
            getItems();
            refreshEmpgrid();
            cleanTree();
            initializeTree();
            SqlConnection con = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=Project_Managment_vol_2.Contexts.CodeFirstContext;Trusted_Connection=True;");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT [ID],[Phone] FROM [Project_Managment_vol_2.Contexts.CodeFirstContext].[dbo].[Employees] ORDER BY [Id]";
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            ConsoleTable table = new ConsoleTable("Id's", "Phone Numbers");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                table.AddRow(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][1].ToString());
            }
            table.Write();
        }
    }
    class ManagerHolder
    {
        public Employee manager;
        public string project;
        public ManagerHolder(Employee manager, string project)
        {
            this.manager = manager;
            this.project = project;
        }
    }
    class User
    {
        private string role;
        private string email;
        private string password;
        [Key]
        private string userId;
        private static int counter = 0;

        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Role { get => role; set => role = value; }
        public string UserId { get => userId; set => userId = value; }
        public int Counter { get => counter; set => counter = value; }

        public User(string role, string email, string password)
        {
            Role = role;
            Password = password;
            Email = email;
            UserId = Counter.ToString();
            Counter++;
        }
        public User() { }
    }

    class Employee
    {
        private static int counter = 0;
        private string id;
        private string email;
        private string firstName;
        private string middleName;
        private string lastName;
        private DateTime birthday;
        private string adress1;
        private string adress2;
        private bool isManager;
        private string password;


        private string phone;
        private string pId;




        [Key]
        public string Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string MiddleName { get => middleName; set => middleName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public DateTime Birthday { get => birthday; set => birthday = value; }
        public string Email { get => email; set => email = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Address1 { get => adress1; set => adress1 = value; }
        public string Address2 { get => adress2; set => adress2 = value; }
        public bool IsManager { get => isManager; set => isManager = value; }
        public string Password { get => password; set => password = value; }



        public string PId { get => pId; set => pId = value; }

        public int Counter { get => counter; set => counter = value; }

        public Employee(string firstName, string middleName, string lastName, DateTime birthday, string email, string phone, string adress1, string adress2, string password)
        {
            Id = Counter.ToString();
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Birthday = birthday;
            Email = email;
            Phone = phone;
            Address1 = adress1;
            Address2 = adress2;
            Password = password;

            Counter++;
        }
        public Employee() { }
    }





    class MileStone
    {
        private string name;
        private String status;
        private List<Task> tasks;
        private DateTime startDate;
        private DateTime endDate;
        private static int countM = 0;
        private string mileId;
        private string pId;

        public string Name { get => name; set => name = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public String Status { get => status; set => status = value; }
        public List<Task> Tasks { get => tasks; set => tasks = value; }
        [Key]
        public string MileId { get => mileId; set => mileId = value; }



        public string PId { get => pId; set => pId = value; }
        public int CountM { get => countM; set => countM = value; }

        public MileStone(string name, DateTime startDate, DateTime endDate, string pId)
        {
            Name = name; StartDate = startDate; EndDate = endDate; MileId = CountM.ToString(); CountM++; PId = pId;
            Tasks = new List<Task>();

        }
        public MileStone() { }
    }



    class Task
    {
        private string name;
        private String status;
        private string assignedTo;
        private static int counT = 0;
        private string taskId;
        private string mId;

        public string Name { get => name; set => name = value; }

        public String Status { get => status; set => status = value; }
        public string AssignedTo { get => assignedTo; set => assignedTo = value; }
        public string TaskId { get => taskId; set => taskId = value; }
        public string MId { get => mId; set => mId = value; }
        public int CounT { get => counT; set => counT = value; }

        public Task(string name, string assignedTo, string mId)
        {
            Name = name; AssignedTo = assignedTo; TaskId = CounT.ToString(); CounT++; MId = mId;

        }
        public Task() { }
    }

    class Project
    {
        private static int count = 0;

        private string pjName;


        private string prjNo;
        private Employee pjManager;
        private string description;
        private DateTime DatReg;
        private bool MailSent;
        private DateTime DatEnd;
        private DateTime probDatStart;
        private DateTime probDatEnd;
        private String pjStatus;
        private Decimal monret;
        private string monrettype;
        private List<Employee> Team;
        private List<MileStone> Milestones;




        public Project(string Name, Employee Manager, string Purpose, DateTime start, DateTime end, Decimal money, string moneytyp, List<Employee> e)
        {

            PjName = Name;
            PjManager = Manager;
            Description = Purpose;
            ProbDatStart = start;
            ProbDatEnd = end;
            Monret = money;
            Monrettype = moneytyp;
            PrjNo = "PRJ" + Count.ToString();
            DatReg1 = DateTime.Now;
            Team1 = e;
            Milestones1 = new List<MileStone>();
            DatEnd1 = new DateTime(1900, 01, 01);


            Count++;

        }

        public string PjName { get => pjName; set => pjName = value; }
        [Key]
        public string PrjNo { get => prjNo; set => prjNo = value; }
        public string Description { get => description; set => description = value; }
        public DateTime DatReg1 { get => DatReg; set => DatReg = value; }

        public DateTime DatEnd1 { get => DatEnd; set => DatEnd = value; }
        public DateTime ProbDatStart { get => probDatStart; set => probDatStart = value; }
        public DateTime ProbDatEnd { get => probDatEnd; set => probDatEnd = value; }
        public Decimal Monret { get => monret; set => monret = value; }
        public string Monrettype { get => monrettype; set => monrettype = value; }
        public List<Employee> Team1 { get => Team; set => Team = value; }


        public Employee PjManager { get => pjManager; set => pjManager = value; }
        public String PjStatus { get => pjStatus; set => pjStatus = value; }
        public List<MileStone> Milestones1 { get => Milestones; set => Milestones = value; }
        public int Count { get => count; set => count = value; }
        public bool MailSent1 { get => MailSent; set => MailSent = value; }

        public Project() { }


    }
}

