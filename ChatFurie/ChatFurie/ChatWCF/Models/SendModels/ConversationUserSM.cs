using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ChatFurie.Models.ChatModel;

namespace ChatWCF.Models.SendModels
{
    [DataContract]
    public class ConversationUserSM : UserSM
    {
        public ConversationUserSM(User user) : base(user)
        {
        }

        [DataMember]
        public bool IsAdmin { get; set; }
    }
}