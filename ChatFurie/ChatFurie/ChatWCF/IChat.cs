
using ChatWCF.Models;
using ChatWCF.Models.SendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatWCF
{
    [ServiceContract]
    interface IChat
    {
        [OperationContract]
        bool AddFriend(int user, int notif);

        [OperationContract]
        bool DeleteFriend(int user, int friend, int conversation);

        [OperationContract]
        bool InvitationAnswer(int user, int initiation, bool answer);

        [OperationContract]
        List<ConversationUserSM> ConversationUserList(int ID);

        [OperationContract]
        int AcceptFriend(int user, int newFriend);

        [OperationContract]
        bool DeclineFriend(int user, int newFriend);

        [OperationContract]
        bool AddFriendToConversation(int user, int friend, int conversation);

        [OperationContract]
        bool DeleteFriendFromConversation(int user, int friend, int conversation);

        [OperationContract]
        UserFriendSM GetUser(int user, int userFrom);

        [OperationContract]
        List<NotificationSM> GetNotifications(int user);

        [OperationContract]
        NotificationSM GetNotification(int user, int notification);

        [OperationContract]
        ConversationUserSM GetConversation(int user, int conversation);

        [OperationContract]
        ConversationInfo ConversationInfo(int conversation, int user);
    }
}
