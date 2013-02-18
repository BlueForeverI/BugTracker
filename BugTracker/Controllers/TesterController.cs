using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using WebMatrix.WebData;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using BugTracker.Views;

namespace BugTracker.Controllers
{
    public class TesterController : Controller
    {
        private ModelContext context = new ModelContext();
        //
        // GET: /Tester/

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ViewBugs()
        {
            var projects = context.Projects.ToList();

            return View(projects);
        }

        [Authorize]
        public ActionResult GetBugs(int id)
        {
            var bugs = (from b in context.Bugs
                        join t in context.Testers on b.OwnerId equals t.TesterId
                        join p in context.Projects on b.ProjectId equals p.ProjectId
                        where b.ProjectId == id
                        select new
                        {
                            DateFound = b.DateFound,
                            Description = b.Description,
                            Owner = t.Username,
                            Priority = b.Priority,
                            Project = p.Name,
                            Status = b.Status
                        })
                        .AsEnumerable()
                        .Select(x => new BugView() { 
                            DateFound = x.DateFound.ToString(),
                            Description = x.Description,
                            Owner = x.Owner,
                            Priority = x.Priority.ToString(),
                            Project = x.Project,
                            Status = x.Status.ToString()
                        })
                        .ToList();

            return Json(bugs, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddBug()
        {
            var projects = context.Projects.ToList();

            return View(projects);
        }

        [Authorize]
        public ActionResult DoAddBug(Bug bug)
        {
            bug.DateFound = DateTime.Now;
            bug.Status = BugStatus.New;
            bug.OwnerId = WebSecurity.CurrentUserId;

            context.Bugs.Add(bug);
            var tester = context.Testers
                .Where(t => t.TesterId == WebSecurity.CurrentUserId)
                .FirstOrDefault();

            tester.LastAction = "Added new bug";
            tester.LastActionDate = DateTime.Now;
            context.SaveChanges();

            return View("BugAdded");
        }

        [Authorize]
        public ActionResult EditBug()
        {
            var bugs = context.Bugs.Where(b => b.Status != BugStatus.Deleted)
                .ToList();

            return View(bugs);
        }

        [Authorize]
        public ActionResult Bugs(int id)
        {
            var bug = context.Bugs.Where(b => b.BugId == id && b.Status != BugStatus.Deleted)
                .FirstOrDefault();

            return Json(bug, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult DoEditBug(Bug bug)
        {
            var entity = context.Bugs
                .Where(b => b.BugId == bug.BugId).FirstOrDefault();

            entity.Description = bug.Description;
            entity.Priority = bug.Priority;
            entity.Status = bug.Status;

            var tester = context.Testers
                .Where(t => t.TesterId == WebSecurity.CurrentUserId)
                .FirstOrDefault();

            tester.LastAction = string.Format("Edited bug #{0}", bug.BugId);
            tester.LastActionDate = DateTime.Now;
            context.SaveChanges();

            return View("BugEdited");
        }

        [Authorize]
        public ActionResult DeleteBug()
        {
            var bugs = context.Bugs.Where(b => b.Status != BugStatus.Deleted).ToList();

            return View(bugs);
        }

        [Authorize]
        public ActionResult DoDeleteBug(int BugId)
        {
            var entity = context.Bugs.Where(b => b.BugId == BugId)
                .FirstOrDefault();

            entity.Status = BugStatus.Deleted;

            var tester = context.Testers
                .Where(t => t.TesterId == WebSecurity.CurrentUserId)
                .FirstOrDefault();

            tester.LastAction = string.Format("Deleted bug #{0}", BugId);
            tester.LastActionDate = DateTime.Now;
            context.SaveChanges();

            return View("BugDeleted");
        }

    }
}
