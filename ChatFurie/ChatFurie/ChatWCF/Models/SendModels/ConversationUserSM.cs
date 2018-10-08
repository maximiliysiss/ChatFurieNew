using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatFurie.Models.ChatModel;

namespace ChatWCF.Models.SendModels
{
    public class ConversationUserSM : UserSM
    {
        public ConversationUserSM(User user) : base(user)
        {
        }

        public bool IsAdmin { get; set; }
    }
}