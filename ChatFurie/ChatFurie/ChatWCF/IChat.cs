using ChatFurie.Models.ChatModel;
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
        bool AddFriend(int user, int friend);

        [OperationContract]
        bool DeleteFriend(int user, int friend);

        [OperationContract]
        bool InvitationAnswer(int user, int initiation, bool answer);

        [OperationContract]
        List<UserSM> FriendList(int user);

        [OperationContract]
        List<UserSM> FindFriends(string keys);

        [OperationContract]
        List<ConversationUserSM> ConversationUserList(int ID); 
    }
}
