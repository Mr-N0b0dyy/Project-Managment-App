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
using Image = System.Drawing.Image;
using DevExpress.Utils.DPI;

namespace Project_Managment_vol_2
{

    public partial class Main : Form
    {

        frmReg frm;

        public Main(frmReg frmReg)
        {
            InitializeComponent();
            this.frm = frmReg;
            Employee employee1 = new Employee("Ahmet", "Can", "Köse", new DateTime(1995, 02, 03), "ahmetcan@gmail.com", "05322335682", "a", "b", "1234", null);
            Employee employee2 = new Employee("Mehmet", "Berk", "Köse", new DateTime(1990, 03, 02), "mehmetberk@gmail.com", "05322335683", "a", "b", "12345", null);
            Employee employee3 = new Employee("Zeynep", "Melis", "Yılmaz", new DateTime(2000, 02, 03), "zeynyıl@gmail.com", "05436562319", "c", "d", "123456", null);
            emp.Add(employee1);
            emp.Add(employee2);
            emp.Add(employee3);
            getItems();
            var source = new BindingSource(emp, null);
            Empgrid.DataSource = source;
            Empgrid.AutoResizeColumns();

        }


        BindingList<Employee> emp = new BindingList<Employee>();
        internal BindingList<Employee> Emp { get => emp; set => emp = value; }
        public static DateTime MaxDT = new DateTime(2005, 01, 01, 00, 00, 00);
        Employee selectedEmp;
        List<Project> projects = new List<Project>();
        Project selecPro;
        MileStone selecMil;
        Task selecTask;


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
                    break;

                case "ProTask":
                    this.peditdesc.SelectedTabPage = this.protasktab;
                    
                    break;

