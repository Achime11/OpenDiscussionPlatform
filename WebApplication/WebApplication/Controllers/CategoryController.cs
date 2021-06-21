using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using WebApplication.DataAccessLayer;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CategoryController : Controller
    {
        private DatabaseContext Database = new DatabaseContext();

        // GET: Category
        public ActionResult Index()
        {
            ViewBag.Categories = Database.Categories;
            return View();
        }

        public ActionResult Show(int id,int page=0) {
            int items_per_page = 5;

            //https://docs.microsoft.com/en-us/ef/ef6/querying/related-data#eagerly-loading
            var category = Database.Categories.Where(item => item.Id == id).Include(item => item.Discussions.Select(item2 => item2.User)).SingleOrDefault();
            var discussions = category.Discussions.Skip(page*items_per_page).Take(items_per_page);

            ViewBag.Page = page;
            ViewBag.CountPages = 1+category.Discussions.Count() / items_per_page;
            ViewBag.Category = category;
            ViewBag.Discussions = discussions;

            return View();  
        }

        public ActionResult New() {
            return View();  // Nu avem ce sa-i pasam. Nu avem de reprezentat niciun obiect concret ci doar forma
        }

        [HttpPost]
        public ActionResult New(Category obj) {
            try {
                if (ModelState.IsValid) {
                    Database.Categories.Add(obj);
                    Database.SaveChanges();

                    TempData["message"] = "Category "+obj.Name+" added succesfully!";
                    return RedirectToAction("Index");
                } else {
                    return View();
                }
            }catch (Exception) {
                return View();
            }
        }


        public ActionResult Update(int id) {
            Category entry = Database.Categories.Find(id);
            return View(entry);
        }
        
        [HttpPost]
        public ActionResult Update(int id,Category obj) {
            try {
                if (ModelState.IsValid) {
                    Category entry = Database.Categories.Find(id);
                    entry.Description = obj.Description;
                    entry.Name = obj.Name;

                    Database.SaveChanges();
                    TempData["message"] = "Category "+entry.Name+" was updated!";

                    return RedirectToAction("Index");
                } else {
                    return View(obj);
                }
            } catch (Exception) {
                return View(obj);
            }
        }

        public ActionResult Delete(int Id) {
            Category entry = Database.Categories.Find(Id);
            Database.Categories.Remove(entry);
            Database.SaveChanges();
            TempData["message"] = "Category " + entry.Name + " succesfully deleted!";
            TempData["warning"] = "";
            return RedirectToAction("Index");
        }
    }
}