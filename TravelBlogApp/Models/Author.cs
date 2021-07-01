using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelBlogApp.Models
{
    public class Author: IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string NickName { get; set; }

        public DateTime DateofBirth { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public virtual List<BlogItem> BlogItems { get; set; }
    }
}
