using System.ComponentModel.DataAnnotations;

namespace AuthServiceWCF.Models
{
    /// <summary>
    /// Login Model
    /// </summary>
    public class LoginModel
    {
        [Required(ErrorMessage = "Login is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
