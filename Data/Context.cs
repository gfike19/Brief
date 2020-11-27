using Brief.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading.Tasks;

namespace Brief.Data
{
    //public class Context : IdentityDbContext<User>
    public class Context : DbContext
    {
        /*
        public Context()
           : base("Brief")
        {
        }
        */
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<User> Users { get; set; }

       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Removing the pluralizing table name convention 
            // so our table names will use our entity class singular names.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Using the fluent API to configure entity properties...

            // Configure the string length for the Blog.Name property.
            /*
            modelBuilder.Entity<Blog>()
                .Property(a => a.Name)
                .HasMaxLength(100);
            */

            // Configure the precision and scale for the Entry.Duration property.
            /*
            modelBuilder.Entity<Entry>()
                .Property(e => e.Duration)
                .HasPrecision(5, 1);
            */
        }
    }
}