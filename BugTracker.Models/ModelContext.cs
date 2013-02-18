using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class ModelContext : DbContext 
    {
        public ModelContext()
            : base("DefaultConnection")
        {
 
        }

        public DbSet<Tester> Testers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Bug> Bugs { get; set; }
    }
}
