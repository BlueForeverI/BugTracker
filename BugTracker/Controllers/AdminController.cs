﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var validationErrors = GetTesterValidationErrors(tester);
            if (validationErrors.Count > 0)
            {
                return View("InvalidTester", validationErrors);
            }

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
            var validationErrors = GetTesterValidationErrors(tester);
            if (validationErrors.Count > 0)
            {
                return View("InvalidTester", validationErrors);
            }

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
            var errors = GetProjectValidationErrors(project);
            if (errors.Count > 0)
            {
                return View("InvalidProject", errors);
            }

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
            var errors = GetProjectValidationErrors(project);
            if (errors.Count > 0)
            {
                return View("InvalidProject", errors);
            }

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

        private List<string> GetTesterValidationErrors(Tester tester)
        {
            List<string> errors = new List<string>();

            if (context.Testers.Any(t => t.Username == tester.Username))
            {
                errors.Add("Tester already exists");
            }

            if (string.IsNullOrEmpty(tester.Username) || string.IsNullOrWhiteSpace(tester.Username))
            {
                errors.Add("The username should not be empty");
            }
            else if (tester.Username.Length < 3)
            {
                errors.Add("The usernane should be at least 3 characters");
            }
            else if (!Regex.IsMatch(tester.Username, "[A-Za-z][A-Za-z0-9]*"))
            {
                errors.Add("The username should start with a letter and contain only letters and numbers");
            }

            if (string.IsNullOrEmpty(tester.Password) || string.IsNullOrWhiteSpace(tester.Password))
            {
                errors.Add("The password should not be empty");
            }
            else if (tester.Password.Length < 3)
            {
                errors.Add("The password should be at least 3 characters");
            }

            if(string.IsNullOrEmpty(tester.Email) || string.IsNullOrWhiteSpace(tester.Email))
            {
                errors.Add("The email should not be empty");
            }
            else if (!Regex.IsMatch(tester.Email, "[A-Za-z][A-Za-z0-9_]+@[A-Za-z0-9][A-Za-z0-9_-]*\\.[A-Za-z]+"))
            {
                errors.Add("Invalid email format");
            }

            if (string.IsNullOrEmpty(tester.FirstName) || string.IsNullOrWhiteSpace(tester.FirstName))
            {
                errors.Add("The first name should not be empty");
            }

            if (string.IsNullOrEmpty(tester.LastName) || string.IsNullOrWhiteSpace(tester.LastName))
            {
                errors.Add("The last name should not be empty");
            }

            if (string.IsNullOrEmpty(tester.Telephone) || string.IsNullOrWhiteSpace(tester.Telephone))
            {
                errors.Add("The telephone should not be empty");
            }

            return errors;
        }

        private List<string> GetProjectValidationErrors(Project project)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(project.Name) || string.IsNullOrWhiteSpace(project.Name))
            {
                errors.Add("The project name should not be empty");
            }

            if (string.IsNullOrEmpty(project.Description) || string.IsNullOrWhiteSpace(project.Description))
            {
                errors.Add("The description should not be empty");
            }

            return errors;
        }
    }
}
