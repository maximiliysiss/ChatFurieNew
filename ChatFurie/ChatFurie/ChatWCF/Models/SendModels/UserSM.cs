using ChatFurie.Models.ChatModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatWCF.Models.SendModels
{
    public class UserSM
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }

        public UserSM(User user)
        {
            this.Email = user.Email;
            this.ID = user.ID;
            this.Image = user.Image;
            this.Login = user.Login;
            this.Name = user.Name;
        }
    }
}