using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Views;

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
            var testers = (from t in context.Testers
                           join b in context.Bugs on t.TesterId equals b.OwnerId
                           join p in context.Projects on b.ProjectId equals p.ProjectId
                           group b by b.BugId
                           ).ToList();

            return View(testers);
        }

    }
}
