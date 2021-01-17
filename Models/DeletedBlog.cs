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
    }
}
