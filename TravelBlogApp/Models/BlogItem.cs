using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBlogApp.Models
{
    public class BlogItem
    {
        public BlogItem()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }

        [Required(ErrorMessage = " Please enter a title for Blog ")]
        [MaxLength(200)]

        public string Title { get; set; }

        [MaxLength(10000)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Published")]
        public bool IsPublished { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }


        /*  public string AuthorId { get; set; }

          public virtual Author Author { get; set; } */
    }
}
