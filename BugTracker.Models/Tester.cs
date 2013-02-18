using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace BugTracker.Models
{
    public class Tester
    {
        public Tester()
        {
            this.Bugs = new HashSet<Bug>();
            this.Projects = new HashSet<Project>();
        }
        
        public int TesterId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string LastAction { get; set; }
        public DateTime? LastActionDate { get; set; }

        public ICollection<Bug> Bugs { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
