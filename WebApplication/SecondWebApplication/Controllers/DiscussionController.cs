using Microsoft.AspNet.Identity;
using SecondWebApplication.Helpers;
using SecondWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace SecondWebApplication.Controllers
{
    public class DiscussionController : Controller
    {
        private ApplicationDbContext Database = new ApplicationDbContext();
        // GET: Discussion
        public ActionResult Show(int id)
        {
            var discussion = Database.Discussions.Where(item => item.Id == id).Include(item => item.Category).SingleOrDefault();
            var comments = Database.Comments.Where(item => item.DiscussionId == id).Include(entry=>entry.User);
            ViewBag.Discussion = discussion;
            ViewBag.Comments = comments;
            ViewBag.afisareButoaneDiscussion = false;
            ViewBag.RegistryPath = generateRegistryPath(discussion);

            if (discussion.UserId == User.Identity.GetUserId() || User.IsInRole("Moderator") || User.IsInRole("Admin"))
            {
                ViewBag.afisareButoaneDiscussion = true;
            }
            return View();
        }
        [Authorize(Roles ="Admin,User")]
        public ActionResult New(int id)
        {
            Discussion discussion = new Discussion();
            discussion.Categories = GetAllCategories();
            discussion.CategoryId = id;

            return View(discussion);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public ActionResult New(Discussion discussion)
        {
            ModelState.Remove("UserId");    // We will add UserId later but the ModelState will still be invalid. Remove then UserId from checks

            discussion.Categories = GetAllCategories();
            discussion.UserId = User.Identity.GetUserId();
            discussion.CreationDate = DateTime.Now;

            try
            {
                if (ModelState.IsValid)
                {
                    Database.Discussions.Add(discussion);
                    Database.SaveChanges();

                    TempData["message"] = "Discussion added successfully";
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
        [Authorize(Roles = "Admin,Moderator,User")]
        public ActionResult Update(int id)
        {
            Discussion entry = Database.Discussions.Find(id);
            entry.Categories = GetAllCategories();
            if (entry.UserId == User.Identity.GetUserId() || User.IsInRole("Moderator") || User.IsInRole("Admin"))
            {
                ViewBag.discussion = entry;
                return View(entry);
            }else
            {
                TempData["message"] = "You do not have the right to make changes to a discussion that does not belong to you!";
                TempData["state"] = "error";
                return RedirectToAction("Show", "Discussion", new { Id = entry.Id });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator,User")]
        public ActionResult Update(int id, Discussion obj)
        {
            ModelState.Remove("UserId");        // ModelState is invalid because UserId is required but was not set. This is intended so remove property from the model.

            obj.Categories = GetAllCategories();
            obj.CreationDate = DateTime.Now;

            try
            {
                if (ModelState.IsValid)
                {
                    Discussion entry = Database.Discussions.Find(id);
                    if (entry.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        entry.Title = obj.Title;
                        entry.Text = obj.Text;
                        entry.CategoryId = obj.CategoryId;

                        Database.SaveChanges();
                        TempData["message"] = "Discussion " + entry.Title + " was updated!";

                        return RedirectToAction("Show", "Discussion", new { Id = obj.Id });
                    }
                    else if (User.IsInRole("Moderator"))
                    {
                        entry.CategoryId = obj.CategoryId;

                        Database.SaveChanges();
                        TempData["message"] = "Discussion " + entry.Title + " was updated!";

                        return RedirectToAction("Show", "Discussion", new { Id = obj.Id });
                    }
                    else
                    {
                        TempData["message"] = "You do not have the right to make changes to a discussion that does not belong to you!";
                        TempData["state"] = "error";
                        return RedirectToAction("Show", "Discussion", new { Id = entry.Id });
                    }
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
        [Authorize(Roles = "Admin,Moderator,User")]
        public ActionResult Delete(int Id)
        {
            Discussion entry = Database.Discussions.Find(Id);
            var SavedCategoryId = entry.CategoryId;
            if (entry.UserId == User.Identity.GetUserId() || User.IsInRole("Moderator") || User.IsInRole("Admin"))
            {
                Database.Discussions.Remove(entry);
                Database.SaveChanges();
                TempData["message"] = "Discussion " + entry.Title + " succesfully deleted!";
                TempData["state"] = "warning";
                return RedirectToAction("Show", "Category", new { Id = SavedCategoryId });
            }
            else
            {
                TempData["message"] = "You do not have the right to delete a discussion that does not belong to you!";
                TempData["state"] = "error";
                return RedirectToAction("Show", "Discussion", new { Id = entry.Id });
            }
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
                    Text = category.Name.ToString(),
                    
                });
            }
            // returnam lista de categorii
            return selectList;
        }

        [NonAction]
        public List<BreadCrumbItem> generateRegistryPath(Discussion current) {
            return new List<BreadCrumbItem> () { 
                    new BreadCrumbItem { name = "All categories",path="/Category/Index" },
                    new BreadCrumbItem { name = current.Category.Name,path="/Category/Show/"+current.CategoryId },
                    new BreadCrumbItem { name = current.Title,path="/Discussion/Show/"+current.Id,last_item=true }
            };
        }
    }
}