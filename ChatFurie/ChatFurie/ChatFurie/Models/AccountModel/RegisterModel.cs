using ChatFurie.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatFurie.Models.AccountModel
{

    /// <summary>
    /// For registration
    /// </summary>
    public class RegisterModel
    {
        [Required(ErrorMessage = "Login is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password not confirmed")]
        [Required(ErrorMessage = "Confirmed password is required")]
        public string ConfirmedPassword { get; set; }

        [ValidationDate]
        [Required(ErrorMessage = "Birthday is required")]
        public DateTime Birthday { get; set; }
    }
}
