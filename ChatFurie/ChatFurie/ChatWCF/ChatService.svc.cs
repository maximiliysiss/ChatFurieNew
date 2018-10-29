using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ChatWCF.Services;
using ChatWCF.Models;
using ChatWCF.Models.SendModels;
using Microsoft.EntityFrameworkCore;

namespace ChatWCF
{
    public class ChatService : IChat
    {
        public bool AcceptFriend(int user, int notif)
        {
            ChatContextWCF ChatContextWCF = new ChatContextWCF();
            var addNotif = ChatContextWCF.AddFriendNotifications.Find(notif);
            var creator = ChatContextWCF.Users.Find(addNotif.UserFromID);
            var userAdd = ChatContextWCF.Users.Find(user);
            if (addNotif.IsDialog)
            {
                var conversation = ChatContextWCF.Conversation.Find(addNotif.DialogID);
                var userInConversation = new UserInConversation()
                {
                    Conversation = conversation,
                    DateStart = DateTime.Now,
                    User = userAdd,
                    Name = $"{conversation.Name}, {userAdd.Name}"
                };
                ChatContextWCF.Entry(userInConversation).State = EntityState.Added;
            }
            else
            {
                var conversation = new Conversation()
                {
                    Creator = creator,
                    DateStart = DateTime.Now,
                    Name = $"{creator.Name}, {userAdd.Name}",
                };
                ChatContextWCF.Entry(conversation).State = EntityState.Added;
                ChatContextWCF.Entry(new UserInConversation()
                {
                    Conversation = conversation,
                    DateStart = DateTime.Now,
                    Name = conversation.Name,
                    User = creator
                });
                ChatContextWCF.Entry(new UserInConversation()
                {
                    Conversation = conversation,
                    DateStart = DateTime.Now,
                    Name = conversation.Name,
                    User = userAdd
                });
            }

            ChatContextWCF.Entry(addNotif).State = EntityState.Deleted;
            ChatContextWCF.SaveChanges();
            return true;
        }

        public bool AddFriend(int user, int friend)
        {
            ChatContextWCF ChatContextWCF = new ChatContextWCF();
            var userFrom = ChatContextWCF.Users.Find(user);
            var userTo = ChatContextWCF.Users.Find(friend);
            ChatContextWCF.AddFriendNotifications.Add(new Models.AddFriendNotification()
            {
                Context = $"{userFrom.Name} want add you to friend",
                DateTime = DateTime.Now,
                UserFrom = userFrom,
                UserTo = userTo
            });
            ChatContextWCF.SaveChanges();
            return true;
        }

        public bool AddFriendToConversation(int user, int friend, int conversation)
        {
            ChatContextWCF ChatContextWCF = new ChatContextWCF();
            var userFrom = ChatContextWCF.Users.Find(user);
            var newFriend = ChatContextWCF.Users.Find(friend);
            var conversationAdd = ChatContextWCF.Conversation.Find(conversation);
            ChatContextWCF.Entry(new AddFriendNotification()
            {
                Context = $"{userFrom.Name} want add you to {conversationAdd.Name}",
                Conversation = conversationAdd,
                DateTime = DateTime.Now,
                IsDialog = true,
                UserFrom = userFrom,
                UserTo = newFriend
            }).State = EntityState.Added;
            ChatContextWCF.SaveChanges();
            return true;
        }

        public List<ConversationUserSM> ConversationUserList(int ID)
        {
            ChatContextWCF ChatContextWCF = new ChatContextWCF();
            var list = ChatContextWCF.UserInConversation.Where(x => x.UserID == ID).Select(x => new ConversationUserSM(x.Conversation));
            return new List<ConversationUserSM>();
        }

        public bool DeclineFriend(int user, int notif)
        {
            ChatContextWCF ChatContextWCF = new ChatContextWCF();
            var addNotif = ChatContextWCF.AddFriendNotifications.Find(notif);
            ChatContextWCF.Entry(addNotif).State = EntityState.Deleted;
            ChatContextWCF.SaveChanges();
            return true;
        }

        public bool DeleteFriend(int user, int friend, int conversation)
        {
            ChatContextWCF ChatContextWCF = new ChatContextWCF();
            var dFriend = ChatContextWCF.Users.Find(friend);
            var conversationD = ChatContextWCF.Conversation.Find(conversation);
            var userInConversation = ChatContextWCF.UserInConversation.Where(x => x.ConversationID == conversationD.ID);
            if (userInConversation.Count() == 2)
            {
                ChatContextWCF.UserInConversation.RemoveRange(userInConversation);
                ChatContextWCF.Entry(conversationD).State = EntityState.Deleted;
            }
            else
                ChatContextWCF.Entry(userInConversation.FirstOrDefault(x => x.UserID == friend)).State = EntityState.Deleted;
            ChatContextWCF.SaveChanges();
            return true;
        }

        public bool DeleteFriendFromConversation(int user, int friend, int conversation)
        {
            throw new NotImplementedException();
        }

        public List<UserSM> FindFriends(string keys, int user)
        {
            ChatContextWCF ChatContextWCF = new ChatContextWCF();
            var result = ChatContextWCF.Users.Where(x => x.Name.Contains(keys) && x.ID != user).Select(x => new UserSM(x)).Take(20).ToList();
            var conversations = ChatContextWCF.UserInConversation.Where(x => x.UserID == user).Select(x => x.Conversation).Take(20).ToList();
            result.AddRange(conversations.Select(x => new UserSM(x)));
            // Change logic!!!
            result.Reverse();
            return result;
        }

        public UserFriendSM GetUser(int user, int userFrom)
        {
            ChatContextWCF chatContextWCF = new ChatContextWCF();
            List<int> setUser = new List<int>() { user, userFrom };
            var userDB = chatContextWCF.Users.Find(user);
            bool isFriend = chatContextWCF.Conversation
                .Where(x => x.UserInConversations
                    .Where(y => y.UserID == user || y.UserID == userFrom).Count() > 0
                            && x.UserInConversations.Count() == 2)
                .Count() > 0
                || chatContextWCF.AddFriendNotifications
                .FirstOrDefault(x => setUser.In(x.UserFromID) && setUser.In(x.UserToID)) != null;
            return new UserFriendSM(userDB, isFriend);
        }

        public bool InvitationAnswer(int user, int initiation, bool answer)
        {
            throw new NotImplementedException();
        }
    }
}
