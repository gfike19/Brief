using Brief.Areas.Identity.Data;
using Brief.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Brief.Models
{
    //[NotMapped]
    public class DeletedBlog// : Blog
    {
        public int Id { get; set; }
        public string PostStatus { get; set; }
        
        public string DeletedBy { get; set; }

        //public int Id { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Comments { get; set; }
        public DateTime TimeCreated { get; set; }
        //public string PostStatus { get; set; }
    }
}
