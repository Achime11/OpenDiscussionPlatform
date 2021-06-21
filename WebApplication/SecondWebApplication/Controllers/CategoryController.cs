using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using SecondWebApplication.Helpers;
using SecondWebApplication.Models;

namespace SecondWebApplication.Controllers {
    public class CategoryController : Controller {
        private ApplicationDbContext Database = new ApplicationDbContext();

        // GET: Category
        public ActionResult Index() {
            ViewBag.Categories = Database.Categories;
            ViewBag.RegistryPath = generateRegistryPath();

            return View();
        }

        public ActionResult Show(int id, int page = 0, string sortparam = "", string sortdirection = "") {
            int items_per_page = 5;

            //https://docs.microsoft.com/en-us/ef/ef6/querying/related-data#eagerly-loading
            var category = Database.Categories.Where(item => item.Id == id).Include(item => item.Discussions.Select(item2 => item2.User)).SingleOrDefault();
            var all_discussions = category.Discussions.AsEnumerable();
            if (sortparam == "name") {
                if (sortdirection == "asc") {
                    all_discussions = all_discussions.OrderBy(item => item.Title);
                }
                if (sortdirection == "desc") {
                    all_discussions = all_discussions.OrderByDescending(item => item.Title);
                }
            }
            if (sortparam == "creator") {
                if (sortdirection == "asc") {
                    all_discussions = all_discussions.OrderBy(item => item.User.UserName);
                }
                if (sortdirection == "desc") {
                    all_discussions = all_discussions.OrderByDescending(item => item.User.UserName);
                }
            }
            var discussions = all_discussions.Skip(page * items_per_page).Take(items_per_page);

            ViewBag.Page = page;
            ViewBag.SortParam = sortparam;
            ViewBag.SortDirection = sortdirection;

            ViewBag.CountPages = 1 + category.Discussions.Count() / items_per_page;
            ViewBag.Category = category;
            ViewBag.Discussions = discussions;
            ViewBag.RegistryPath = generateRegistryPath(category);


            return View();
        }

        [Authorize(Roles = "Admin")] // Adminul poate crea noi categorii
        public ActionResult New() {
            return View();  // Nu avem ce sa-i pasam. Nu avem de reprezentat niciun obiect concret ci doar forma
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Adminul poate crea noi categorii
        public ActionResult New(Category obj) {
            try {
                if (ModelState.IsValid) {
                    Database.Categories.Add(obj);
                    Database.SaveChanges();

                    TempData["message"] = "Category " + obj.Name + " added succesfully!";
                    return RedirectToAction("Index");
                } else {
                    return View();
                }
            } catch (Exception) {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id) {
            Category entry = Database.Categories.Find(id);
            return View(entry);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Category obj) {
            try {
                if (ModelState.IsValid) {
                    Category entry = Database.Categories.Find(id);
                    entry.Description = obj.Description;
                    entry.Name = obj.Name;

                    Database.SaveChanges();
                    TempData["message"] = "Category " + entry.Name + " was updated!";

                    return RedirectToAction("Index");
                } else {
                    return View(obj);
                }
            } catch (Exception) {
                return View(obj);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int Id) {
            Category entry = Database.Categories.Find(Id);
            Database.Categories.Remove(entry);
            Database.SaveChanges();
            TempData["message"] = "Category " + entry.Name + " succesfully deleted!";
            TempData["state"] = "warning";
            return RedirectToAction("Index");
        }

        [NonAction]
        public List<BreadCrumbItem> generateRegistryPath() {
            return new List<BreadCrumbItem>() {
                    new BreadCrumbItem { name = "All categories",path="/Category/Index" }
            };
        }

        [NonAction]
        public List<BreadCrumbItem> generateRegistryPath(Category current) {
            return new List<BreadCrumbItem>() {
                    new BreadCrumbItem { name = "All categories",path="/Category/Index" },
                    new BreadCrumbItem { name = current.Name,path="",last_item=true }
            };
        }
    }
}