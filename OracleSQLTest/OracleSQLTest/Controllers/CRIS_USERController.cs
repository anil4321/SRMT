using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OracleSQLTest.Models;

namespace OracleSQLTest.Controllers
{
    public class CRIS_USERController : Controller
    {
        private ProfileEntities db = new ProfileEntities();

        // GET: CRIS_USER
        public ActionResult Index()
        {
            return View(db.CRIS_USER.ToList());
        }

        // GET: CRIS_USER/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRIS_USER cRIS_USER = db.CRIS_USER.Find(id);
            if (cRIS_USER == null)
            {
                return HttpNotFound();
            }
            return View(cRIS_USER);
        }

        // GET: CRIS_USER/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CRIS_USER/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USER_ID,LOGIN_NAME,USER_PASSWORD,USER_EMAIL,RETRY_ATTEMPTS,USER_STATUS,CREATED_BY,CREATED_DATE,LAST_UPDATE_BY,LAST_UPDATE_DATE,PASSWORD_CHANGE_DATE,PASSWORD_EXP_DATE,USER_QUESTION,USER_ANSWER,ENC_DEC_KEY")] CRIS_USER cRIS_USER)
        {
            if (ModelState.IsValid)
            {
                db.CRIS_USER.Add(cRIS_USER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cRIS_USER);
        }

        // GET: CRIS_USER/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRIS_USER cRIS_USER = db.CRIS_USER.Find(id);
            if (cRIS_USER == null)
            {
                return HttpNotFound();
            }
            return View(cRIS_USER);
        }

        // POST: CRIS_USER/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USER_ID,LOGIN_NAME,USER_PASSWORD,USER_EMAIL,RETRY_ATTEMPTS,USER_STATUS,CREATED_BY,CREATED_DATE,LAST_UPDATE_BY,LAST_UPDATE_DATE,PASSWORD_CHANGE_DATE,PASSWORD_EXP_DATE,USER_QUESTION,USER_ANSWER,ENC_DEC_KEY")] CRIS_USER cRIS_USER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cRIS_USER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cRIS_USER);
        }

        // GET: CRIS_USER/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRIS_USER cRIS_USER = db.CRIS_USER.Find(id);
            if (cRIS_USER == null)
            {
                return HttpNotFound();
            }
            return View(cRIS_USER);
        }

        // POST: CRIS_USER/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            CRIS_USER cRIS_USER = db.CRIS_USER.Find(id);
            db.CRIS_USER.Remove(cRIS_USER);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
