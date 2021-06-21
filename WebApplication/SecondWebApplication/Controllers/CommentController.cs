using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SecondWebApplication.Models;

namespace SecondWebApplication.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin,User")]
        public ActionResult New(int discussion_id)
        {
            Comment empty_comment = new Comment();
            empty_comment.Discussions = GetAllDiscussions();
            empty_comment.DiscussionId = discussion_id;
            empty_comment.Discussion = db.Discussions.Find(discussion_id);

            return View(empty_comment);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public ActionResult New(Comment comment)
        {
            comment.Discussions = GetAllDiscussions();
            comment.Discussion = db.Discussions.Find(comment.DiscussionId);
            comment.UserId = User.Identity.GetUserId();
            
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

        [Authorize(Roles = "Admin,User")]
        public ActionResult Update(int id)
        {
            // Where + Include must be used otherwise no eager loading is done. Find does not fill up Discussion field
            Comment comment = db.Comments.Where(item => item.Id == id).Include(item => item.Discussion).SingleOrDefault();
            comment.Discussions = GetAllDiscussions();


            if (comment.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(comment);
            }
            else
            {
                TempData["message"] = "You do not have the right to make changes to a comment that does not belong to you!";
                TempData["state"] = "error";
                return RedirectToAction("Show", "Discussion", new { Id = comment.DiscussionId });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Update(int id,Comment obj)
        {
            obj.Discussions = GetAllDiscussions();

            try {
                if (ModelState.IsValid) {
                    Comment entry = db.Comments.Find(id);

                    if (entry.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        entry.Content = obj.Content;
                        entry.DiscussionId = obj.DiscussionId;
                        entry.Date = DateTime.Now;

                        db.SaveChanges();

                        TempData["message"] = "Updated your comment!";
                        return RedirectToAction("Show", "Discussion", new { Id = obj.DiscussionId });
                    }
                    else
                    {
                        TempData["message"] = "You do not have the right to make changes to a comment that does not belong to you!";
                        TempData["state"] = "error";
                        return RedirectToAction("Show", "Discussion", new { Id = entry.DiscussionId });
                    }
                }

                return View(obj);
            } catch (Exception) {
                return View(obj);
            }
            
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        public ActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment.UserId == User.Identity.GetUserId() || User.IsInRole("Moderator") || User.IsInRole("Admin"))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                TempData["message"] = "Deleted comment!";
                TempData["state"] = "warning";
                return RedirectToAction("Show", "Discussion", new { Id = comment.DiscussionId });
            }
            else
            {
                TempData["message"] = "You do not have the right to delete a comment that does not belong to you!";
                TempData["warning"] = "";
                return RedirectToAction("Show", "Discussion", new { Id = comment.DiscussionId });
            }
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllDiscussions() {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach(var discussion in db.Discussions) {
                list.Add(new SelectListItem { Value = discussion.Id.ToString(), Text = discussion.Title });
            }
            return list;
        }
    }
}
