using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace sample.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public string? ProfilePicture { get; set; }
    }
}
