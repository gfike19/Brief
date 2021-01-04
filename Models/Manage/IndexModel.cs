using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Brief.Models.Manage
{
    public class IndexModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
