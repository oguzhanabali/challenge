using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication3.Models
{
    public class OwinAuthDbContext : IdentityDbContext
    {
        public OwinAuthDbContext()
            : base("OwinAuthDbContext")
        {
           
        }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<BookByUser> BookByUser { get; set; }
    }
}