using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace BugTracker.Models
{
    public class Project
    {
        public Project()
        {
            this.Bugs = new HashSet<Bug>();
            this.Testers = new HashSet<Tester>();   
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Bug> Bugs { get; set; }
        public ICollection<Tester> Testers { get; set; }
    }
}
