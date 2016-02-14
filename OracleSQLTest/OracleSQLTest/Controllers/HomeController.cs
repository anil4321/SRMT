using OracleSQLTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

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
        public string ReadCert()
        {
            X509Certificate2 x509Cert2 = new X509Certificate2(this.Request.ClientCertificate.Certificate);
            Debug.WriteLine(string.Format("Issued To:"));
            Debug.WriteLine(string.Format(x509Cert2.Subject));

            Debug.WriteLine(string.Format("Issued By:"));
            Debug.WriteLine(string.Format(x509Cert2.Issuer));

            Debug.WriteLine(string.Format("Friendly Name:"));
            Debug.WriteLine(string.Format(string.IsNullOrEmpty(x509Cert2.FriendlyName) ? "(None Specified)" : x509Cert2.FriendlyName));

            Debug.WriteLine(string.Format("Valid Dates:"));
            Debug.WriteLine(string.Format("From: " + x509Cert2.GetEffectiveDateString()));
            Debug.WriteLine(string.Format("To: " + x509Cert2.GetExpirationDateString()));

            Debug.WriteLine(string.Format("Thumbprint:"));
            Debug.WriteLine(string.Format(x509Cert2.Thumbprint));

            //Debug.WriteLine(string.Format( "Public Key:"));
            //Debug.WriteLine(string.Format( x509Cert2.GetPublicKeyString()));

            #region EKU Section - Retrieve EKU info and write out each OID
            X509EnhancedKeyUsageExtension ekuExtension = (X509EnhancedKeyUsageExtension)x509Cert2.Extensions["Enhanced Key Usage"];
            if (ekuExtension != null)
            {
                Debug.WriteLine(string.Format("Enhanced Key Usages (" + ekuExtension.EnhancedKeyUsages.Count.ToString() + " found)"));

                OidCollection ekuOids = ekuExtension.EnhancedKeyUsages;
                foreach (Oid ekuOid in ekuOids)
                    Debug.WriteLine(string.Format(ekuOid.FriendlyName + " (OID: " + ekuOid.Value + ")"));
            }
            else
            {
                Debug.WriteLine(string.Format("No EKU Section Data"));
            }
            #endregion // EKU Section

            #region Subject Alternative Name Section
            X509Extension sanExtension = (X509Extension)x509Cert2.Extensions["Subject Alternative Name"];
            if (sanExtension != null)
            {
                Debug.WriteLine(string.Format("Subject Alternative Name:"));
                Debug.WriteLine(string.Format(sanExtension.Format(true)));
            }
            else
            {
                Debug.WriteLine(string.Format("No Subject Alternative Name Data"));
            }

            #endregion // Subject Alternative Name Section

            #region Certificate Policies Section
            X509Extension policyExtension = (X509Extension)x509Cert2.Extensions["Certificate Policies"];
            if (policyExtension != null)
            {
                Debug.WriteLine(string.Format("Certificate Policies:"));
                Debug.WriteLine(string.Format(policyExtension.Format(true)));
            }
            else
            {
                Debug.WriteLine(string.Format("No Certificate Policies Data"));
            }
            #endregion //Certificate Policies Section
            return "uid";
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
