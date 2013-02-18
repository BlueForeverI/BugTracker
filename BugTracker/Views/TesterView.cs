using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Views
{
    public class TesterView
    {
        public string Username { get; set; }
        public int ProjectCount { get; set; }
        public int BugCount { get; set; }
        public string LastActionDate { get; set; }
        public string LastAction { get; set; }
    }
}