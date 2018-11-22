using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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

    [DataContract]
    public class LoginActionResult : IActionResultWCF
    {
        [DataMember]
        public ResultCode Status { get; set; }
        [DataMember]
        public string Id { get; set; }
    }

    [DataContract]
    public class RegisterActionResult : LoginActionResult, IActionResultWCF
    {
        [DataMember]
        public string Email { get; set; }
    }
}