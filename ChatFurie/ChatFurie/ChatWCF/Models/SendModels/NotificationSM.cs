using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChatWCF.Models.SendModels
{
    [DataContract]
    public class NotificationSM
    {
        public NotificationSM(CommonNotification common)
        {
            this.Context = common.Context;
            this.ID = common.ID;
            this.DateTime = common.DateTime;
        }

        public NotificationSM(AddFriendNotification addFriend, User user)
        {
            this.ID = addFriend.ID;
            this.Context = $"User {user.Name} want add you to dialog";
            this.DateTime = addFriend.DateTime;
        }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Context { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public int CreatorID { get; set; }
    }
}