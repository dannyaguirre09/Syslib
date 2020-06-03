using SysLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SysLibrary.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index(category objCategory = null)
        {
			using(DbModel db = new DbModel())
			{
				if (objCategory.NameCategory == null)
					ViewBag.list = db.category.Where(x => x.IsActive == 1).ToList();
				else
					ViewBag.list = db.category.Where(x => x.IsActive == 1 && x.NameCategory.Contains(objCategory.NameCategory)).ToList();

			}
            return View();
        }

		public ActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Add(category model)
		{
			try
			{
				if(ModelState.IsValid)
				{
					using(DbModel db = new DbModel())
					{
						model.IsActive = 1;
						db.category.Add(model);
						db.SaveChanges();
						Request.Flash("success", "Add Success");
					}
					return Redirect("/Category");
				}

				return View(model);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public ActionResult Edit(int Id)
		{
			category model = new category();
			using (DbModel db = new DbModel())
			{
				model = db.category.Find(Id);
			}
			return View(model);
		}

		[HttpPost]
		public ActionResult Edit(category model)
		{
			try
			{
				if(ModelState.IsValid)
				{
					using (DbModel db = new DbModel() )
					{
						db.Entry(model).State = EntityState.Modified;
						db.SaveChanges();
						Request.Flash("success", "Edit Success");
					}
					return Redirect("~/Category");
				}

				return View(model);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public ActionResult Delete(int Id)
		{
			category objCategory = new category();
			using(DbModel db = new DbModel())
			{
				objCategory = db.category.Find(Id);
			}
			return View(objCategory);
		}

		[HttpPost]
		public ActionResult Delete(category model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					using (DbModel db = new DbModel())
					{
						var objbook = db.book.Where(x => x.IdCategory == model.IdCategory && x.IsActive == 1).ToList();
						if (objbook.Count != 0)
						{
							Request.Flash("warning", "No es posible eliminar porque hay libros asociados a esta categoría");
							return Redirect("~/Category");
						}
						db.Entry(model).State = EntityState.Modified;
						db.SaveChanges();
						Request.Flash("success", "Deleted Success");
					}
					return Redirect("~/Category");
				}

				return View(model);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

	}
}