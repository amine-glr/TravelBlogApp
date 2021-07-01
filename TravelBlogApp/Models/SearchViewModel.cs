using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBlogApp.Models
{
    public class SearchViewModel
    {
        public string SearchText { get; set; }

        public List<BlogItem> Result { get; set; }

        public List<Category> Result2 { get; set; }
    }
}
