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
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index(book objBook = null)
        {
			using(DbModel db = new DbModel())
			{
				if (objBook.NameBook == null)
					ViewBag.list = db.book.Include(b => b.category).Where(b=> b.IsActive==1).ToList();
				else
					ViewBag.list = db.book.Include(b => b.category).Where(b => b.IsActive == 1 && b.NameBook.Contains(objBook.NameBook)).ToList();
			}

			ViewBag.Count = ViewBag.list.Count;

            return View();
        }

		public ActionResult Add()
		{
			DbModel db = new DbModel();
			ViewBag.IdCategory = new SelectList(db.category.Where(d => d.IsActive == 1), "IdCategory", "NameCategory");

			return View(new book());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(book model)
		{
			try
			{
				DbModel db = new DbModel();

				if (ModelState.IsValid)
				{
					model.IsActive = 1;
					db.book.Add(model);
					db.SaveChanges();
					Request.Flash("success", "Add Success");
					return Redirect("~/book");
				}

				ViewBag.IdCategory = new SelectList(db.category.Where(d => d.IsActive == 1), "IdCategory", "NameCategory");
				return View(model);
			}
			catch(Exception ex)
			{
				Request.Flash("error", ex.Message);
				return Redirect("~/book");
			}
		}

		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			book model = new book();
			DbModel db = new DbModel();
			model = db.book.Find(id);
			ViewBag.IdCategory = new SelectList(db.category.Where(d => d.IsActive == 1), "IdCategory", "NameCategory", model.IdCategory);
			if (model == null)
			{
				return HttpNotFound();
			}
				
			
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(book model)
		{
			try
			{
				DbModel db = new DbModel();
				if (ModelState.IsValid)
				{		
					db.Entry(model).State = EntityState.Modified;
					db.SaveChanges();
					Request.Flash("success", "Edit Success");
					return Redirect("~/Book");
				}

				ViewBag.IdCategory = new SelectList(db.category.Where(d => d.IsActive == 1), "IdCategory", "NameCategory");
				return View(model);
			}
			catch (Exception ex)
			{
				Request.Flash("error", ex.Message);
				return Redirect("~/book");
			}
		}

		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			book objBook = new book();
			using(DbModel db = new DbModel())
			{
				objBook = db.book.Find(id);
			}

			if (objBook == null)
			{
				return HttpNotFound();
			}
			return View(objBook);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(book model)
		{
			try
			{
				DbModel db = new DbModel();
				if (ModelState.IsValid)
				{
					db.Entry(model).State = EntityState.Modified;
					db.SaveChanges();
					Request.Flash("success", "Deleted Success");
					return Redirect("~/Book");
				}

				return View(model);
			}
			catch (Exception ex)
			{
				Request.Flash("error", ex.Message);
				return Redirect("~/book");
			}
		}
	}
}