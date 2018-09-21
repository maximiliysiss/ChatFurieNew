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

    }
}
