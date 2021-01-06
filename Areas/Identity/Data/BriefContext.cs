using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brief.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Brief.Models;

namespace Brief.Data
{
    public class BriefContext : IdentityDbContext<BriefUser>
    {
        public BriefContext(DbContextOptions<BriefContext> options)
            : base(options)
        {
        }

        public DbSet<BriefUser> BriefUsers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        //public DbSet<Blog> DeletedBlogs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
