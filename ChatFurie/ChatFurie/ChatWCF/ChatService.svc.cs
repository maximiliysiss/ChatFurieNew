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
using ChatWCF.ServicesAddings;

namespace ChatWCF
{
    public class ChatService : IChat
    {
        public int AcceptFriend(int user, int notif)
        {
            ChatContextWCF ChatContextWCF = new ChatContextWCF();
            var addNotif = ChatContextWCF.AddFriendNotifications.Find(notif);
            var creator = ChatContextWCF.Users.Find(addNotif.UserFromID);
            var userAdd = ChatContextWCF.Users.Find(user);
            Conversation conversation = null;
            if (addNotif.IsDialog)
            {
                conversation = ChatContextWCF.Conversation.Find(addNotif.DialogID);
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
                conversation = new Conversation()
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
                }).State = EntityState.Added;
                ChatContextWCF.Entry(new UserInConversation()
                {
                    Conversation = conversation,
                    DateStart = DateTime.Now,
                    Name = conversation.Name,
                    User = userAdd
                }).State = EntityState.Added;
            }

            ChatContextWCF.Entry(addNotif).State = EntityState.Deleted;
            ChatContextWCF.SaveChanges();
            return conversation.ID;
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

        public ConversationInfo ConversationInfo(int conversation, int user)
        {
            ChatContextWCF chatContextWCF = new ChatContextWCF();
            var conversationDB = chatContextWCF.Conversation.Find(conversation);
            return new ConversationInfo(conversationDB,
                chatContextWCF.UserInConversation.Where(x => x.ConversationID == conversation).Select(x => x.User).ToList())
            { IsCurrentUserAdmin = conversationDB.CreatorID == user };
        }

