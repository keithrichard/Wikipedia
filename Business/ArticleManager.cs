using System.Collections.Generic;
using System.Linq;
using Wikipedia.Database;

namespace Business
{
    public class ArticleManager
    {
        private readonly WikipediaEntities _wikipediadatacontext;
        public ArticleManager()
        {
            _wikipediadatacontext = new WikipediaEntities();
        }
        /// <summary>
        /// Adds an article to DB
        /// </summary>
        /// <param name="article"></param>
        public void SaveArticle(Business.Entities.Article article)
        {
            _wikipediadatacontext.AddArticle(article.Url, article.ArticleName, article.PageId);            
        }
        /// <summary>
        /// Adds a category to DB
        /// </summary>
        /// <param name="categoryName"></param>
        public void SaveCategory(string categoryName)
        {
            _wikipediadatacontext.AddCategory(categoryName);
        }
        /// <summary>
        /// Adds an ArticleCategory to DB
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="categoryId"></param>
        public void ConnectArticleCategory(int articleId, int categoryId) {
            _wikipediadatacontext.AddArticleCategory(articleId, categoryId);
        }

        /// <summary>
        /// Gets Wikiarticle with its categories
        /// </summary>
        /// <returns></returns>
        public List<Entities.Article> GetSearchResults()
        {
            List<Entities.Article> articles = new List<Entities.Article>();
            List<GetAllArticles_Result> dbArticles = _wikipediadatacontext.GetAllArticles().ToList();

            foreach (GetAllArticles_Result dbArticle in dbArticles)
            {
                if (articles.Exists(x => x.Id == dbArticle.Id))
                    continue;

                List<string> catNames = dbArticles.Where(x => x.Id == dbArticle.Id).Select(y => y.CategoryName).Where(category => category != null).ToList();

                if (!catNames.Any())
                    catNames.Add("Not sorted");

                List<Business.Entities.Category> categories = new List<Entities.Category>();

                
                foreach (var catName in catNames)
                {
                    categories.Add(new Entities.Category {
                        CategoryName = catName,
                        Id = dbArticles.Where(x => x.CategoryName == catName).Select(y => y.CategoryID).FirstOrDefault()
                    });
                }
                

                Entities.Article article = new Entities.Article
                {
                    Id = dbArticle.Id,
                    ArticleName = dbArticle.ArticleName,
                    Url = dbArticle.Url,
                    Category = categories
                };

                articles.Add(article);
            }            
            return articles;
        }

        /// <summary>
        /// Gets all categories in DB
        /// </summary>
        /// <returns></returns>
        public List<Entities.Category> GetCategories()
        {
            List<Entities.Category> returnCategories = new List<Entities.Category>();
            var categories = _wikipediadatacontext.GetCategories();
            foreach (var cat in categories)
            {
                returnCategories.Add(new Entities.Category {
                    Id = cat.Id,
                    CategoryName = cat.CategoryName
                });
            }

            return returnCategories;
        }
        /// <summary>
        /// Clears all tables in DB
        /// </summary>
        public void ClearDB()
        {
            _wikipediadatacontext.ClearDatabase();
        }        
    }
} 