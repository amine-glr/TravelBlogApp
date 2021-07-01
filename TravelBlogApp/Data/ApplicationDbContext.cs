using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TravelBlogApp.Models;

namespace TravelBlogApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<Author>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogItem> BlogItems { get; set; }
    }
}
