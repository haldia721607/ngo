using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ngo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Services()
        {
            return View();
        }
        public ActionResult EventDetail()
        {
            return View();
        }
        public ActionResult Events()
        {
            return View();
        }
        public ActionResult Blog()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authentic()
        {
            if (Request.Form["Email"].ToString()=="" && Request.Form["Email"].ToString() == "")
            {
                Session["UserName"] = "Ravi Shanker Pandey";
                Session["UserId"] = 1;
                Session["UserType"] = "Admin";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Msg = "Check Email Id And Password";
                return View("~/Views/Home/Login.cshtml");
            }
        }
    }
}