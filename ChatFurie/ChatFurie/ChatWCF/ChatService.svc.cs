using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChatService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ChatService.svc or ChatService.svc.cs at the Solution Explorer and start debugging.
    public class ChatService : IChat
    {
        public bool AddFriend(int user, int friend)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFriend(int user, int friend)
        {
            throw new NotImplementedException();
        }

        public bool InvitationAnswer(int user, int initiation, bool answer)
        {
            throw new NotImplementedException();
        }
    }
}
