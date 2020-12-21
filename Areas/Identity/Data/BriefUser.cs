using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Brief.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Brief.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the BriefUser class
    public class BriefUser : IdentityUser
    {

        //public string FullName { get; internal set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime JoinedOn { get; internal set; }
        public int RoleId { get; set; }
    }
}
