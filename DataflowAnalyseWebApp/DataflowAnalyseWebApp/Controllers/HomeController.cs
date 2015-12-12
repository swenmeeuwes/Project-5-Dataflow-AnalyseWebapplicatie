using DataflowAnalyseWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DataflowAnalyseWebApp.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewBag.Title = "Home Page";
            return View();
        }

        public ActionResult UnitInformation() {
            UnitInformation unit = new UnitInformation();
            List<Position> data = unit.GetData();
            ViewBag.Data = data;
            return View();
        }
    }
}
