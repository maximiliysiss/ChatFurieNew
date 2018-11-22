using AuthServiceWCF.Helpers;
using AuthServiceWCF.Models;
using ChatWCF.Models.SendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AuthServiceWCF
{
    [ServiceContract]
    public interface IAuthService
    {

        [OperationContract]
        UserPageModel GetUserSettings(int id);

        [OperationContract]
        IActionResultWCF Login(LoginModel loginModel);

        [OperationContract]
        IActionResultWCF Register(RegisterModel registerModel);

        [OperationContract]
        IActionResultWCF SaveSettingsUser(UserPageModel userPageModel);
    }
}
