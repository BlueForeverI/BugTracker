using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using WebMatrix.WebData;
using System.Web.Security;

namespace BugTracker.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();

            return Redirect("../");
        }

        public ActionResult DoLogin(string Username, string Password)
        {
            if (WebSecurity.Login(Username, Password))
            {
                return View("LoginSuccess");
            }

            return View("Login");
        }
    }
}
