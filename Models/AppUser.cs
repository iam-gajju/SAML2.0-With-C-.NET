using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWithSAML.Models
{
    public class AppUser
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
    }
}