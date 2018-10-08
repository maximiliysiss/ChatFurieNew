using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatFurie.Models.ChatModel
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
    }
}
