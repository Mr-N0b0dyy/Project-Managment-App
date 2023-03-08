using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Project_Managment_vol_2.Contexts
{
     class CodeFirstContext:DbContext

    {
        public DbSet<User> Users { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<MileStone> MileStones { get; set; }
        public DbSet<Task> tasks { get; set; }
        public DbSet<Project> projects { get; set; }
        
     }
}
