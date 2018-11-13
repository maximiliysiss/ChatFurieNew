

using AuthServiceWCF.Helpers;
using AuthServiceWCF.Services;
using ChatWCF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthServiceWCF.Models
{

    /// <summary>
    /// For registration
    /// </summary>
    public class RegisterModel : IEquatable<RegisterModel>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as RegisterModel);
        }

        #region HashAndEquals

        public bool Equals(RegisterModel other)
        {
            return other != null &&
                   Email == other.Email &&
                   Password == other.Password &&
                   ConfirmedPassword == other.ConfirmedPassword &&
                   Birthday == other.Birthday;
        }


        public override int GetHashCode()
        {
            var hashCode = 229174974;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ConfirmedPassword);
            hashCode = hashCode * -1521134295 + Birthday.GetHashCode();
            return hashCode;
        }

        #endregion HashAndEquals

        public static bool operator ==(RegisterModel r1, User u1)
        {
            return r1.Email == u1.Email && u1.PasswordHash == CryptService.GetMd5Hash(r1.Password);
        }

        public static bool operator !=(RegisterModel r1, User u1)
        {
            return !(r1 == u1);
        }
    }
}
