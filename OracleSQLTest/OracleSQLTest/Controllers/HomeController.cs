using OracleSQLTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Diagnostics;

namespace OracleSQLTest.Controllers
{
    public string uid = "";
    public class HomeController : Controller
    {
        private CRIDBAEntities db = new CRIDBAEntities();
        public ActionResult Index()
        {

            string id = "";
            if (this.Request.ClientCertificate.IsPresent)
            {
                try
                {
                    id = ReadCert();
                }
                catch (Exception ex)
                {
                    Response.Write(string.Format("An error occured:"));
                    Response.Write(string.Format(ex.Message));
                    Response.Write(string.Format(ex.StackTrace));
                }
            }

            if (string.IsNullOrEmpty(id))
            {
                return View("CertError");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<CRIS_USER> list = (from a in db.EMPLOYEEs
                                    join c in db.CRIS_USER on a.USER_ID equals c.USER_ID
                                    where (a.EMPL_UID.Equals(id))
                                    select c).ToList();
            ViewBag.crisUser = list;
            return View();
        }

        public string ReadCert()
        {
            string oid = "OID";
            string cn = "CN";
            string uscis = ".USCIS";

            X509Certificate2 x509Cert2 = new X509Certificate2(this.Request.ClientCertificate.Certificate);
            string id = string.Format(x509Cert2.Subject);

            //CN = ANIL K DAS (affiliate) + OID.0.9.2342.19200300.100.1.1 = 0652858337.USCIS, OU = People, OU = USCIS, OU = Department of Homeland Security, O = U.S.Government, C = US
            /*
            1. Get CN and read until + to get the Name
            2. Get OID. 
            3. Get the next token after = and until space
            3. This token is the UID + USCIS
            4. Suppress USCIS from UID
            */

            //Get UID
            int position1 = id.IndexOf(oid);
            int position2 = id.IndexOf(uscis);

            string str1 = id.Substring(position1, position2 - position1);
            char[] delimiterChars = { '=' };
            string uid = str1.Replace(".USCIS", "").Split(delimiterChars).Last().Trim();
            HttpCookie uidCookie = new HttpCookie("uid");
            uidCookie.HttpOnly = true;
            uidCookie.Expires = DateTime.Now.AddMinutes(15.0);
            uidCookie.Value = uid;
            this.ControllerContext.HttpContext.Response.Cookies.Add(uidCookie);

            //Get Name
            position1 = id.IndexOf(cn);
            str1 = id.Substring(position1, id.Length - position1);
            position1 = str1.IndexOf("=");
            position2 = str1.IndexOf("+");
            int position3 = str1.IndexOf(",");
            if (position2 > 0 && position3 > 0)
            {
                if (position3 < position2)
                {
                    position2 = position3;
                }
            }
            else {
                if (position2 < 0)
                    position2 = position3;
            }

            string str2 = str1.Substring(position1 + 1, position2 - position1-1);
            ViewBag.UserName = str2.Replace("(affiliate)", "").Trim()+"("+ uid + ")";

            //char[] delimiterChars = { ',', '=', '\t' };
            //string[] words = subject.Split(delimiterChars);
            //ViewBag.UserName = subject;
            //string uidString = words[2];
            //char[] uiddelimiter = { '.', '\t' };
            //string[] uidparts = uidString.Split(uiddelimiter);
            //string uid = uidparts[0];

            return uid;
        }

        // GET: employees/Edit/5
        public ActionResult Detail(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpCookie username = new HttpCookie("username");
            username.HttpOnly = true;
            username.Expires = DateTime.Now.AddMinutes(15.0);
            username.Value = id;
            this.ControllerContext.HttpContext.Response.Cookies.Add(username);
            return Redirect("/cris/employee/logonEmployee.do");

            //return View(employee);
        }
    }
}
