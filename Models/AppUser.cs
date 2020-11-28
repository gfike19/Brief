using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Brief.Models
{
    public class AppUser : IdentityUser
    //public class User
    {
        //public int Id { get; set; }
        public ICollection<Blog> Blogs { get; set; }
    }
}
