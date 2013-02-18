using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using WebMatrix.WebData;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        private ModelContext context = new ModelContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddTester()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DoAddTester(Tester tester)
        {
            string password = tester.Password;
            tester.Password = "";
            context.Testers.Add(tester);
            context.SaveChanges();

            WebSecurity.CreateAccount(tester.Username, password);

            return View("TesterAdded");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditTester()
        {
            var testers = context.Testers.ToList();

            return View(testers);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DoEditTester(Tester tester)
        {
            Tester entity = context.Testers.Where(t => t.TesterId == tester.TesterId).FirstOrDefault();

            string resetToken = WebSecurity.GeneratePasswordResetToken(entity.Username);
            WebSecurity.ResetPassword(resetToken, tester.Password);

            entity.FirstName = tester.FirstName;
            entity.LastName = tester.LastName;
            entity.Email = tester.Email;
            entity.Telephone = tester.Telephone;
            context.SaveChanges();

            return View("TesterEdited");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Testers(int id)
        {
            Tester tester = context.Testers.Where(x => x.TesterId == id).FirstOrDefault();

            return Json(tester, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveTester()
        {
            var testers = context.Testers.ToList();

            return View(testers);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DoRemoveTester(int TesterId)
        {
            Tester tester = context.Testers.Where(t => t.TesterId == TesterId).FirstOrDefault();
            context.Testers.Remove(tester);
            context.SaveChanges();

            return View("TesterRemoved");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddProject()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DoAddProject(Project project)
        {
            context.Projects.Add(project);
            context.SaveChanges();

            return View("ProjectAdded");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditProject()
        {
            var projects = context.Projects.ToList();

            return View(projects);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Projects(int id)
        {
            Project project = context.Projects.Where(p => p.ProjectId == id).FirstOrDefault();

            return Json(project, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DoEditProject(Project project)
        {
            Project entity = context.Projects
                .Where(p => p.ProjectId == project.ProjectId).FirstOrDefault();

            entity.Name = project.Name;
            entity.Description = project.Description;
            context.SaveChanges();

            return View("ProjectEdited");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveProject()
        {
            var projects = context.Projects.ToList();

            return View(projects);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DoRemoveProject(int ProjectId)
        {
            Project project = context.Projects
                .Where(p => p.ProjectId == ProjectId).FirstOrDefault();

            context.Projects.Remove(project);
            context.SaveChanges();

            return View("ProjectRemoved");
        }
    }
}
