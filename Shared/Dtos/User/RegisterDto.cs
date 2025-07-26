using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.User
{
    public  class RegisterDto
    {
        // UserId, Username, PasswordHash, Role(Admin, Customer)
        
        public string UserName { get; set; } = string.Empty;
        [DataType(DataType.Password), Required(ErrorMessage = "Password is required"), MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        public string? City { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? Street { get; set; } = string.Empty;
    }
}
