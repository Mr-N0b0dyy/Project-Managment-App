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


    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
    }

    public static class JsonSerialization
    {
        /// <summary>
        /// Writes the given object instance to a Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = Newtonsoft.Json.JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }


    class Admin
    {
        private string email;
        private string password ;
        private string adminKey = "aziz";

        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string AdminKey { get => adminKey; }
    }

    class Employee
    {
        private string email;
        private string firstName;
        private string middleName;
        private string lastName;
        private DateTime birthday;
        private string adress1;
        private string adress2;
        private bool isManager;
        private string password; 
        private Project project;

        public string Email { get => email; set => email = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string MiddleName { get => middleName; set => middleName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public DateTime Birthday { get => birthday; set => birthday = value; }
        public string Adress1 { get => adress1; set => adress1 = value; }
        public string Adress2 { get => adress2; set => adress2 = value; }
        public bool IsManager { get => isManager; set => isManager = value; }
        public string Password { get => password; set => password = value; }
        internal Project Project { get => project; set => project = value; }

        public Employee(string email, string firstName, string middleName, string lastName, DateTime birthday, string adress1, string adress2,string password)
        {
            Email = email; 
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Birthday = birthday;
            Adress1 = adress1;
            Adress2 = adress2;
            Password = password;
        }
    }
    class Status // Project status
    {
        private string status;
    }

    class ProjectType
    {
        private string type;
    }

    class Project
    {
        private string pjName;
        private string prjNo;
        private Employee pjManager;
        private string purpose;
        private DateTime DatReg;
        private DateTime DatStart;
        private DateTime DatEnd;
        private DateTime probDatStart;
        private DateTime probDatEnd;
        private Status pjStatus;
        private float monret;
        private string monrettype;
        private string[] Team;

        private TreeView Tasks_Miles;

        public Project(string Name, Employee Manager, string Purpose, DateTime start, DateTime end)
        {
            PjName = Name;
            PjManager = Manager;
            Purpose = Purpose;
            ProbDatStart = start;
            ProbDatStart = end;
            PrjNo = "PRJ";
            DatReg1 = DateTime.Now;
        }

        public string PjName { get => pjName; set => pjName = value; }
        public string PrjNo { get => prjNo; set => prjNo = value; }
        public string Purpose { get => purpose; set => purpose = value; }
        public DateTime DatReg1 { get => DatReg; set => DatReg = value; }
        public DateTime DatStart1 { get => DatStart; set => DatStart = value; }
        public DateTime DatEnd1 { get => DatEnd; set => DatEnd = value; }
        public DateTime ProbDatStart { get => probDatStart; set => probDatStart = value; }
        public DateTime ProbDatEnd { get => probDatEnd; set => probDatEnd = value; }
        public float Monret { get => monret; set => monret = value; }
        public string Monrettype { get => monrettype; set => monrettype = value; }
        public string[] Team1 { get => Team; set => Team = value; }
        public TreeView Tasks_Miles1 { get => Tasks_Miles; set => Tasks_Miles = value; }
        internal Employee PjManager { get => pjManager; set => pjManager = value; }
        internal Status PjStatus { get => pjStatus; set => pjStatus = value; }
    }
}
