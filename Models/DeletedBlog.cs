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
    public class DeletedBlog
    {
        public int Id { get; set; }
        public string PostStatus { get; set; }
        public string DeletedBy { get; set; }
    }
}
