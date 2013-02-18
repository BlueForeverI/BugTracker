using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Views
{
    public class BugView
    {
        public string Description  { get; set; }
        public string Owner { get; set; }
        public string Priority { get; set; }
        public string DateFound { get; set; }
        public string Project { get; set; }
        public string Status { get; set; }
    }
}