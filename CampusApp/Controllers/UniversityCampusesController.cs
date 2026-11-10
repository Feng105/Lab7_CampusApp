using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusApp.Data;
using CampusApp.Models;

namespace CampusApp.Controllers
{
    public class UniversityCampusesController : Controller
    {
        private CampusContext db = new CampusContext();

        // GET: UniversityCampuses
        public ActionResult Index()
        {
            return View(db.UniversityCampuses.ToList());
        }

        // GET: UniversityCampuses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UniversityCampus campus = db.UniversityCampuses.Find(id);
            if (campus == null)
            {
                return HttpNotFound();
            }
            return View(campus);
        }

        // GET: UniversityCampuses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UniversityCampuses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] UniversityCampus campus)
        {
            if (ModelState.IsValid)
            {
                db.UniversityCampuses.Add(campus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(campus);
        }

        // GET: UniversityCampuses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UniversityCampus campus = db.UniversityCampuses.Find(id);
            if (campus == null)
            {
                return HttpNotFound();
            }
            return View(campus);
        }

        // POST: UniversityCampuses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] UniversityCampus campus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(campus);
        }

        // GET: UniversityCampuses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UniversityCampus campus = db.UniversityCampuses.Find(id);
            if (campus == null)
            {
                return HttpNotFound();
            }
            return View(campus);
        }

        // POST: UniversityCampuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UniversityCampus campus = db.UniversityCampuses.Find(id);
            db.UniversityCampuses.Remove(campus);
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
