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
            this.CreatorID = conversation.CreatorID;
        }

        public ConversationUserSM(UserInConversation userInConversation) : base(userInConversation)
        {
        }

        [DataMember]
        public bool IsAdmin { get; set; }

        [DataMember]
        public int ConversationID { get; set; }

        [DataMember]
        public int CreatorID { get; set; }

        [DataMember]
        public List<ConversationMessageSM> ConversationMessageSMs { get; set; }
    }
}