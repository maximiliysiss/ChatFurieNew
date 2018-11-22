using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AuthServiceWCF.Models
{
    [DataContract]
    public class UserPageModel
    {
        [DataMember]
        [Required]
        public int ID { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string PasswordHash { get; set; }
        [DataMember]
        public string ChangePassword { get; set; }
        [DataMember]
        public string ConfirmedPassword { get; set; }

        [Required]
        [DataMember]
        public string VerifyPassword { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}