using System;
using System.Collections.Generic;
using System.Web;
using Wikipedia.Models;


namespace Wikipedia
{
    public class SessionHandler
    {
        /// <summary>
        /// </summary>
        public bool MaxArticleCount
        {
            get
            {
                if (HttpContext.Current.Session["MaxArticleCount"] == null)
                {
                    return false;
                }
                return (bool)HttpContext.Current.Session["MaxArticleCount"];
            }
            set { HttpContext.Current.Session["MaxArticleCount"] = value; }
        }
        /// <summary>
        /// </summary>
        public SearchResultModel SearchResultModel
        {
            get
            {
                if ((SearchResultModel)HttpContext.Current.Session["SearchResultModel"] == null)
                {

                    return null;
                }
                return (SearchResultModel)HttpContext.Current.Session["SearchResultModel"];

            }
            set { HttpContext.Current.Session["SearchResultModel"] = value; }
        }
        /// <summary>
        /// </summary>
        public List<Business.Entities.Category> Categories
        {
            get
            {
                if ((List<Business.Entities.Category>)HttpContext.Current.Session["Categories"] == null)
                {
                    return null;
                }
                return (List<Business.Entities.Category>)HttpContext.Current.Session["Categories"];

            }
            set { HttpContext.Current.Session["Categories"] = value; }
        }
    }
}