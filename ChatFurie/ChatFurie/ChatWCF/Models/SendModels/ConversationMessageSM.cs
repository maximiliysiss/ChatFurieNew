using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChatWCF.Models.SendModels
{
    [DataContract]
    public class ConversationMessageSM
    {
        public ConversationMessageSM(ConversationMessage conversation, bool IsRead, string user)
        {
            this.Content = conversation.Content;
            this.IsRead = IsRead;
            this.Sender = user;
            this.Time = conversation.DateTime;
            this.ID = conversation.ID;
        }

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public string Time { get; set; }
        [DataMember]
        public string Sender { get; set; }
        [DataMember]
        public bool IsRead { get; set; }
    }
}