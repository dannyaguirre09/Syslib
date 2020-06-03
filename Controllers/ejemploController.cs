using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SysLibrary.Models;

namespace SysLibrary.Controllers
{
    public class ejemploController : Controller
    {
        private DbModel db = new DbModel();

        // GET: ejemplo
        public ActionResult Index()
        {
            var book = db.book.Include(b => b.category);
            return View(book.ToList());
        }

        // GET: ejemplo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = db.book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: ejemplo/Create
        public ActionResult Create()
        {
            ViewBag.IdCategory = new SelectList(db.category, "IdCategory", "NameCategory");
            return View();
        }

        // POST: ejemplo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdBook,IdCategory,NameBook,DescriptionBook,PublicationDateBook,IsActive")] book book)
        {
            if (ModelState.IsValid)
            {
                db.book.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCategory = new SelectList(db.category, "IdCategory", "NameCategory", book.IdCategory);
            return View(book);
        }

        // GET: ejemplo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = db.book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCategory = new SelectList(db.category, "IdCategory", "NameCategory", book.IdCategory);
            return View(book);
        }

        // POST: ejemplo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdBook,IdCategory,NameBook,DescriptionBook,PublicationDateBook,IsActive")] book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCategory = new SelectList(db.category, "IdCategory", "NameCategory", book.IdCategory);
            return View(book);
        }

        // GET: ejemplo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = db.book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: ejemplo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            book book = db.book.Find(id);
            db.book.Remove(book);
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
