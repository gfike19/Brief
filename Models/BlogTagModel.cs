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
    public class BlogTag
    {
        public int ID { get; set; }
        public string TagID { get; set; }
        public string BlogID { get; set; }
    }
}
