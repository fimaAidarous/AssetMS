using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sample.Models.DTO
{
    public class UserListDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public IList<string>? Roles { get; set; }
    }
}