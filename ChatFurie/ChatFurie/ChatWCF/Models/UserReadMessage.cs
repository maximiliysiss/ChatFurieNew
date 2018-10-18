using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatFurie.Models.ChatModel
{
    public class UserReadMessage
    {
        [Key]
        public int ID { get; set; }

        public int ConversationMessageID { get; set; }
        public ConversationMessage ConversationMessage { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public bool IsRead { get; set; }
    }
}
