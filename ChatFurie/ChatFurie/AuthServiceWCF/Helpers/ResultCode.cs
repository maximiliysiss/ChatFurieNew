using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthServiceWCF.Helpers
{
    public enum ResultCode
    {
        Success,
        Error,
        ErrorAccess,
        ErrorLoginPassword
    }

    public interface IActionResultWCF
    {
        ResultCode Status { get; set; }
    }

    public class LoginActionResult : IActionResultWCF
    {
        public ResultCode Status { get; set; }
        public string Id { get; set; }
    }

    public class RegisterActionResult : LoginActionResult, IActionResultWCF
    {
        public string Email { get; set; }
    }
}