        public List<ConversationUserSM> ConversationUserList(int ID)
        {
            ChatContextWCF ChatContextWCF = new ChatContextWCF();
            var list = ChatContextWCF.UserInConversation.Where(x => x.UserID == ID).Select(x => new ConversationUserSM(x));
            return list.ToList();
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

        public bool DeleteUserFromConversation(int user, int conversation)
        {
            ChatContextWCF chatContextWCF = new ChatContextWCF();
            var chain = chatContextWCF.UserInConversation.FirstOrDefault(x => x.UserID == user && x.ConversationID == conversation);
            if (chain != null)
            {
                var conversationEntity = chatContextWCF.Conversation.Find(conversation);
                chatContextWCF.UserInConversation.Remove(chain);
                int count = chatContextWCF.UserInConversation.Where(x => x.ConversationID == conversation).Count();
                if (count == 0)
                    DeleteConversation(chatContextWCF, chatContextWCF.Conversation.Find(conversation));
                else if (conversationEntity.CreatorID == user)
                    ChangeConversationAdmin(conversation, chatContextWCF);
                chatContextWCF.SaveChanges();
            }
            return true;
        }


        /// <summary>
        /// Delete Conversation and all connected data
        /// </summary>
        /// <param name="chatContextWCF"></param>
        /// <param name="conversation"></param>
        private void DeleteConversation(ChatContextWCF chatContextWCF, Conversation conversation)
        {
            chatContextWCF.Conversation.Remove(conversation);
        }

        public List<UserSM> FindFriends(string keys, int user)
        {
            ChatContextWCF ChatContextWCF = new ChatContextWCF();
            var result = ChatContextWCF.Users.Where(x => x.Name.Contains(keys) && x.ID != user).Select(x => new UserSM(x)).Take(20).ToList();
            var conversations = ChatContextWCF.UserInConversation.Where(x => x.UserID == user && x.Name.Contains(keys)).Select(x => new UserSM(x.Conversation) { Image = x.Image }).Take(20).ToList();
            conversations.AddRange(result);
            return conversations;
        }

        public List<UserSM> FindOnlyFriends(string keys, int user, int conversation)
        {
            ChatContextWCF chatContextWCF = new ChatContextWCF();
            var usersInConversation = chatContextWCF.UserInConversation.Where(x => x.ConversationID == conversation).Select(x => x.UserID);
            var conversations = chatContextWCF.UserInConversation.Where(x => x.UserID == user)
                .Select(x => x.Conversation.UserInConversations).SelectMany(x => x)
                .Select(x => x.User).Where(x => !usersInConversation.Contains(x.ID)).ToList();
            if (keys != null)
                conversations.AddRange(chatContextWCF.Users.Where(x => x.Login.Contains(keys) && !usersInConversation.Contains(x.ID)));
            return conversations.Distinct().Select(x => new UserSM(x)).ToList();
        }

        public ConversationUserSM GetConversation(int user, int conversation)
        {
            ChatContextWCF chatContextWCF = new ChatContextWCF();
            var conversationEntity = chatContextWCF.Conversation.Find(conversation);
            var conversationDB = chatContextWCF.UserInConversation.FirstOrDefault(x => x.UserID == user && x.ConversationID == conversation);
            return new ConversationUserSM(conversationDB) { ConversationID = conversation, IsAdmin = user == conversationEntity.CreatorID };
        }

        public NotificationSM GetNotification(int user, int notification)
        {
            ChatContextWCF chatContextWCF = new ChatContextWCF();
            NotificationSM notificationSM;
            Notification common = chatContextWCF.CommonNotifications.Find(notification);
            if (common == null)
            {
                notificationSM = chatContextWCF.AddFriendNotifications.Where(x => x.ID == notification)
                    .Select(x => new NotificationSM(x, x.UserTo)).FirstOrDefault();
                return notificationSM;
            }
            return new NotificationSM(common as CommonNotification);
        }

        public List<NotificationSM> GetNotifications(int user)
        {
            ChatContextWCF chatContextWCF = new ChatContextWCF();
            List<NotificationSM> notifications = chatContextWCF.AddFriendNotifications
                .Where(x => !x.IsRead && x.UserToID == user).Take(20).Select(x => new NotificationSM(x, x.UserFrom)).ToList();
            notifications.AddRange(chatContextWCF.CommonNotifications
                .Where(x => !x.IsRead && x.UserID == user).Take(20).Select(x => new NotificationSM(x)));
            notifications.OrderByDescending(x => x.DateTime);
            return notifications;
        }

        public UserFriendSM GetUser(int user, int userFrom)
        {
            ChatContextWCF chatContextWCF = new ChatContextWCF();
            List<int> setUser = new List<int>() { user, userFrom };
            var userDB = chatContextWCF.Users.Find(user);
            var userConvers = chatContextWCF.Users.Where(x => x.ID == user).Select(x => x.Conversations);
            var anotherConvers = chatContextWCF.Users.Where(x => x.ID == userFrom).Select(x => x.Conversations);
            bool isFriend = chatContextWCF.UserInConversation.Where(x => setUser.In(x.UserID)).GroupBy(x => x.ConversationID)
                .Where(x => x.Count() == 2).Count() > 0;
            bool isAdding = chatContextWCF.AddFriendNotifications
                .FirstOrDefault(x => setUser.In(x.UserFromID) && setUser.In(x.UserToID)) != null;
            return new UserFriendSM(userDB, isFriend) { IsAdding = isAdding };
        }

        public bool InvitationAnswer(int user, int initiation, bool answer)
        {
            return answer ? AcceptFriend(user, initiation) > -1 : DeclineFriend(user, initiation);
        }

        public bool ChangeConversationAdmin(int conversation, ChatContextWCF chatContextWCF = null, int user = -1)
        {
            bool f = chatContextWCF == null;
            if (f)
                chatContextWCF = new ChatContextWCF();
            var conversationEntity = chatContextWCF.Conversation.Find(conversation);
            if (user == -1)
            {
                var usersConvers = chatContextWCF.UserInConversation.Where(x => x.Conversation == conversationEntity).ToList();
                user = usersConvers[Helpers.Random.Next() % usersConvers.Count].UserID;
            }

            conversationEntity.CreatorID = user;
            if (f)
                chatContextWCF.SaveChanges();

            return true;
        }
    }
}
