using ChatWCF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatWCF.Models
{
    public class Conversation
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateStart { get; set; }
        public int CreatorID { get; set; }
        public User Creator { get; set; }

        public List<UserInConversation> UserInConversations { get; set; }
        public List<AddFriendNotification> AddFriendNotifications { get; set; }
        public List<ConversationMessage> ConversationMessages { get; set; }
    }
}
