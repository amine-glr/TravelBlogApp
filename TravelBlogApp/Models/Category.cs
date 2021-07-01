using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBlogApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1500)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual List<BlogItem> BlogItems { get; set; }

        public string AuthorId { get; set; }

        public virtual Author Author { get; set; }
    }
}
