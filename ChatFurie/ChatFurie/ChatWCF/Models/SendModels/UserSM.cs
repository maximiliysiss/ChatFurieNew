using System.Runtime.Serialization;

namespace ChatWCF.Models.SendModels
{
    [DataContract]
    public class UserSM
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Image { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public bool IsConversation { get; set; }


        public UserSM(User user)
        {
            this.Email = user.Email;
            this.ID = user.ID;
            this.Image = user.Image;
            this.Login = user.Login;
            this.Name = user.Name;
            this.IsConversation = false;
        }

        public UserSM(Conversation conversation)
        {
            this.ID = conversation.ID;
            this.Name = conversation.Name;
            this.IsConversation = true;
        }
    }
}