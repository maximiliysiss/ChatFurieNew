using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChatWCF.Models.SendModels
{
    [DataContract]
    public class UserFriendSM
    {
        public UserFriendSM(UserSM userSM, bool isFriend)
        {
            UserSM = userSM;
            IsFriend = isFriend;
        }

        public UserFriendSM(User userSM, bool isFriend)
        {
            UserSM = new UserSM(userSM);
            IsFriend = isFriend;
        }


        [DataMember]
        public UserSM UserSM { get; set; }

        [DataMember]
        public bool IsFriend { get; set; }
    }
}