using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusApp.Data;
using CampusApp.Models;
using CampusApp.ViewModels;

namespace CampusApp.Controllers
{
    public class StudentsController : Controller
    {
        private CampusContext db = new CampusContext();

        // GET: Students
        public ActionResult Index(string campus, string search)
        {
            StudentIndexViewModel viewModel = new StudentIndexViewModel();

            var students = db.Students.Include(s => s.Campus);

            if (!String.IsNullOrEmpty(search))
            {
                students = students.Where(s => s.Name.Contains(search) ||
                                               s.Address.Contains(search) ||
                                               s.Campus.Name.Contains(search));
                viewModel.Search = search;
            }

            //count the students per campus before the campus filter removes any rows
            viewModel.CampusesWithCount = from s in students
                                          where s.CampusID != null
                                          group s by s.Campus.Name into campusGroup
                                          select new CampusWithCount
                                          {
                                              CampusName = campusGroup.Key,
                                              StudentCount = campusGroup.Count()
                                          };

            if (!String.IsNullOrEmpty(campus))
            {
                students = students.Where(s => s.Campus.Name == campus);
            }

            viewModel.Students = students.OrderBy(s => s.Campus.Name).ThenBy(s => s.Name).ToList();
            viewModel.Campus = campus;

            return View(viewModel);
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.CampusID = new SelectList(db.UniversityCampuses, "ID", "Name");
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Address,CampusID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampusID = new SelectList(db.UniversityCampuses, "ID", "Name", student.CampusID);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampusID = new SelectList(db.UniversityCampuses, "ID", "Name", student.CampusID);
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,CampusID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CampusID = new SelectList(db.UniversityCampuses, "ID", "Name", student.CampusID);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
