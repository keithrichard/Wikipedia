﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wikipedia.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class WikipediaEntities : DbContext
    {
        public WikipediaEntities()
            : base("name=WikipediaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    
        public virtual int AddArticle(string url, string articleName, string pageId)
        {
            var urlParameter = url != null ?
                new ObjectParameter("Url", url) :
                new ObjectParameter("Url", typeof(string));
    
            var articleNameParameter = articleName != null ?
                new ObjectParameter("ArticleName", articleName) :
                new ObjectParameter("ArticleName", typeof(string));
    
            var pageIdParameter = pageId != null ?
                new ObjectParameter("PageId", pageId) :
                new ObjectParameter("PageId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddArticle", urlParameter, articleNameParameter, pageIdParameter);
        }
    
        public virtual int AddArticleCategory(Nullable<int> artId, Nullable<int> catId)
        {
            var artIdParameter = artId.HasValue ?
                new ObjectParameter("artId", artId) :
                new ObjectParameter("artId", typeof(int));
    
            var catIdParameter = catId.HasValue ?
                new ObjectParameter("catId", catId) :
                new ObjectParameter("catId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddArticleCategory", artIdParameter, catIdParameter);
        }
    
        public virtual int ClearArticles()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClearArticles");
        }
    
        public virtual int AddCategory(string categoryName)
        {
            var categoryNameParameter = categoryName != null ?
                new ObjectParameter("CategoryName", categoryName) :
                new ObjectParameter("CategoryName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddCategory", categoryNameParameter);
        }
    
        public virtual ObjectResult<GetCategories_Result> GetCategories()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCategories_Result>("GetCategories");
        }
    
        public virtual ObjectResult<GetAllArticles_Result> GetAllArticles()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllArticles_Result>("GetAllArticles");
        }
    
        public virtual int ClearDatabase()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClearDatabase");
        }
    }
}