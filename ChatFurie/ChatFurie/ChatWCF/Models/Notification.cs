using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatWCF.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public string Context { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class CommonNotification : Notification
    {
        public int UserID { get; set; }
        public User User { get; set; }
    }

    public class AddFriendNotification : Notification
    {
        public int UserFromID { get; set; }
        public User UserFrom { get; set; }

        public int UserToID { get; set; }
        public User UserTo { get; set; }

        public bool IsDialog { get; set; } = false;

        public Conversation Conversation { get; set; }
        public int? DialogID { get; set; }
    }
}