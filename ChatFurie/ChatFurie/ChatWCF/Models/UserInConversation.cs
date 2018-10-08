using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatFurie.Models.ChatModel
{
    public class UserInConversation
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int ConversationID { get; set; }
        public Conversation Conversation { get; set; }

        public string Image { get; set; }

        public DateTime DateStart { get; set; }
    }
}
