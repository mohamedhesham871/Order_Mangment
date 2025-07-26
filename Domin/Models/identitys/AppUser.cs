using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models.identitys
{
    public class AppUser : IdentityUser
    {

        #region Address of User
        public string? Country { get; set; } 
        public string? City { get; set; } 
        public string? Street { get; set; } 
        #endregion
    }
}
