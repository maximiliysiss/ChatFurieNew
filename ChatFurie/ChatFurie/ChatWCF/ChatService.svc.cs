using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ChatWCF.Models.SendModels;

namespace ChatWCF
{
    public class ChatService : IChat
    {
        public bool AddFriend(int user, int friend)
        {
            throw new NotImplementedException();
        }

        public List<ConversationUserSM> ConversationUserList(int ID)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFriend(int user, int friend)
        {
            throw new NotImplementedException();
        }

        public List<UserSM> FindFriends(string keys)
        {
            throw new NotImplementedException();
        }

        public List<UserSM> FriendList(int user)
        {
            throw new NotImplementedException();
        }

        public bool InvitationAnswer(int user, int initiation, bool answer)
        {
            throw new NotImplementedException();
        }
    }
}
