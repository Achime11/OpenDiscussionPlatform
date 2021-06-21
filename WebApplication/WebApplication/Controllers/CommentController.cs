using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.DataAccessLayer;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CommentController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // This displays all of the users Comments
        // Should be accesed only from his dashboard and not from Discussions
        public ActionResult Index()
        {
            var comments = db.Comments.Include(item => item.Discussion).ToList();

            return View(comments);
        }

        public ActionResult Show(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        public ActionResult New(int? discussion_id)
        {
            Comment empty_comment = new Comment();
            empty_comment.Discussions = GetAllDiscussions();
            empty_comment.DiscussionId = discussion_id??0;

            return View(empty_comment);
        }

        [HttpPost]
        public ActionResult New(Comment comment)
        {
            comment.Discussions = GetAllDiscussions();
            comment.UserId = 1;        // Some user value

            try {
                if (ModelState.IsValid) {
                    comment.Date = DateTime.Now;

                    db.Comments.Add(comment);
                    db.SaveChanges();

                    TempData["message"] = "Added a new comment!";
                    return RedirectToAction("Show","Discussion",new { Id=comment.DiscussionId});
                }

                return View(comment);
            } catch (Exception) {
                return View(comment);
            }
            
        }

        public ActionResult Update(int id)
        {
            Comment comment = db.Comments.Find(id);
            comment.Discussions = GetAllDiscussions();

            return View(comment);
        }

        [HttpPost]
        public ActionResult Update(int id,Comment obj)
        {
            obj.Discussions = GetAllDiscussions();

            try {
                if (ModelState.IsValid) {
                    Comment entry = db.Comments.Find(id);

                    entry.Content = obj.Content;
                    entry.DiscussionId = obj.DiscussionId;
                    entry.Date = DateTime.Now;

                    db.SaveChanges();

                    TempData["message"] = "Updated your comment!";
                    return RedirectToAction("Show", "Discussion", new { Id = obj.DiscussionId });
                }

                return View(obj);
            } catch (Exception) {
                return View(obj);
            }
            
        }


        public ActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();

            TempData["message"] = "Deleted comment!";
            TempData["warning"] = "";
            return RedirectToAction("Show", "Discussion", new { Id = comment.DiscussionId });
        }

        private IEnumerable<SelectListItem> GetAllDiscussions() {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach(var discussion in db.Discussions) {
                list.Add(new SelectListItem { Value = discussion.Id.ToString(), Text = discussion.Title });
            }
            return list;
        }
    }
}
