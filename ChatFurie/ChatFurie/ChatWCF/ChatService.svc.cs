using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ChatFurie.Models;
using ChatWCF.Models.SendModels;
using Microsoft.EntityFrameworkCore;

namespace ChatWCF
{
    public class ChatService : IChat
    {
        public bool AddFriend(int user, int friend)
        {
            return true;
        }

        public List<ConversationUserSM> ConversationUserList(int ID)
        {
            ChatContextWCF chatContextWCF = new ChatContextWCF();
            //var list = chatContextWCF.UserInConversation.Select(x => new ConversationUserSM(x));
            return new List<ConversationUserSM>();
        }

        public bool DeleteFriend(int user, int friend)
        {
            ChatContextWCF chatContextWCF = new ChatContextWCF();
            var dFriend = chatContextWCF.Users.Find(friend);
            if (dFriend != null)
                chatContextWCF.Entry(dFriend).State = EntityState.Deleted;
            else
                return false;
            return true;
        }

        public List<UserSM> FindFriends(string keys, int user)
        {
            ChatContextWCF chatContextWCF = new ChatContextWCF();
            var result = chatContextWCF.Users.Where(x => x.Name.Contains(keys) && x.ID != user).Select(x => new UserSM(x)).ToList();
            var conversations = chatContextWCF.UserInConversation.Where(x => x.UserID == user).Select(x => x.Conversation).ToList();
            result.AddRange(conversations.Select(x => new UserSM(x)));
            return result;
        }

        public List<UserSM> FindFriends(string keys)
        {
            throw new NotImplementedException();
        }

        public List<UserSM> FriendList(int user)
        {
            return new List<UserSM>();
        }

        public bool InvitationAnswer(int user, int initiation, bool answer)
        {
            return true;
        }
    }
}
