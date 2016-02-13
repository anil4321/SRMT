using OracleSQLTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OracleSQLTest.Controllers
{
    public class HomeController : Controller
    {
        private ProfileEntities db = new ProfileEntities();
        public ActionResult Index()
        {
            string id = "11";
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //List<EMPLOYEE> list = (from a in db.EMPLOYEEs
            //                        select a).ToList();

            List<CRIS_USER> list = (from a in db.EMPLOYEEs
                                    join c in db.CRIS_USER on a.USER_ID equals c.USER_ID
                                    where (a.EMPL_UID.Equals(id))
                                    select c).ToList();
            ViewBag.crisUser = list;

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