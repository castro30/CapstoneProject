using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapstoneProject.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            ViewBag.Title = "Russian";
            ViewBag.Example = "Plusee";
            ViewBag.Number = 75;
            ViewBag.YesNo = false;

            return View();
        }
    }
}