                case "Empl":
                    this.peditdesc.SelectedTabPage = this.emptab;
                    break;
            }
        }
        public void initializeTree()
        {

            int counter = 0; 
            foreach(Project proje in projects)
            {
                int counter2 = 1;
                protree.Nodes.Add("P " + proje.PrjNo + " " + proje.PjName);
                foreach (MileStone M in proje.Milestones1)
                {
                    int counter3 = 1;
                    protree.Nodes[counter].Nodes.Add("Status:" + proje.PjStatus.sıtatus + "\n End Date:" + proje.ProbDatEnd + "\nMoney:" + proje.Monret + "\nMoney Type:" + proje.Monrettype);
                    protree.Nodes[counter].Nodes.Add("M " + M.MileId + " " + M.Name);

                    foreach (Task T in M.Tasks)
                    {
                        protree.Nodes[counter].Nodes[counter2].Nodes.Add("Start Date:" + M.StartDate + "\nExp. EndDate:" + M.EndDate + "\nStatus" + M.Status.sıtatus);
                        protree.Nodes[counter].Nodes[counter2].Nodes.Add("T " + T.TaskId + " " + T.Name);
                        protree.Nodes[counter].Nodes[counter2].Nodes[counter3].Nodes.Add ("Status:" + T.Status.sıtatus+ "\nAssignedTo:" + T.AssignedTo.FirstName+T.AssignedTo.MiddleName+T.AssignedTo.LastName);
                        counter3++;
                    }
                    counter2++;
                }
                counter++;
                
                protree.Refresh();
            }
        }
        public void cleanTree()
        {
            protree.Nodes.Clear();
            protree.Refresh();
            
        }
        
        public void delItems()
        {
            foreach (Employee i in emp)
            {
                Teambox.Items.Clear();
                Manager.Items.Clear();
                Assigned.Items.Clear();
            }
        }
        public void getItems()
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

        private void EmpSubmit_Click(object sender, EventArgs e)
        {

            if (Email.Text == " " || Nametxt.Text == " " || Mname.Text == " " || Lname.Text == " " || Birth.Value == MaxDT || Add1.Text == " " || Add2.Text == " " || Pass.Text == " ")
            {
                MessageBox.Show("Please fill all the fields before submition!", "Warning");
            }
            else
            {

                Employee employee = new Employee(Nametxt.Text, Mname.Text, Lname.Text, Birth.Value, Email.Text, Pnumber.Text, Add1.Text, Add2.Text, Pass.Text, Emppic.Image);

                emp.Add(employee);
                delItems();
                getItems();


                User user = new User();

                user.Email = Email.Text;
                user.Password = Pass.Text;
                user.Role = "Employee";

                frm.Users.Add(user);

                Nametxt.Text = " ";
                Mname.Text = " ";
                Lname.Text = " ";
                Lname.Text = " ";
                Emppic.Image = null;
                Add1.Text = " ";
                Add2.Text = " ";
                Pass.Text = " ";
                Birth.Value = MaxDT;
                Email.Text = " ";
                Pnumber.Text = " ";

            }
        }

        private void PicChoose_Click(object sender, EventArgs e)
        {
            Emppic.SizeMode = PictureBoxSizeMode.StretchImage;
            openFileDialog1.ShowDialog();
            Emppic.ImageLocation = openFileDialog1.FileName;
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
                        Console.WriteLine("aa");
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
                Emppic.Image = selectedEmp.Image;
            }
            else { };
        }

        private void EmpUpt_Click(object sender, EventArgs e)
        {
            if (selectedEmp != null)
            {
                if ((Email.Text == " ") || (Nametxt.Text == " ") || (Mname.Text == " ") || (Lname.Text == " ") || Birth.Value == MaxDT || (Add1.Text == " ") || (Add2.Text == " ") || (Pass.Text == " "))
                {
                    MessageBox.Show("please fill all the fields before submition!", "Warning");
                }
                else
                {
                    selectedEmp.FirstName = Nametxt.Text;
                    selectedEmp.MiddleName = Mname.Text;
                    selectedEmp.LastName = Lname.Text;
                    selectedEmp.Email = Email.Text;
                    selectedEmp.Birthday = Birth.Value;
                    selectedEmp.Adress1 = Add1.Text;
                    selectedEmp.Adress2 = Add2.Text;
                    selectedEmp.Password = Pass.Text;
                    selectedEmp.Image = Emppic.Image;
                    delItems();
                    getItems();
                    refreshEmpgrid();
                }
            }
            else { MessageBox.Show("please select an employee from grid first!", "Warning"); }
        }

        private void EmpDel_Click(object sender, EventArgs e)
        {
            if (selectedEmp != null)
            {
                emp.Remove(selectedEmp);
                delItems();
                getItems();
                refreshEmpgrid();
            }
            else { MessageBox.Show("Please select an employee from grid first!", "Warning"); }
        }

        


        private void ProSub_Click(object sender, EventArgs e)
        {
            if (milestart.Value < prostart.Value || mileend.Value > profinish.Value || milestart.Value >= mileend.Value || milestart.Value < DateTime.Now || prostart.Value >= profinish.Value || prostart.Value < DateTime.Now)
            {
                MessageBox.Show("Please enter valid dates!", "Warning");
            }
            else
            {
                List<Employee> temp = new List<Employee>();
                Employee man = null;
                Employee assigned = null;
                string[] suba = Manager.SelectedItem.ToString().Split(' ');
                string[] subb = Assigned.SelectedItem.ToString().Split(' ');
                string type = null;



                foreach (Employee b in emp)
                {
                    if (suba[0] == b.Id.ToString())
                    {
                        man = b;
                    }
                    if (subb[0] == b.Id.ToString())
                    {
                        assigned = b;
                    }

                    foreach (string item in Teambox.CheckedItems)
                    {
                        string[] subs = item.Split(' ');
                        foreach (Employee a in emp)
                        {
                            if (subs[0] == a.Id.ToString())
                            {
                                temp.Add(a);
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
                }
                if (Teambox.CheckedItems == null || type == null || Proname.Text == " " || Prodes.Text == " " || MoneyRet.Value == 0 || man == null || mileexp.Text == " " || taskexp.Text == " " || assigned == null)
                {

                    MessageBox.Show("Please fill all the fields before submition!", "warning");
                }
                else
                {
                    Project project = new Project(Proname.Text, man, Prodes.Text, prostart.Value, profinish.Value, Convert.ToDecimal(MoneyRet.Value), type, temp);
                    if (project.ProbDatStart > DateTime.Now)
                    {
                        project.PjStatus.sıtatus = "In Progress";
                    }
                    else
                    {

                        project.PjStatus.sıtatus = "To Be Started";
                    }

                    MileStone firstMile = new MileStone(mileexp.Text, milestart.Value, mileend.Value, project.PrjNo);
                    if (firstMile.StartDate > DateTime.Now)
                    {
                        firstMile.Status.sıtatus = "In Progress";
                    }
                    else
                    {
                        firstMile.Status.sıtatus = "To Be Started";
                    }
                    Task firstTask = new Task(taskexp.Text, assigned, firstMile.MileId);
                    firstTask.Status.sıtatus = "To Be Completed";
                    firstMile.Tasks.Add(firstTask);
                    project.Milestones1.Add(firstMile);
                    projects.Add(project);

                    cleanTree();
                    initializeTree();
                    Proname.Text = " "; Prodes.Text = " "; MoneyRet.Value = 0; taskexp.Text = " "; assigned = null; type = null; Manager.SelectedIndex = -1; Teambox.ClearSelected(); mileexp.Text = " ";taskexp.Text = " "; groupBox1.Controls.Clear();
                }
            }
        }

        private void protree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = sender as TreeNode;
                    
            string[] subs = node.Text.Split(' ');
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

                            Proname.Text = p.PjName;  prostart.Value = p.ProbDatStart; profinish.Value = p.ProbDatEnd; p.Description = Prodes.Text; MoneyRet.Value = p.Monret;
                            Manager.SelectedIndex = Manager.FindString(p.PjManager.Id + " " + p.PjManager.FirstName + " " + p.PjManager.MiddleName + " " + p.PjManager.LastName);
                            for(int i = 0; i<Teambox.Items.Count; i++)
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
                                if (rb.Text == p.Monrettype )
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
            foreach(Project p in projects)
            {
                if(p.PrjNo == selecPro.PrjNo)
                {
                    projects.Remove(p);
                    selecPro = null;
                    this.peditdesc.SelectedTabPage = this.protasktab;
                    break;
                }
            }
            cleanTree();
            initializeTree();

        }

        private void ProUpt_Click(object sender, EventArgs e)
        {
            if (   prostart.Value >= profinish.Value || prostart.Value < DateTime.Now)
            {
                MessageBox.Show("Please enter valid dates!", "Warning");
            }
            else {
                foreach (Project p in projects)
                {
                    if (p.PrjNo == selecPro.PrjNo)
                    {


                        List<Employee> temp = new List<Employee>();
                        Employee man = null;

                        string[] suba = Manager.SelectedItem.ToString().Split(' ');

                        string type = null;
                        foreach (Employee b in emp)
                        {
                            if (suba[0] == b.Id.ToString())
                            {
                                man = b;
                            }



                            foreach (string item in Teambox.CheckedItems)
                            {
                                string[] subs = item.Split(' ');
                                foreach (Employee a in emp)
                                {
                                    if (subs[0] == a.Id.ToString())
                                    {
                                        temp.Add(a);
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
                            if (Teambox.CheckedItems == null || type == null || Proname.Text == " " || Prodes.Text == " " || MoneyRet.Value == 0 || man == null){
                                MessageBox.Show("Please fill all the fields before submition!", "warning");
                            }
                            else {
                                p.Team1 = temp; p.PjManager = man; p.Monrettype = type; p.PjName = Proname.Text; p.Description = Prodes.Text; p.ProbDatEnd = profinish.Value; p.ProbDatStart = prostart.Value; p.Monret = Convert.ToDecimal(MoneyRet.Value); }
                        }

                    }
                }
                cleanTree();
                initializeTree();
            }
        }

        private void milesub_Click(object sender, EventArgs e)
        {
           
            
                foreach (Project p in projects)
                {
                if (mileend.Value > p.ProbDatStart || milestart.Value >= mileend.Value || milestart.Value < DateTime.Now || milestart.Value < p.ProbDatStart)
                {
                    MessageBox.Show("Please enter valid dates!", "Warning");
                    break;
                }
                else {
                    if (p.PrjNo == selecPro.PrjNo)
                    {

                        MileStone mile = new MileStone(mileexp.Text, milestart.Value, mileend.Value, p.PrjNo);
                        Employee assigned = null;
                        string[] subb = Assigned.SelectedItem.ToString().Split(' ');
                        foreach (Employee b in emp)
                        {
                            if (subb[0] == b.Id.ToString())
                            {
                                assigned = b;
                            }
                        }
                        if (mileexp.Text == " " || taskexp.Text == " " || assigned == null)
                        {
                            MessageBox.Show("Please fill all the fields before submition!", "warning");
                            break;
                        }
                        else
                        {
                            mile.Tasks.Add(new Task(taskexp.Text, assigned, mile.MileId));
                            p.Milestones1.Add(mile);
                            cleanTree();
                            initializeTree();
                        }
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
                    if (mileend.Value > p.ProbDatStart || milestart.Value >= mileend.Value || milestart.Value < DateTime.Now || milestart.Value < p.ProbDatStart|| mileexp.Text == " ")
                    {
                        MessageBox.Show("Please enter valid dates! and/or Fill all the fields before submition!", "Warning");
                        break;
                    }
                    else
                    {
                        p.Milestones1.Remove(selecMil);
                        selecMil.Name = mileexp.Text; selecMil.EndDate = mileend.Value; selecMil.StartDate = milestart.Value;
                        cleanTree();
                        initializeTree();
                    }
                }
            }
            

        }

        private void miledel_Click(object sender, EventArgs e)
        {
            foreach(Project p in projects)
            {
                if(selecMil.PId == p.PrjNo)
                {
                    p.Milestones1.Remove(selecMil);
                    cleanTree();
                    initializeTree();
                }
            }
        }

        private void tasub_Click(object sender, EventArgs e)
        {
            Employee assigned = null;
            string[] subb = Assigned.SelectedItem.ToString().Split(' ');
            foreach (Employee b in emp)
            {
                if (subb[0] == b.Id.ToString())
                {
                    assigned = b;
                }
            }
            if(taskexp.Text == " " || assigned == null)
            {
                MessageBox.Show("Please fill all the fields before submition!", "warning");
            }
            else { 
            selecMil.Tasks.Add(new Task(taskexp.Text, assigned, selecMil.MileId));
            cleanTree();
            initializeTree();}
        }
    }

    class User
    {
        private string role;
        private string email;
        private string password;

        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Role { get => role; set => role = value; }
    }

    class Employee
    {
        private static int counter = 0;
        private int id;
        private string email;
        private string firstName;
        private string middleName;
        private string lastName;
        private DateTime birthday;
        private string adress1;
        private string adress2;
        private bool isManager;
        private string password;
        private List<Project> projects;
        private Image image;
        private string phone;



        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string MiddleName { get => middleName; set => middleName = value; }
        public string LastName { get => lastName; set => lastName = value; } 
        public DateTime Birthday { get => birthday; set => birthday = value; }
        public string Email { get => email; set => email = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Adress1 { get => adress1; set => adress1 = value; }
        public string Adress2 { get => adress2; set => adress2 = value; }
        public bool IsManager { get => isManager; set => isManager = value; }
        public string Password { get => password; set => password = value; }

        public Image Image { get => image; set => image = value; }
        private List<Project> Projects { get => projects; set => projects = value; }

        public Employee(string firstName, string middleName, string lastName, DateTime birthday, string email, string phone, string adress1, string adress2, string password, Image image)
        {
            Id = counter;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Birthday = birthday;
            Email = email;
            Phone = phone;
            Adress1 = adress1;
            Adress2 = adress2;
            Password = password;
            Image = image;
            counter++;
        }
    }

    class Status // Project status
    {
        private string status;

        public string sıtatus { get => status; set => status = value; }
    }

    class ProjectType
    {
        private string type;

        public string Type { get => type; set => type = value; }
    }

    class MileStone
    {
        private string name;
        private Status status;
        private List<Task> tasks;
        private DateTime startDate;
        private DateTime endDate;
        private static int countM = 0;
        private int mileId;
        private string pId;

        public string Name { get => name; set => name = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        internal Status Status { get => status; set => status = value; }
        internal List<Task> Tasks { get => tasks; set => tasks = value; }
        public int MileId { get => mileId; set => mileId = value; }
        public string PId { get => pId; set => pId = value; }

        public MileStone(string name, DateTime startDate, DateTime endDate, string pId)
        {
            Name = name;  StartDate = startDate; EndDate = endDate;MileId = countM; countM++; PId = pId;
            Tasks = new List<Task>();
            Status = new Status();
        }
    }

    

        class Task
        {
            private string name;
            private Status status;
            private Employee assignedTo;
            private static int counT = 0;
            private int taskId;
            private int mId;

        public string Name { get => name; set => name = value; }
        
        internal Status Status { get => status; set => status = value; }
        internal Employee AssignedTo { get => assignedTo; set => assignedTo = value; }
        public int TaskId { get => taskId; set => taskId = value; }
        public int MId { get => mId; set => mId = value; }

        public Task(string name, Employee assignedTo,int mId)
        {
            Name = name; AssignedTo = assignedTo; TaskId = counT; counT++; MId = mId;
            Status = new Status();
        }
    }

        class Project
        {
            private static int count = 1;

            private string pjName;
            private string prjNo;
            private Employee pjManager;
            private string description;
            private DateTime DatReg;
            private DateTime DatStart;
            private DateTime DatEnd;
            private DateTime probDatStart;
            private DateTime probDatEnd;
            private Status pjStatus;
            private Decimal monret;
            private string monrettype;
            private List<Employee> Team;
            private List<MileStone> Milestones;
            



            public Project(string Name, Employee Manager, string Purpose, DateTime start, DateTime end,Decimal money, string moneytyp, List<Employee> e)
            {
                PjName = Name;
                PjManager = Manager;
                Description = Purpose;
                ProbDatStart = start;
                ProbDatStart = end;
                Monret = money;
                Monrettype = moneytyp;  
                PrjNo = "PRJ" + count.ToString();
                DatReg1 = DateTime.Now;
                Team1 = e;
                Milestones1 = new List<MileStone>();
                PjStatus = new Status();
                count++;

            }

            public string PjName { get => pjName; set => pjName = value; }
            public string PrjNo { get => prjNo; set => prjNo = value; }
            public string Description { get => description; set => description = value; }
            public DateTime DatReg1 { get => DatReg; set => DatReg = value; }
            public DateTime DatStart1 { get => DatStart; set => DatStart = value; }
            public DateTime DatEnd1 { get => DatEnd; set => DatEnd = value; }
            public DateTime ProbDatStart { get => probDatStart; set => probDatStart = value; }
            public DateTime ProbDatEnd { get => probDatEnd; set => probDatEnd = value; }
            public Decimal Monret { get => monret; set => monret = value; }
            public string Monrettype { get => monrettype; set => monrettype = value; }
            public List<Employee> Team1 { get => Team; set => Team = value; }

            public Employee PjManager { get => pjManager; set => pjManager = value; }
            public Status PjStatus { get => pjStatus; set => pjStatus = value; }
            public List<MileStone> Milestones1 { get => Milestones; set => Milestones = value; }
    }
}

