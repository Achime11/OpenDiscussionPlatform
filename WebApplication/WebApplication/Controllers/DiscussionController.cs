using WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using WebApplication.DataAccessLayer;

namespace WebApplication.Controllers
{
    public class DiscussionController : Controller
    {
        private DatabaseContext Database = new DatabaseContext();
        // GET: Discussion
        public ActionResult Show(int id)
        {
            var discussion = Database.Discussions.Find(id);
            var comments = Database.Comments.Where(item => item.DiscussionId == id);
            ViewBag.Discussion = discussion;
            ViewBag.Comments = comments;
            return View();
        }
        public ActionResult New(int id)
        {
            Discussion discussion = new Discussion();
            discussion.Categories = GetAllCategories();
            discussion.CategoryId = id;

            return View(discussion);
        }
        [HttpPost]
        public ActionResult New(Discussion discussion)
        {
            discussion.Categories = GetAllCategories();
            discussion.UserId = 1;
            discussion.CreationDate = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    Database.Discussions.Add(discussion);
                    Database.SaveChanges();

                    TempData["message"] = "Discutia adaugata cu success";
                    return RedirectToAction("Show","Discussion",new { Id=discussion.Id});
                }
                else
                {
                    return View(discussion);
                }
            }
            catch (Exception)
            {
                return View(discussion);
            }
        }
        public ActionResult Update(int id)
        {
            Discussion entry = Database.Discussions.Find(id);
            entry.Categories = GetAllCategories();
            return View(entry);
        }

        [HttpPost]
        public ActionResult Update(int id, Discussion obj)
        {
            obj.Categories = GetAllCategories();
            obj.UserId = 1;
            obj.CreationDate = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    Discussion entry = Database.Discussions.Find(id);
                    entry.Title = obj.Title;
                    entry.Text = obj.Text;
                    entry.CategoryId = obj.CategoryId;
                    entry.UserId = obj.UserId;

                    Database.SaveChanges();
                    TempData["message"] = "Discussion " + entry.Title + " was updated!";

                    return RedirectToAction("Show", "Discussion", new { Id = obj.Id });
                }
                else
                {
                    return View(obj);
                }
            }
            catch (Exception)
            {
                return View(obj);
            }
        }
        public ActionResult Delete(int Id)
        {
            Discussion entry = Database.Discussions.Find(Id);
            var SavedCategoryId = entry.CategoryId;
            Database.Discussions.Remove(entry);
            Database.SaveChanges();
            TempData["message"] = "Discussion " + entry.Title + " succesfully deleted!";
            TempData["warning"] = "";
            return RedirectToAction("Show", "Category", new { Id = SavedCategoryId });
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var categories = from cat in Database.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories(Category category_id)
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var categories = from cat in Database.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }
    }
}