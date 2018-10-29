using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChatWCF.Models.SendModels
{
    [DataContract]
    public class ConversationUserSM : UserSM
    {
        public ConversationUserSM(User user) : base(user)
        {
        }

        public ConversationUserSM(Conversation conversation) : base(conversation)
        {
        }

        [DataMember]
        public bool IsAdmin { get; set; }
    }
}