using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Business;
using Business.Entities;
using Wikipedia.Models;

namespace Wikipedia.Controllers
{
    public class HomeController : Controller
    {
        private ArticleManager _articleManager;
        private SessionHandler _sessionHandler;

        public HomeController()
        {
            _articleManager = new ArticleManager();
            _sessionHandler = new SessionHandler();
        }

        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        /// <summary>
        ///Returns wikipedia articles and their categories   
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult GetSearchResult()
        {
            List<Article> articles = _articleManager.GetSearchResults();

            SearchResultModel searchResultModel = new SearchResultModel()
            {
                WikipediaArticles = articles
            };
            _sessionHandler.SearchResultModel = searchResultModel;

            if (articles.Count() >= 10)
                _sessionHandler.MaxArticleCount = true;

            return PartialView("~/Views/PartialViews/_PartialViewSearchResult.cshtml", searchResultModel);
        }

        /// <summary>
        ///Returns JSON of the categories in DB 
        /// </summary>
        /// <returns></returns>    
        [HttpPost]
        public ActionResult GetCategories()
        {
            List<Category> categories = GetCategoryList();
            return new JsonResult() { Data = categories };
        }

        /// <summary>
        ///Returns a List<Category> of the categories in DB and stores them in a session
        /// </summary>
        /// <returns></returns>    
        private List<Category> GetCategoryList()
        {
            List<Category> categories = _articleManager.GetCategories();
            _sessionHandler.Categories = categories;
            return categories;
        }

        /// <summary>
        ///Saves a wikipediaarticle to the DB
        /// </summary>
        /// <param name="articleName"></param>
        /// <param name="pageId"></param>
        [HttpPost]        
        public void SaveArticle(string articleName, string pageId)
        {
            try
            {
                if (_sessionHandler.MaxArticleCount == true)
                    return;

                string url = "https://en.wikipedia.org/w/api.php?action=query&format=json&pageids=" + pageId + "&prop=extracts&format=json&callback=?";
                Article article = new Article()
                {
                    ArticleName = articleName,
                    PageId = pageId,
                    Url = url
                };
                _articleManager.SaveArticle(article);
            }
            catch (Exception exception)
            {
                throw ;
            }
        }
        /// <summary>
        /// Clears all tables in DB
        /// </summary>
        [HttpPost]        
        public void ClearDB()
        {
            try
            {
                _articleManager.ClearDB();
                _sessionHandler.MaxArticleCount = false;

            }
            catch (Exception exception)
            {
                throw;
            }
        }

        /// <summary>
        ///Add category to the article, if the category does not exist in the db it also adds category to DB
        /// </summary>
        /// <param name="category"></param>
        /// <param name="articleId"></param>
        [HttpPost]
        public void AddCategory(string category, int articleId)
        {
            try
            {
                int categoryId;
                List<Category> categories = _sessionHandler.Categories != null
                    ? _sessionHandler.Categories
                    : GetCategoryList();

                if (categories.Any(x => x.CategoryName == category))
                {
                    categoryId = Convert.ToInt32(GetCategoryId(category, categories));
                }
                else
                {// if category doesnt exist in DB, add category to DB                    
                    _articleManager.SaveCategory(category);
                    categories = GetCategoryList();
                    categoryId = Convert.ToInt32(GetCategoryId(category, categories));
                }

                _articleManager.ConnectArticleCategory(articleId, categoryId);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get categoryID of categoryname
        /// </summary>
        /// <param name="category"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        private static int? GetCategoryId(string category, List<Category> categories)
        {
            Category cat = categories.Where(x => x.CategoryName == category).FirstOrDefault();
            return cat.Id;
        }
    }
}