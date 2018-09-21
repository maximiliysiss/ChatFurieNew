using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatWCF
{
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
