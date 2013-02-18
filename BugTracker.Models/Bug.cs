using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Models
{
    public enum BugPriority
    {
        Crirical,
        High,
        Normal,
        Low
    }

    public enum BugStatus
    {
        New,
        InProgress,
        Fixed,
        Closed,
        Deleted
    }

    public class Bug
    {
        public int BugId { get; set; }
        public DateTime DateFound { get; set; }
        public int OwnerId { get; set; }
        public string Description { get; set; }
        public BugPriority Priority { get; set; }
        public int ProjectId { get; set; }
        public BugStatus Status { get; set; }
    }
}
