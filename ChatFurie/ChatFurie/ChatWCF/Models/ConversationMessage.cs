using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatWCF.Models
{
    public class ConversationMessage
    {
        [Key]
        public int ID { get; set; }

        public int ConversationID { get; set; }
        public Conversation Conversation { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }

        public int AuthorID { get; set; }
        public User Author { get; set; }

        public List<UserReadMessage> UserReadMessages { get; set; }
    }
}
