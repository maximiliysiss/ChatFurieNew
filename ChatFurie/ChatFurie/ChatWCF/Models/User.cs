using ChatWCF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatWCF.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public DateTime LastEnter { get; set; }
        public string Login { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }

        public List<Conversation> Conversations { get; set; }
        public List<UserInConversation> UserInConversations { get; set; }
        public List<CommonNotification> CommonNotifications { get; set; }
        public List<AddFriendNotification> AddFriendNotificationsTo { get; set; }
        public List<AddFriendNotification> AddFriendNotificationsFrom { get; set; }
        public List<UserReadMessage> UserReadMessages { get; set; }
        public List<ConversationMessage> ConversationMessages { get; set; }
    }
}
