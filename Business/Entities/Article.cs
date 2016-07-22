using System.Collections.Generic;

namespace Business.Entities
{
    public class Article
    {
        public int Id { get; set; }

        public string Url {get; set;}

        public string ArticleName { get; set; }

        public List<Category> Category { get; set; }

        public string PageId { get; set; }
    }
}