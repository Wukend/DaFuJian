using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Dafujian.Manage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Convert the IIdentity to a ClaimsIdentity
            ClaimsIdentity Identity = new ClaimsIdentity(Thread.CurrentPrincipal.Identity);

            ViewBag.Name = Thread.CurrentPrincipal.Identity.Name;
            ViewBag.Identity = Identity;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}