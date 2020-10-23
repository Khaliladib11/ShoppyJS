using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Remote(action: "isUsernameInUse", controller: "Account")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string EncryptedPass { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "isEmailInUse", controller: "Account")]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Role { get; set; }

    }
}
