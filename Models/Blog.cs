using Brief.Areas.Identity.Data;
using Brief.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Brief.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Comments { get; set; }
        public DateTime TimeCreated { get; set; }
        public string PostStatus { get; set; }
        public int SaveDetails()
        {
            using(SqlConnection con = new SqlConnection(GetConString.ConString()))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Blogs(Title, CreatorId, Content, CreatorName, TimeCreated, PostStatus) VALUES (@Title, @CreatorId, @Content, @CreatorName, @TimeCreated, @status)";
                cmd.Parameters.AddWithValue("@Title", Title);
                cmd.Parameters.AddWithValue("@CreatorId", CreatorId);
                cmd.Parameters.AddWithValue("@Content", Content);
                cmd.Parameters.AddWithValue("@CreatorName", CreatorName);
                cmd.Parameters.AddWithValue("@TimeCreated", TimeCreated);
                cmd.Parameters.AddWithValue("@Status", "Posted");
                con.Open();
                int i = cmd.ExecuteNonQuery();
                return i;
            }            
        }

        public static class SeedBlogs
        {
            public static void Initialize(IServiceProvider serviceProvider)
            {
                using (var context = new BriefContext(
                    serviceProvider.GetRequiredService<
                        DbContextOptions<BriefContext>>()))
                {
                    // Look for any movies.
                    if (context.Blogs.Any())
                    {
                        return;   // DB has been seeded
                    }

                    context.Blogs.AddRange(
                        new Blog
                        {

                            Title = "When Harry Met Sally",
                            Content = "Blog 1",
                            CreatorName = "Justin",
                            TimeCreated = DateTime.Now
                        },

                        new Blog
                        {

                            Title = "When Harry Met Sally",
                            Content = "Blog 2",
                            CreatorName = "Justin",
                            TimeCreated = DateTime.Now
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
