using SecondWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using SecondWebApplication.Helpers;

namespace SecondWebApplication.Controllers {
    public class HomeController : Controller {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index() {
            return RedirectToAction("Index", "Category");
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public ActionResult Search() {
            ViewBag.Categories = db.Categories;
            return View();
        }

        [HttpPost]
        public ActionResult Search(int categoryFilter, int scopeSearch, string phraseSearch) {
            List<SearchEntry> comparisons = new List<SearchEntry>();
            phraseSearch = phraseSearch.ToLower();

            switch (scopeSearch) {
                case 1: {
                        // Search in discussion
                        IEnumerable<Discussion> discussions;
                        if (categoryFilter != 0) {
                            discussions = db.Discussions.Where(item => item.CategoryId == categoryFilter).Include(item => item.Category);
                        } else {
                            discussions = db.Discussions.Include(item => item.Category);
                        }

                        foreach (var entry in discussions) {
                            comparisons.Add(new SearchEntry() { link = "/Discussion/Show/" + entry.Id, path = "All categories > " + entry.Category.Name + " > " + entry.Title, similarity = Levenshtein.Calculate(phraseSearch, entry.Title.ToLower()) });
                        }
                        comparisons.Sort();
                        break;
                    }
                case 2: {
                        // Search in comments
                        IEnumerable<Comment> comments;
                        if (categoryFilter != 0) {
                            comments = db.Comments.Where(item => item.Discussion.CategoryId == categoryFilter).Include(item => item.Discussion.Category).Include(item => item.User);
                        } else {
                            comments = db.Comments.Include(item => item.Discussion.Category).Include(item => item.User);
                        }

                        foreach (var entry in comments) {
                            comparisons.Add(new SearchEntry() { link = "/Discussion/Show/" + entry.DiscussionId, path = $"All categories > {entry.Discussion.Category.Name} > {entry.Discussion.Title} > [Comment by {entry.User.UserName} on {entry.Date.ToString("dd-MM-yyyy")}]", extra = entry.Content, similarity = Levenshtein.Calculate(phraseSearch, entry.Content.ToLower()) });
                        }
                        comparisons.Sort();
                        break;
                    }
                default: {
                        // Search in discussion and comments
                        IEnumerable<Discussion> discussions;
                        IEnumerable<Comment> comments;
                        if (categoryFilter != 0) {
                            discussions = db.Discussions.Where(item => item.CategoryId == categoryFilter).Include(item => item.Category);
                            comments = db.Comments.Where(item => item.Discussion.CategoryId == categoryFilter).Include(item => item.Discussion.Category).Include(item => item.User);
                        } else {
                            discussions = db.Discussions.Include(item => item.Category);
                            comments = db.Comments.Include(item => item.Discussion.Category).Include(item => item.User);
                        }


                        foreach (var entry in discussions) {
                            comparisons.Add(new SearchEntry() { link = "/Discussion/Show/" + entry.Id, path = "All categories > " + entry.Category.Name + " > " + entry.Title, similarity = Levenshtein.Calculate(phraseSearch, entry.Title.ToLower()) });
                        }
                        foreach (var entry in comments) {
                            comparisons.Add(new SearchEntry() { link = "/Discussion/Show/" + entry.DiscussionId, path = $"All categories > {entry.Discussion.Category.Name} > {entry.Discussion.Title} > [Comment by {entry.User.UserName} on {entry.Date.ToString("dd-MM-yyyy")}]", extra = entry.Content, similarity = Levenshtein.Calculate(phraseSearch, entry.Content.ToLower()) });
                        }

                        comparisons.Sort();
                        break;
                    }
            }

            var result = comparisons.Take(10);
            ViewBag.Categories = db.Categories;
            ViewBag.Results = (result.Count() == 0) ? null : result;
            return PartialView("_SearchResults");
        }
    }
}