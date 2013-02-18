using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Views;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace BugTracker.Controllers
{
    public class ReferencesController : Controller
    {
        private ModelContext context = new ModelContext();

        //
        // GET: /References/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ShowTesters()
        {
            return View(GetTesters());
        }

        [Authorize]
        private List<TesterView> GetTesters()
        {
            var testers = (from t in context.Testers.Include("Projects").Include("Bugs")
                           select new
                           {
                               BugCount = t.Bugs.Count,
                               LastAction = t.LastAction,
                               LastActionDate = t.LastActionDate,
                               ProjectCount = t.Projects.Count,
                               Username = t.Username
                           })
                           .AsEnumerable()
                           .Select(x => new TesterView()
                           {
                               Username = x.Username,
                               ProjectCount = x.ProjectCount,
                               LastActionDate = x.LastActionDate.ToString(),
                               LastAction = x.LastAction,
                               BugCount = x.BugCount
                           }).ToList(); ;

            return testers;
        }

        [Authorize]
        public ActionResult Testers_Read([DataSourceRequest] DataSourceRequest request)
        {
            request.PageSize = 15;

            return Json(GetTesters().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ShowBugs()
        {
            return View(GetBugs());
        }

        [Authorize]
        private List<BugView> GetBugs()
        {
            var bugs = (from b in context.Bugs
                        join t in context.Testers on b.TesterId equals t.TesterId
                        join p in context.Projects on b.ProjectId equals p.ProjectId
                        where b.Status != BugStatus.Closed && b.Status != BugStatus.Deleted
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
                        .Select(x => new BugView
                        {
                            DateFound = x.DateFound.ToString(),
                            Description = (x.Description.Length > 50) 
                                            ? x.Description.Substring(0, 50)
                                            : x.Description,
                            Owner = x.Owner,
                            Priority = x.Priority.ToString(),
                            Project = x.Project,
                            Status = x.Status.ToString()
                        }).ToList();

            return bugs;
        }

        [Authorize]
        public ActionResult Bugs_Read([DataSourceRequest] DataSourceRequest request)
        {
            request.PageSize = 15;

            return Json(GetBugs().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ShowBugsByProject()
        {
            var projects = context.Projects.ToList();

            return View(projects);
        }

        [Authorize]
        public ActionResult GetBugsByProject(int id)
        {
            var bugs = (from b in context.Bugs
                        join t in context.Testers on b.TesterId equals t.TesterId
                        join p in context.Projects on b.ProjectId equals p.ProjectId
                        where b.Status != BugStatus.Closed && b.Status != BugStatus.Deleted
                            && p.ProjectId == id
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
                        .Select(x => new BugView
                        {
                            DateFound = x.DateFound.ToString(),
                            Description = (x.Description.Length > 50)
                                            ? x.Description.Substring(0, 50)
                                            : x.Description,
                            Owner = x.Owner,
                            Priority = x.Priority.ToString(),
                            Project = x.Project,
                            Status = x.Status.ToString()
                        }).ToList();

            return Json(bugs, JsonRequestBehavior.AllowGet);
        }

    }
}
