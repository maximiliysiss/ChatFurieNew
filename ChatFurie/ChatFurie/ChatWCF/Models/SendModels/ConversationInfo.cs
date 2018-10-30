using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChatWCF.Models.SendModels
{
    [DataContract]
    public class ConversationInfo
    {
        [DataMember]
        public List<ConversationUserSM> ConversationUserSMs { get; set; }

        [DataMember]
        public ConversationUserSM Conversation { get; set; }

        [DataMember]
        public bool IsCurrentUserAdmin { get; set; }

        public ConversationInfo(Conversation conversation, List<User> userInConversations)
        {
            ConversationUserSMs = userInConversations.Select(x => new ConversationUserSM(x)).ToList();
            Conversation = new ConversationUserSM(conversation);
        }
    }
}